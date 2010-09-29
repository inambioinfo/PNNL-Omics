﻿/*////////////////////////////////////////////////////////////////////////////////////////////////////////////
 * 
 * Name:    UMC Single Linkage Cluster Parameters Test 
 * File:    UMCSingleLinkageClusterParameterTest.cs
 * Author:  Brian LaMarche 
 * Purpose: Tests parameter method and properties.
 * Date:    9-22-2010
 * Revisions:
 ////////////////////////////////////////////////////////////////////////////////////////////////////////////*/
using System;
using System.Collections.Generic;

using NUnit.Framework;

using PNNLOmics.Data;
using PNNLOmics.Algorithms;
using PNNLOmics.Data.Features;
using PNNLOmics.Utilities.Importers;
using PNNLOmics.Algorithms.FeatureClustering;

namespace PNNLOmics.UnitTests.AlgorithmTests.FeatureClustering
{
    [TestFixture]
    public class UMCSingleLinkageClusterParameterTests
    {        
        /// <summary>
        ///  Part of a clustering test to make sure when sending a 
        ///  null list the clustering algorithm fails.
        /// </summary>
        [Test]        
        [Description("Sends a null list of UMC's to the clustering algorithm.")]
        public void ClearMethodTest()
        {
            FeatureClusterParameters parameters  = new FeatureClusterParameters();
            parameters.CentroidRepresentation               = ClusterCentroidRepresentation.Mean;
            parameters.DistanceFunction                     = null;
            bool useCharges                                 = parameters.OnlyClusterSameChargeStates;
            parameters.OnlyClusterSameChargeStates          = parameters.OnlyClusterSameChargeStates == false;
            parameters.Tolerances                           = null;
            parameters.Clear();

            Assert.AreEqual(parameters.CentroidRepresentation, ClusterCentroidRepresentation.Median);
            Assert.NotNull(parameters.Tolerances);
            Assert.AreEqual(useCharges, parameters.OnlyClusterSameChargeStates);
            Assert.NotNull(parameters.DistanceFunction);            
        }
    }
}