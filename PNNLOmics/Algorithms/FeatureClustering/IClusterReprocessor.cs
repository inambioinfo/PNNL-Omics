﻿using System.Collections.Generic;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureClustering
{
	[System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.Clustering")]
    public interface IClusterReprocessor<T, U> 
        where T : FeatureLight, IChildFeature<U>, new()
        where U : FeatureLight, IFeatureCluster<T>, new()
    {

        /// <summary>
        /// Reprocesses clusters and returns a list of new clusters.
        /// </summary>
        /// <param name="clusters"></param>
        /// <returns></returns>
        List<U> ProcessClusters(List<U> clusters);
    }
}
