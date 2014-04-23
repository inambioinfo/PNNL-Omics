﻿using System.Linq.Expressions;
using MathNet.Numerics;
using PNNLOmics.Algorithms.FeatureClustering;
using PNNLOmics.Algorithms.SpectralProcessing;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using PNNLOmics.Extensions;

namespace PNNLOmics.Algorithms.Chromatograms
{
    public class XicCreator
    {
        private const int CONST_POLYNOMIAL_ORDER = 3;

        public XicCreator()
        {
            ScanWindowSize          = 100;
            FragmentationSizeWindow = .5;
            NumberOfPoints          = 5;
        }

        public event EventHandler<ProgressNotifierArgs> Progress;

        private void OnProgress(string message)
        {
            if (Progress != null)
            {
                Progress(this, new ProgressNotifierArgs(message));
            }
        }

        public IEnumerable<UMCLight> CreateXic(IList<UMCLight> features, 
                              double            massError,
                              ISpectraProvider  provider)
        {
            // this algorithm works as follows
            // 
            //  PART A - Build the XIC target list 
            //  For each UMC Light , find the XIC representation
            //      for each charge in a feature          
            //          from start scan to end scan
            //              1. Compute a lower / upper m/z bound
            //              2. build an XIC chomatogram object
            //              3. reference the original UMC Feature -- this allows us to easily add 
            //                  chromatograms to the corresponding feature
            //              4. store the chomatogram (with unique ID across all features)
            //
            //  PART B - Read Data From File
            //  Sort the list of XIC's by scan
            //  for each scan s = start scan to end scan 
            //      1. find all xic's that start before and end after s - 
            //          a. cache these xics in a dictionary based on unique id
            //          b. NOTE: this is why we sort so we can do an O(N) search for 
            //             all XIC's that need data from this scan s
            //      2.  Then for each XIC that needs data
            //          a. Pull intensity data from lower / upper m/z bound 
            //          b. create an MS Feature
            //          c. store in original UMC Feature
            //          d. Test to see if the XIC is done building (Intensity < 1 or s > scan end)
            //      3. Remove features that are done building from cache
            // 
            //  CONCLUSIONS
            //  Building UMC's then takes linear time  (well O(N Lg N) time if you consider sort)
            //      and theoretically is only bounded by the time it takes to read an entire raw file
            // 
            if (features.Count <= 0) 
                throw new Exception("No features were available to create XIC's from");

            var minScan = Math.Max(0, features.Min(x => x.Scan - ScanWindowSize));
            var maxScan = features.Max(x => x.Scan + ScanWindowSize);

            OnProgress("Sorting features for optimized scan partitioning");
            // PART A 
            // Map the feature ID to the xic based features
            var xicFeatures = new SortedSet<XicFeature>();
            var allFeatures = CreateXicTargets(features, massError);

            // PART B 
            // sort the features...
            var m           = allFeatures.Count;
            allFeatures     = allFeatures.OrderBy(x => x.StartScan).ToList();

            // This map tracks all possible features to keep
            var featureMap  = new Dictionary<int, XicFeature>();
            int j           = 0;  // this is our feature index
            int msFeatureId = 0;
            
            // This list stores a temporary amount of parent MS features
            // so that we can link MS/MS spectra to MS Features
            var parentMsList = new List<MSFeatureLight>();

            // Creates a comparison function for building a BST from a spectrum.
            var msmsFeatureId = 0;

            var N = provider.GetTotalScans(0);
            OnProgress(string.Format("Analyzing {0} scans", N));


            // Iterate over all the scans...
            for (int s = minScan; s < maxScan && s < N; s++)
            {
                // Find any features that need data from this scan, s 
                while (j < m)
                {
                    var xicFeature = allFeatures[j];
                    // This means that no new features were eluting with this scan....
                    if (xicFeature.StartScan > s)
                        break;

                    // This means that there is a new feature...
                    if (s < xicFeature.EndScan)
                    {
                        xicFeatures.Add(xicFeature);
                    }
                    j++;
                }

                // Skip pulling the data from the file if there is nothing to pull from.
                if (xicFeatures.Count < 1)
                    continue;

                // Here We link the MSMS Spectra to the UMC Features
                var summary           = new ScanSummary();
                List<XYData> spectrum = provider.GetRawSpectra(s, 0, 1, out summary);                
                                
                if (summary.MsLevel > 1)
                {
                    // If it is an MS 2 spectra... then let's link it to the parent MS
                    // Feature
                    var matching = parentMsList.FindAll(
                        x => Math.Abs(x.Mz - summary.PrecursorMZ) <= FragmentationSizeWindow
                        );
                    
                    foreach (var match in matching)
                    {
                        // We create multiple spectra because this guy is matched to multiple ms
                        // features
                        var spectraData = new MSSpectra
                        {
                            ID              = msmsFeatureId,
                            ScanMetaData    = summary,
                            CollisionType   = summary.CollisionType,
                            Scan            = s,
                            MSLevel         = summary.MsLevel,
                            PrecursorMZ     = summary.PrecursorMZ,
                            TotalIonCurrent = summary.TotalIonCurrent
                        };

                        match.MSnSpectra.Add(spectraData);
                        spectraData.ParentFeature = match;
                    }
                    msmsFeatureId++;

                    continue;
                }



                var sortedList = spectrum.OrderBy(x => x.X).ToList();
                

                // Tracks which spectra need to be removed from the cache
                var toRemove = new List<XicFeature>();
                // Tracks which features we need to link to MSMS spectra with
                parentMsList.Clear();
                
                // now we iterate through all features that need data from this scan
                //foreach (var xic in xicFeatures.Values)
                int k = 0;
                foreach (var xic in xicFeatures)
                {                    
                    var lower  = xic.LowMz;
                    var higher = xic.HighMz;


                    while (k < sortedList.Count && sortedList[k].X < lower)
                    {
                        k++;
                    }

                    double summedIntensity = 0;
                    while (k < sortedList.Count && sortedList[k].X <= higher)
                    {
                        summedIntensity += sortedList[k++].Y;
                    }                    
                    // See if we need to remove this feature
                    // We only do so if the intensity has dropped off and we are past the end of the feature.
                    if (summedIntensity < 1 && xic.EndScan < s)
                    {
                        toRemove.Add(xic);
                        continue;
                    }

                    var umc = xic.Feature;

                    // otherwise create a new feature here...
                    var msFeature               = new MSFeatureLight
                    {
                        ChargeState      = xic.ChargeState,   
                        Mz               = xic.Mz,
                        MassMonoisotopic = umc.MassMonoisotopic,
                        Scan             = s,
                        Abundance        = Convert.ToInt64(summedIntensity),
                        ID               = msFeatureId++,
                        DriftTime        = umc.DriftTime,
                        RetentionTime    = s,
                        GroupID          = umc.GroupID
                    };
                    parentMsList.Add(msFeature);
                    xic.Feature.AddChildFeature(msFeature);
                }

                toRemove.ForEach(x => xicFeatures.Remove(x));
            }

            OnProgress("Filtering bad features with no data.");
            features = features.Where(x => x.MSFeatures.Count > 0).ToList();
            
            OnProgress("Refining XIC features.");
            return RefineFeatureXics(features);
            
        }

