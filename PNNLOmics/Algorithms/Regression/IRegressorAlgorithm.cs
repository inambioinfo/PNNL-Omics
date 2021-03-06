﻿#region Namespaces

using System.Collections.Generic;
using PNNLOmics.Annotations;

#endregion

namespace PNNLOmics.Algorithms.Regression
{
    [System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.Regression")]
    public interface IRegressorAlgorithm<T>
    {
        [UsedImplicitly]
        T CalculateRegression(IEnumerable<double> observed, IEnumerable<double> predicted);
        [UsedImplicitly]
        double Transform(T regressionFunction, double observed);        
    }
}
