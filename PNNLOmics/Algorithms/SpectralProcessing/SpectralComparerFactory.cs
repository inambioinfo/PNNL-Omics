﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Algorithms.SpectralComparisons;

namespace PNNLOmics.Algorithms.SpectralProcessing
{
    public class SpectralComparerFactory
    {
        /// <summary>
        /// Creates a spectral comparer based on the comparison type.
        /// </summary>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static ISpectralComparer CreateSpectraComparer(SpectralComparison comparisonType, double percent = .4)
        {
            ISpectralComparer comparer = null;
            switch (comparisonType)
            {
                case SpectralComparison.DotProduct:
                    comparer = new SpectralDotProductComprarer(percent);
                    break;
                case SpectralComparison.NormalizedDotProduct:
                    comparer = new SpectralNormalizedDotProductComparer(percent);
                    break;
                case SpectralComparison.BinaryDotProduct:
                    comparer = new BinaryDotProduct(percent);
                    break;
                case SpectralComparison.CosineDotProduct:
                    comparer = new SpectralCosineComparer();
                    break;
                case SpectralComparison.PeakCounts:
                    comparer = new SpectralPeakCountComparer();
                    break;
                default:
                    break;
            }
            return comparer;
        }
    }


    public enum SpectralComparison
    {
        DotProduct,
        NormalizedDotProduct,
        BinaryDotProduct,
        CosineDotProduct,
        PeakCounts
    }
}