        private IEnumerable<UMCLight> RefineFeatureXics(IList<UMCLight> features)
        {
            // Here we smooth the points...and remove any features with from and trailing zero points
            var numberOfPoints = NumberOfPoints;
            var smoother = new SavitzkyGolaySmoother(numberOfPoints, CONST_POLYNOMIAL_ORDER, false);
            
            foreach (var feature in features)
            {
                var map = feature.CreateChargeMap();

                // Clear the MS Feature List 
                // Because we're going to refine each charge state then fix the length of the feature
                // from it's known max abundance value.                
                feature.MSFeatures.Clear();


                // Work on a single charge state since XIC's have different m/z values
                foreach (var chargeFeatures in map.Values)
                {
                    var xic = new List<XYData>();
                    var msFeatures = chargeFeatures.Where(x => x.Abundance > 0).OrderBy(x => x.Scan).ToList();
                    msFeatures.ForEach(x => xic.Add(new XYData(x.Scan, x.Abundance)));

                    var points = smoother.Smooth(xic);
                    if (msFeatures.Count <= 0) continue;

                    // Find the biggest peak...
                    var maxScanIndex  = 0;
                    long maxAbundance = 0;
                    for (int i = 0; i < msFeatures.Count; i++)
                    {
                        msFeatures[i].Abundance = Convert.ToInt64(points[i].Y);

                        if (maxAbundance < msFeatures[i].Abundance)
                        {
                            maxScanIndex = i;
                            maxAbundance = msFeatures[i].Abundance;
                        }
                    }

                    // Then find when the feature goes to zero
                    // Start from max to left                        
                    var startIndex = maxScanIndex;

                    // If we hit zero, then keep
                    for (; startIndex > 0; startIndex--)
                    {
                        if (msFeatures[startIndex].Abundance < 1)
                            break;
                    }

                    // Start from max to right
                    var stopIndex = maxScanIndex;
                    for (; stopIndex < msFeatures.Count - 1; stopIndex++)
                    {
                        if (msFeatures[stopIndex].Abundance < 1)
                            break;
                    }

                    // Add the features back
                    for (var i = startIndex; i <= stopIndex; i++)
                    {
                        feature.AddChildFeature(msFeatures[i]);
                    }
                }

                // Clean up 
            }
            return features.Where(x => x.MSFeatures.Count > 0).ToList();
        }

