using System;
using System.Collections.Generic;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    /// <summary>
    /// Handles tracking how many 
    /// </summary>
	[Obsolete("Code moved to MultiAlignWinOmics: MultiAlignCore.Algorithms.Alignment.SpectralMatching")]
    public sealed class GlobalClusterPeptideStatistics
    {
        public GlobalClusterPeptideStatistics()
        {
            Maps = new Dictionary<string, Dictionary<int, UMCClusterLight>>();
        }
        /// <summary>
        /// Gets or sets the number of peptides that went to the same cluster.
        /// </summary>
        public int SameCluster { get; set; }
        /// <summary>
        /// Gets or sets the number of peptides that went to multiple clusters.
        /// </summary>
        public int DifferentCluster { get; set; }

        public Dictionary<string, Dictionary<int, UMCClusterLight>> Maps { get; set; }
    }
}