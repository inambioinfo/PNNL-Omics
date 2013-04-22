﻿/* Written by Joseph N. Brown
 * for the Department of Energy (PNNL, Richland, WA)
 * Battelle Memorial Institute
 * E-mail: joseph.brown@pnnl.gov
 * Website: http://omics.pnl.gov/software
 * -----------------------------------------------------
 * 
 * Notice: This computer software was prepared by Battelle Memorial Institute,
 * hereinafter the Contractor, under Contract No. DE-AC05-76RL0 1830 with the
 * Department of Energy (DOE).  All rights in the computer software are reserved
 * by DOE on behalf of the United States Government and the Contractor as
 * provided in the Contract.
 * 
 * NEITHER THE GOVERNMENT NOR THE CONTRACTOR MAKES ANY WARRANTY, EXPRESS OR
 * IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.
 * 
 * This notice including this sentence must appear on any copies of this computer
 * software.
 * -----------------------------------------------------*/

using System;
using System.Collections.Generic;
using PNNLOmics.Data;

namespace PNNLOmics.Algorithms.Distance
{
    /// <summary>
    /// Class to calculate Pearson Product-Moment Correlation
    /// </summary>
    public class PearsonCorrelation
    {
        #region Properties
        /// <summary>
        /// Pearson correlation coefficient
        /// </summary>
        public double r { get; set; }

        /// <summary>
        /// Coefficient of Determination
        /// </summary>
        public double RSquared { get; set; }

        /// <summary>
        /// P-value of statistically significant correlation
        /// </summary>
        public double Pvalue { get; set; }

        /// <summary>
        /// Degrees of freedom for the study
        /// </summary>
        public int DegreesOfFreedom { get; set; }
        #endregion
        
        #region Constructors
        /// <summary>
        /// General Constructor
        /// </summary>
        public PearsonCorrelation()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Calculates the Pearson Product-Moment Correlation Coefficient (r)
        /// </summary>
        /// <param name="x">vector or array</param>
        /// <param name="y">vector or array</param>
        /// <returns>True, if the function completes successfully</returns>
        public bool Pearson(double[] x, double[] y)
        {
            if (!CheckVectors(x, y))
                return false;

            try
            {
                int n = x.Length;

                r = alglib.pearsoncorr2(x, y, n);
                RSquared = r * r;
                DegreesOfFreedom = x.Length - 2;

                Pvalue = 2 * (1 - alglib.studenttdistribution(DegreesOfFreedom,
                    (r * Math.Sqrt(DegreesOfFreedom) / Math.Sqrt(1 - r * r))));
                                
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        /// <summary>
        /// Calculates the Pearson Product-Moment Correlation Coefficient (r) on Y values from Lists of XYData
        /// </summary>
        /// <param name="x">vector or array</param>
        /// <param name="y">vector or array</param>
        /// <returns>True, if the function completes successfully</returns>
        public bool Pearson(List<XYData> data1, List<XYData> data2)
        {
            if (data1.Count != data2.Count)
                return false;

            try
            {
                int n = data1.Count;

                //convert XYData to arrays
                int xLength = data1.Count;
                double[] vectorY1 = new double[xLength];
                double[] vectorY2 = new double[xLength];

                for (int i = 0; i < xLength; i++)
                {
                    vectorY1[i] = data1[i].Y;
                    vectorY2[i] = data2[i].Y;
                }

                return Pearson(vectorY1, vectorY2);
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        /// <summary>
        /// Calculates the Pearson Product-Moment Correlation Coefficient (r)
        /// </summary>
        /// <param name="x">vector or array</param>
        /// <param name="y">vector or array</param>
        /// <returns>True, if the function completes successfully</returns>
        public bool Pearson(List<double> x, List<double> y)
        {
            if (x.Count != y.Count)
                return false;

            return Pearson(x.ToArray(), y.ToArray());
        }

        /// <summary>
        /// Calculates the Pearson Product-Moment Correlation Coefficient (r)
        /// </summary>
        /// <param name="x">vector or array</param>
        /// <param name="y">vector or array</param>
        /// <returns>True, if the function completes successfully</returns>
        public bool Pearson(float[] x, float[] y)
        {
            if (x.Length != y.Length)
                return false;

            double[] x1 = new double[x.Length];
            double[] y1 = new double[y.Length];
            for (int i = 0; i < x.Length; i++)
            {
                x1[i] = x[i];
                y1[i] = y[i];
            }

            return Pearson(x1, y1);
        }

        /// <summary>
        /// Calculates the Pearson Product-Moment Correlation Coefficient (r)
        /// </summary>
        /// <param name="x">vector or array</param>
        /// <param name="y">vector or array</param>
        /// <returns>True, if the function completes successfully</returns>
        public bool Pearson(List<float> x, List<float> y)
        {
            if (x.Count != y.Count)
                return false;

            return Pearson(x.ToArray(), y.ToArray());
        }

        /// <summary>
        /// Calculates the Pearson Product-Moment Correlation Coefficient (r)
        /// </summary>
        /// <param name="x">vector or array</param>
        /// <param name="y">vector or array</param>
        /// <returns>True, if the function completes successfully</returns>
        public bool Pearson(int[] x, int[] y)
        {
            if (x.Length != y.Length)
                return false;

            double[] x1 = new double[x.Length];
            double[] y1 = new double[y.Length];
            for (int i = 0; i < x.Length; i++)
            {
                x1[i] = x[i];
                y1[i] = y[i];
            }

            return Pearson(x1, y1);
        }

        /// <summary>
        /// Calculates the Pearson Product-Moment Correlation Coefficient (r)
        /// </summary>
        /// <param name="x">vector or array</param>
        /// <param name="y">vector or array</param>
        /// <returns>True, if the function completes successfully</returns>
        public bool Pearson(List<int> x, List<int> y)
        {
            if (x.Count != y.Count)
                return false;

            return Pearson(x.ToArray(), y.ToArray());
        }

        /// <summary>
        /// Checks that the two vectors are of equal length, and that there are
        /// more than 3 elements in the vector or array.
        /// </summary>
        /// <param name="x">vector or array</param>
        /// <param name="y">vector or array</param>
        /// <returns>True, if the parameters are correct for correlation</returns>
        public bool CheckVectors(double[] x, double[] y)
        {
            bool b_Successful = true;

            if (x.Length != y.Length)
                return false;

            if (x.Length < 3)
                return false;

            return b_Successful;
        }

        /// <summary>
        /// Generic method to write results to console, including r, R^2, and p-value
        /// </summary>
        public void WriteOutResultsToConsole()
        {
            Console.WriteLine(
                string.Format("Results from Pearson Product-Moment Correlation Analysis...\n" +
                "r = {0}\nR-squared = {1}\nP-Value = {2}",
                r, RSquared, Pvalue));
        }
        #endregion
    }        
}