        /// <summary>
        /// Creates XIC Targets from a list of UMC Features
        /// </summary>
        /// <param name="features"></param>
        /// <param name="massError"></param>
        /// <returns></returns>
        private  List<XicFeature> CreateXicTargets(IEnumerable<UMCLight> features, double massError)
        {
            
            var allFeatures = new List<XicFeature>();

            // Create XIC Features
            var id = 0;
            // Then for each feature turn it into a new feature
            foreach (var feature in features)
            {
                // Build XIC features from each
                var x = feature.CreateChargeMap();
                foreach (var charge in x.Keys)
                {
                    long maxIntensity = 0;
                    double mz = 0;
                    var min = double.MaxValue;
                    var max = double.MinValue;

                    int scanStart = int.MaxValue;
                    int scanEnd = 0;

                    foreach (var chargeFeature in x[charge])
                    {
                        min         = Math.Min(min, chargeFeature.Mz);
                        max         = Math.Max(max, chargeFeature.Mz);
                        scanStart   = Math.Min(scanStart, chargeFeature.Scan);
                        scanEnd     = Math.Min(scanStart, chargeFeature.Scan);

                        if (chargeFeature.Abundance > maxIntensity)
                        {
                            maxIntensity = chargeFeature.Abundance;
                            mz = chargeFeature.Mz;
                        }
                    }

                    // Clear the ms feature list...because later we will populate it
                    feature.MSFeatures.Clear();

                    var xicFeature = new XicFeature()
                    {
                        Area        = 0,
                        HighMz      = Feature.ComputeDaDifferenceFromPPM(mz, -massError),
                        LowMz       = Feature.ComputeDaDifferenceFromPPM(mz, massError),
                        Mz          = mz,
                        Feature     = feature,
                        Id          = id++,
                        EndScan     = scanEnd + ScanWindowSize,
                        StartScan   = scanStart - ScanWindowSize,
                        ChargeState = charge
                    };

                    allFeatures.Add(xicFeature);
                }
            }

            return allFeatures;
        }

