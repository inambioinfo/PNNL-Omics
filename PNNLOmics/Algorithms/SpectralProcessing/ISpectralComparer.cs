﻿using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.SpectralComparisons
{
    /// <summary>
    /// Interface for spectral comparison algorithms
    /// </summary>
    [System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.SpectralProcessing")]
    public interface ISpectralComparer
    {
        /// <summary>
        /// Compares two spectra together.
        /// </summary>
        /// <param name="spectraX"></param>
        /// <param name="spectraY"></param>
        /// <returns>Score based on how similar they are.</returns>
        double CompareSpectra(MSSpectra spectraX, MSSpectra spectraY);
    }
}
