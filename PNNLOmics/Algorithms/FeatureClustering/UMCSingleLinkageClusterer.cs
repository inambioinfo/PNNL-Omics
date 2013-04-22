﻿/*////////////////////////////////////////////////////////////////////////////////////////////////////////////
 * 
 * Name:    UMC Single Linkage Clusterer 
 * File:    UMCSingleLinkageClustering.cs
 * Author:  Brian LaMarche 
 * Purpose: Perform clustering of UMC features across datasets into UMC Clusters.
 * Date:    5-19-2010
 * Revisions:
 *          5-19-2010 - BLL - Created clustering class and algorithm.
 ////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System;
using System.Collections.Generic;
using System.Linq;
using PNNLOmics.Data;
using PNNLOmics.Data.Features;

namespace PNNLOmics.Algorithms.FeatureClustering
{
    /// <summary>
    /// Clusters UMC's (LC-MS Features, LC-IMS-MS Features) into UMC Clusters.
    /// </summary>
    public class UMCSingleLinkageClusterer<T, U> : LinkageClustererBase<T, U>
        where T : FeatureLight, IChildFeature<U>,   new()
        where U : FeatureLight, IFeatureCluster<T>, new()
	{		
		/// <summary>
        /// Default Constructor.  This sets the parameters and tolerances to their default values.
        /// </summary>
        public UMCSingleLinkageClusterer()
        {
            Parameters		= new FeatureClusterParameters<T>();
			m_massComparer	= new Comparison<T>(FeatureLight.MassComparison);
        }

        #region Clustering Methods        
        /// <summary>
        /// Performs single linkage clustering over the data and returns a list of UMC clusters.
        /// </summary>
        /// <param name="data">Data to cluster on.</param>
        /// <param name="distances">pairwise distance between UMC's.</param>
        /// <returns>List of UMC clusters.</returns>        
        public override List<U> LinkFeatures(List<PairwiseDistance<T>> distances, Dictionary<int, U> clusters)
        {
            // We assume that the features have already been put into singleton
            // clusters or have a cluster already associated with them.  Otherwise
            // nothing will cluster.

            // Sort links based on distance            
			var newDistances = from element in distances
							   orderby element.Distance
							   select element;
            
            // Then do the linking           
			
            //foreach (PairwiseDistance<UMC> distance in distances)
			foreach (PairwiseDistance<T> distance in newDistances)
            {
                T featureX = distance.FeatureX;
                T featureY = distance.FeatureY;

                U clusterX = featureX.ParentFeature as U;
                U clusterY = featureY.ParentFeature as U;
                 
                // Determine if they are already clustered into the same cluster                                 
                if (clusterX == clusterY && clusterX != null)
                {
                    continue;
                }
                                                                       
                // Remove the references for all the clusters in the group 
                // and merge them into the other cluster.                    
                foreach (T umcX in clusterX.Features)
                {
                    umcX.SetParentFeature(clusterY);
                    clusterY.AddChildFeature(umcX);
                }

				// Remove the old cluster so we don't process it again.
				clusters.Remove(clusterX.ID);					                
            }

            //return clusters;
			U [] array			= new U[clusters.Values.Count];
			clusters.Values.CopyTo(array, 0);
			List<U> newClusters = new List<U>();
			newClusters.AddRange(array);

			return newClusters;
        }
        #endregion
		
        /// <summary>
        /// Reports the name of this clustering algorithm.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "single linkage clustering";
        }
    }
}