        /// <summary>
        /// Creates an XIC from the given set of target features.
        /// </summary>
        /// <param name="massError">Mass error to use when pulling peaks</param>
        /// <param name="msFeatures">Seed features that provide the targets</param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public IEnumerable<MSFeatureLight> CreateXic(   IList<MSFeatureLight> msFeatures, 
                                                        double massError,                  
                                                        ISpectraProvider provider)
        {
            var newFeatures = new List<MSFeatureLight>();

            if (msFeatures.Count <= 0) return newFeatures;

            var minScan  = msFeatures[0].Scan;
            var maxScan  = msFeatures[msFeatures.Count - 1].Scan;
            minScan     -= 100;
            maxScan     += 100;
            minScan      = Math.Max(0, minScan); 

            var  min             = double.MaxValue;
            var  max             = double.MinValue;
            long maxIntensity    = 0;
            var  featureMap      = new Dictionary<int, MSFeatureLight>();
            double mz            = 0;
            foreach (var chargeFeature in msFeatures)
            {
                min = Math.Min(min, chargeFeature.Mz);
                max = Math.Max(max, chargeFeature.Mz);                    

                if (chargeFeature.Abundance > maxIntensity)
                {
                    maxIntensity = chargeFeature.Abundance;
                    mz           = chargeFeature.Mz;
                }

                // Map the feature...
                if (!featureMap.ContainsKey(chargeFeature.Scan))
                {
                    featureMap.Add(chargeFeature.Scan, chargeFeature);
                }
            }

            var features = CreateXic(mz, massError, minScan, maxScan, provider);
            foreach (var msFeature in features)
            {
                var scan = msFeature.Scan;
                if (featureMap.ContainsKey(msFeature.Scan))                
                    featureMap[scan].Abundance = msFeature.Abundance;                                
                newFeatures.Add(msFeature);                
            }
            return newFeatures;
        }
        /// <summary>
        /// Creates an XIC from the m/z values provided.
        /// </summary>
        /// <param name="mz"></param>
        /// <param name="massError"></param>
        /// <param name="minScan"></param>
        /// <param name="maxScan"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public IEnumerable<MSFeatureLight> CreateXic(double mz,
                                                     double massError,
                                                     int minScan,
                                                     int maxScan,
                                                     ISpectraProvider provider)
        {

            var newFeatures = new List<MSFeatureLight>();
            var lower  = Feature.ComputeDaDifferenceFromPPM(mz, massError);
            var higher = Feature.ComputeDaDifferenceFromPPM(mz, -massError);

                        

            for (var i = minScan; i < maxScan; i++)
            {
                List<XYData> spectrum = null;

                try
                {
                    var summary = new ScanSummary();
                    spectrum = provider.GetRawSpectra(i, 0, 1, out summary);
                }catch
                {
                    
                }

                if (spectrum == null)
                    continue;

                var data = (from x in spectrum
                            where x.X > lower && x.X < higher
                            select x).ToList();

                var summedIntensity = data.Sum(x => x.Y);

               
                var newFeature = new MSFeatureLight
                {
                    Scan = i,
                    RetentionTime = i,
                    Abundance = Convert.ToInt64(summedIntensity)
                };
                newFeatures.Add(newFeature);                
            }
            return newFeatures;   
        }

        public IDictionary<int, IList<MSFeatureLight>> CreateXic(UMCLight feature, double massError, ISpectraProvider provider)
        {
            var features        = new Dictionary<int, IList<MSFeatureLight>>();
            var chargeFeatures  = feature.CreateChargeMap();

            // For each UMC...
            foreach (var charge in chargeFeatures.Keys)
            {
                // Find the mininmum and maximum features                             
                var msFeatures = CreateXic(chargeFeatures[charge], 
                                            massError,
                                            provider);

                features.Add(charge, new List<MSFeatureLight>());

                foreach (var newFeature in msFeatures)
                {
                    // Here we ask if this is a new MS Feature or old...
                    if (!chargeFeatures.ContainsKey(newFeature.Scan))
                    {
                        // Otherwise add the new feature
                        newFeature.MassMonoisotopic = feature.MassMonoisotopic;
                        newFeature.DriftTime        = feature.DriftTime;
                        newFeature.GroupID          = feature.GroupID;
                    }
                    features[charge].Add(newFeature);
                }
            }
            return features;
        }
        /// <summary>
        /// Gets or sets how many scans to add before and after an initial XIC target
        /// </summary>
        public int ScanWindowSize { get; set; }
        /// <summary>
        /// Gets or sets the size of the m/z window to use when linking MS Features to MS/MS spectra
        /// </summary>
        public double FragmentationSizeWindow { get; set; }


        public int NumberOfPoints { get; set; }
    }

}