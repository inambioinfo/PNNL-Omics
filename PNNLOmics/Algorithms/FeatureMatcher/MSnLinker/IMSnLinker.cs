﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureMatcher.MSnLinker
{
    /// <summary>
    /// Interface for linking features together from MSMS to MSn
    /// </summary>
    public interface IMSnLinker
    {

        /// <summary>
        /// Gets or sets the feature tolerances to use.
        /// </summary>
        FeatureTolerances Tolerances
        {
            get;
            set;
        }


        /// <summary>
        /// Links MS Features to MSMS Spectra.
        /// </summary>
        /// <param name="features"></param>
        /// <param name="spectra"></param>
        /// <returns>The number of a times a MSn spectra was mapped to a feature using the spectrum ID as a key.</returns>
        Dictionary<int, int> LinkMSFeaturesToMSn(List<MSFeatureLight> features, List<MSSpectra> spectra);               
    }
}