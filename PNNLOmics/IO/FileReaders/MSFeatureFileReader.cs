﻿using System;
using System.Collections.Generic;
using System.IO;
using PNNLOmics.Data.Features;

namespace PNNLOmics.IO.FileReaders
{
    /// <summary>
    /// Decon2ls (MS Feature) file text reader.
    /// </summary>
    public class MSFeatureFileReader : BaseTextFileReader<MSFeature>
    {
        #region Column constants
        private const string SCAN_NUMBER        = "scan";
        private const string FRAME_NUMBER       = "frame";
        private const string MZ                 = "mz";
        private const string ABUNDANCE          = "abundance";
        private const string ISOTOPIC_FIT       = "fit";
        private const string CHARGE             = "charge";
        private const string MONO_MASS          = "monoMass";
        private const string AVERAGE_MASS       = "averageMass";
        private const string ABUNDANT_MASS      = "abundantMass";
        private const string FWHM               = "fwhm";
        private const string MONO_ABUNDANCE     = "monoAbundance";
        private const string MONO_2_ABUNDANCE   = "mono2Abundance";
        private const string SNR                = "SNR";
        #endregion

        /// <summary>
        /// Parses the column header text into a map of name column index.
        /// </summary>
        /// <param name="textReader"></param>
        /// <returns></returns>
		protected override Dictionary<string, int> CreateColumnMapping(TextReader textReader)
		{            

			Dictionary<string, int> columnMap   = new Dictionary<string, int>();
			string[] columnTitles               = textReader.ReadLine().Split('\t', '\n');
			int numOfColumns                    = columnTitles.Length;
           

			for (int i = 0; i < numOfColumns; i++)
			{
                string title    = columnTitles[i].Trim();
                title           = title.ToLower();
                
				switch (columnTitles[i].Trim())
				{
                    case "frame_num":
                        columnMap.Add(FRAME_NUMBER, i);
                        break;
                    case "scan_num":                        
						columnMap.Add(SCAN_NUMBER, i);
                        break;
                    case "charge":                 
						columnMap.Add(CHARGE, i);
                        break;
                    case "abundance":	                 
						columnMap.Add(ABUNDANCE, i);
                        break;
                    case "mz":                 
						columnMap.Add(MZ, i);
                        break;	
                    case "fit":	                 
						columnMap.Add(ISOTOPIC_FIT, i);
                        break;
                    case "average_mw":	                 
						columnMap.Add(AVERAGE_MASS, i);
                        break;
                    case "monoisotopic_mw":                 
						columnMap.Add(MONO_MASS, i);
                        break;
                    case "mostabundant_mw":	                 
						columnMap.Add(ABUNDANT_MASS, i);
                        break;
                    case "fwhm":	                 
						columnMap.Add(FWHM, i);
                        break;
                    case "signal_noise":                 
						columnMap.Add(SNR, i);
                        break;
                    case "mono_abundance":                 
						columnMap.Add(MONO_ABUNDANCE, i);
                        break;
                    case "mono_plus2_abundance":                 
						columnMap.Add(MONO_2_ABUNDANCE, i);
                        break;					
					default:						
						break;
				}
			}
			return columnMap;
		}
        /// <summary>
        /// Saves the MS feature data to a list.
        /// </summary>
        /// <param name="textReader"></param>
        /// <param name="columnMapping"></param>
        /// <returns></returns>
		protected override IEnumerable<MSFeature> SaveFileToEnumerable(TextReader textReader, Dictionary<string, int> columnMapping)
		{
			List<MSFeature>    features     = new List<MSFeature>();			
			int                currentId    = 0;			
			string             line         = "";			

            // Detect if the data comes from an IMS platform.
            bool hasDriftTimeData   = columnMapping.ContainsKey(FRAME_NUMBER); 
			
			while ((line = textReader.ReadLine()) != null)
			{
				string[] columns    = line.Split(new string [] {Delimeter}, 0, StringSplitOptions.RemoveEmptyEntries);												
				MSFeature feature   = new MSFeature();
				feature.ID          = currentId;


                // In case this file does not have drift time, we need to make sure we clean up the
                // feature so the downstream processing can complete successfully.
                if (!hasDriftTimeData)
                {                    
                    if (columnMapping.ContainsKey(SCAN_NUMBER))     feature.RetentionTime       = float.Parse(columns[columnMapping[SCAN_NUMBER]]);
                }
                else
                {
                    if (columnMapping.ContainsKey(FRAME_NUMBER))    feature.RetentionTime       = float.Parse(columns[columnMapping[FRAME_NUMBER]]);
                    if (columnMapping.ContainsKey(SCAN_NUMBER))     feature.ScanIMS             = int.Parse(columns[columnMapping[SCAN_NUMBER]]);
                }
                if (columnMapping.ContainsKey(CHARGE))              feature.ChargeState         = int.Parse(columns[columnMapping[CHARGE]]);
                if (columnMapping.ContainsKey(ABUNDANCE))           feature.Abundance           = long.Parse(columns[columnMapping[ABUNDANCE]]);
                if (columnMapping.ContainsKey(MZ))                  feature.MZ                  = float.Parse(columns[columnMapping[MZ]]);
                if (columnMapping.ContainsKey(ISOTOPIC_FIT))        feature.Fit                 = float.Parse(columns[columnMapping[ISOTOPIC_FIT]]);
                if (columnMapping.ContainsKey(MONO_MASS))           feature.MassMonoisotopic    = float.Parse(columns[columnMapping[MONO_MASS]]);
                if (columnMapping.ContainsKey(FWHM))                feature.Fwhm                = float.Parse(columns[columnMapping[FWHM]]);
                if (columnMapping.ContainsKey(SNR))                 feature.SignalToNoiseRatio  = float.Parse(columns[columnMapping[SNR]]);
                if (columnMapping.ContainsKey(FWHM))                feature.Fwhm                = float.Parse(columns[columnMapping[FWHM]]);
                if (columnMapping.ContainsKey(MONO_ABUNDANCE))      feature.AbundanceMono       = int.Parse(columns[columnMapping[MONO_ABUNDANCE]]);
                if (columnMapping.ContainsKey(MONO_2_ABUNDANCE))    feature.AbundancePlus2      = int.Parse(columns[columnMapping[MONO_2_ABUNDANCE]]);
                
                features.Add(feature);
                currentId++;
            }
			return features;
		}
    }
}
