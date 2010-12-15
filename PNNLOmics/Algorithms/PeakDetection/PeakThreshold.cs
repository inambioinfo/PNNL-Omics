﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.PeakDetector
{
    public class PeakThresholdParameters
    {
        public bool isDataThresholded { get; set; }
        public List<XYData> ThresholdedPeakData { get; set; }//results Npoints Long
        public List<double> ThresholdedPeakFWHM { get; set; }//results NPoints Long
        public List<double> ThresholdedPeakSNShoulder {get;set;}//resutls NpointsLong
        public List<double> ThresholdedPeakSN { get; set; }//resutls NpointsLong
        public List<double> ThresholdedPeakSignalToBackground { get; set; }//results NpointsLong
        public List<ThresholdpeakObject> ThresholdedObjectlist { get; set; }//NpointsLong
        public float SignalToShoulderCuttoff { get; set; }
        public int ScanNumber { get; set; }

        public PeakThresholdParameters()
        {
            this.isDataThresholded = false;
            this.ThresholdedPeakData = new List<XYData>();
            this.ThresholdedPeakFWHM = new List<double>();
            this.ThresholdedPeakSNShoulder = new List<double>();
            this.ThresholdedPeakSN = new List<double>();
            this.ThresholdedPeakSignalToBackground = new List<double>();
            this.SignalToShoulderCuttoff = 3;
            this.ThresholdedObjectlist = new List<ThresholdpeakObject>();
            this.ScanNumber = 0;
        }
    }

    public class ThresholdpeakObject
    {
        public XYData PeakData { get; set; }
        public double PeakFWHM { get; set; }
        public double PeakSNShoulder { get; set; }
        public double PeakSignalToBackground { get; set; }
        public double PeakSN { get; set; }

        public ThresholdpeakObject()
        {
            this.PeakData = new XYData(0, 0);
            this.PeakFWHM = 0;
            this.PeakSNShoulder = 0;
            this.PeakSignalToBackground = 0;
            this.PeakSN = 0;
        }
    }

    public class PeakThreshold
    {
        /// <summary>
        /// calculate mean of the noise (aka mean of the data) then calculate the standard deviation of the noise (aks data)
        /// if peak is above Xsigma+mean reject
        /// FWHM thresholding is not proving to be usefull yet.
        /// </summary>
        /// <param name="peakData">input peaks we want to threshold</param>
        /// <param name="peakShoulderNoise">lowest minima point intensity surrounding the peak</param>
        /// <param name="localMinimaData">index of minima on each side of point X=left, Y=right</param>
        /// <param name="parameters">Peakthreshold parameters</param>
        //public static void ApplyThreshold(ref List<XYData> peakData, List<double> peakShoulderNoise, List<XYData> localMinimaData, List<double> FWHMList, PeakThresholdParameters parameters)
        public static List<ProcessedPeak> ApplyThreshold(ref List<ProcessedPeak> peakList, PeakThresholdParameters parameters)
        {
            List<ProcessedPeak> ResultListThresholded = new List<ProcessedPeak>();
            //int numPoints = peakData.Count;
            int numPoints = peakList.Count;

            double signaltoShoulder = 0;
            double signaltoBackground = 0;
            double signaltoNoise = 0;
            double thresholdIntensity = 0;

            if (!parameters.isDataThresholded)
            {
                #region calculate average noise value and average shoulderNoiseLeve  = baseline
                double averageShoulderNoise = 0;
                double averagePeakNoise = 0;
                
                for (int i = 0; i < numPoints; i++)
                {
                    //averageShoulderNoise += peakShoulderNoise[i];
                    //averagePeakNoise += peakData[i].Y;
                    averageShoulderNoise += peakList[i].LocalLowestMinimaHeight;
                    averagePeakNoise += peakList[i].Height;
                }
                #endregion
                averageShoulderNoise /= numPoints;//baseline
                averagePeakNoise /= numPoints;//works if the noise dominates the spectra

                #region calculate standard deviation
                double stdevDeviationsSquared = 0;
                double stdevDeviations = 0;
                double stdevSumDeviationsSquared = 0;
                double standardDevAllSignal =0;
                double MAD = 0; //Median Absolute Deviation
                double stdevMAD = 0;//standard deviation derived from MAD

                List<ProcessedPeak> sortedCentroidedPeak = new List<ProcessedPeak>();
                List<double> medanDeviationList = new List<double>();

                sortedCentroidedPeak = peakList.OrderBy(p => p.Height).ToList();

                double median = sortedCentroidedPeak[(int)(sortedCentroidedPeak.Count / 2)].Height;//if it is sorted.

                double medianDeviations = 0;

                for (int i = 0; i < numPoints; i++)
                {
                    stdevDeviations = (peakList[i].Height - averagePeakNoise);
                    stdevDeviationsSquared = stdevDeviations * stdevDeviations;
                    stdevSumDeviationsSquared += stdevDeviationsSquared;

                    medianDeviations = Math.Abs(peakList[i].Height - median);
                    medanDeviationList.Add(medianDeviations);
                }
                medanDeviationList.Sort();
                MAD = medanDeviationList[(int)(medanDeviationList.Count / 2)];
                
                
                #endregion
                stdevMAD = MAD * 1.4826;
                standardDevAllSignal = Math.Sqrt(stdevSumDeviationsSquared / (numPoints - 1));
                
                for (int i = 0; i < numPoints; i++)
                {
                    ProcessedPeak thresholdedPeak = new ProcessedPeak();

                    signaltoShoulder = peakList[i].Height / peakList[i].LocalLowestMinimaHeight;
                    signaltoBackground = peakList[i].Height / averageShoulderNoise;
                    signaltoNoise = peakList[i].Height / averagePeakNoise;

                    thresholdIntensity = parameters.SignalToShoulderCuttoff * stdevMAD + averagePeakNoise;

                    if (peakList[i].Height >= thresholdIntensity)
                    {
                        //include high abundant peaks
                        thresholdedPeak = peakList[i];
                        thresholdedPeak.SignalToNoiseGlobal = signaltoNoise;
                        thresholdedPeak.SignalToNoiseLocalMinima = signaltoShoulder;
                        thresholdedPeak.SignalToBackground = signaltoBackground;
                       
                        ResultListThresholded.Add(thresholdedPeak);// parameters.ThresholdedPeakData.Add(thresholdedPeak);
                    }
                    else
                    {
                        //TODO: deal with low abundant peaks base on FWHM or some other criteria.  Perhaps all data should be filtered on FWHM
                    }
                }

                //TODO: possible FWHM filtering
                ////now that we have a global threshold, repeat filter by FWHM for the most abundant peaks so we can redraw the threshold line
                //convert to similar triangles
                //bool shouldWeThresholdByFWHM = false;
                //if (shouldWeThresholdByFWHM)
                //{
                //    #region peakfiltering by FWHM
                //    //List<XYData> sortedPeaks = new List<XYData>();
                //    parameters.ThresholdedObjectlist = parameters.ThresholdedObjectlist.OrderBy(p => p.PeakData.Y).ToList();
                //    //parameters.ThresholdedPeakData = sortedPeaks;
                //    double topPeaksFactor = 0.25;//how much of the list do we to use for our average FWHM.  0.5 means use top half of peaks above threshold
                //    double averageFWHM = 0;

                //    //calculate average
                //    for (int i = 0; i < (int)(parameters.ThresholdedObjectlist.Count * topPeaksFactor); i++)
                //    {
                //        averageFWHM += parameters.ThresholdedObjectlist[i].PeakFWHM;
                //    }
                //    averageFWHM /= parameters.ThresholdedObjectlist.Count;

                //    stdevSumDeviationsSquared = 0;
                //    //calculate standard deviation
                //    for (int i = 0; i < parameters.ThresholdedObjectlist.Count; i++)
                //    {
                //        stdevDeviations = (parameters.ThresholdedObjectlist[i].PeakFWHM - averageFWHM);
                //        stdevDeviationsSquared = stdevDeviations * stdevDeviations;
                //        stdevSumDeviationsSquared += stdevDeviationsSquared;
                //    }

                //    double standardFWHM = Math.Sqrt(stdevSumDeviationsSquared / (parameters.ThresholdedObjectlist.Count - 1));

                //    int y = 6;
                //    y = y * (int)standardFWHM;
                //    #endregion
                //}
            }
            else//add all peaks since the data is thresholded already ot setup some sort of other cuttoff
            {
                for (int i = 0; i < numPoints; i++)
                {
                    //include all peaks
                    ProcessedPeak thresholdedPeak = new ProcessedPeak();

                    thresholdedPeak = peakList[i];
                    ResultListThresholded.Add(thresholdedPeak);// parameters.ThresholdedPeakData.Add(thresholdedPeak);
                }
            }
            return ResultListThresholded;
        }
    }
}