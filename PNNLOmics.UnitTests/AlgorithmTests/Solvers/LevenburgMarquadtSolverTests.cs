﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MS.Internal.Xml.XPath;
using NUnit.Framework;
using PNNLOmics.Algorithms.Solvers;
using PNNLOmics.Algorithms.Solvers.LevenburgMarquadt;
using PNNLOmics.Algorithms.Solvers.LevenburgMarquadt.BasisFunctions;
using PNNLOmics.Data;
using System.Reflection;

namespace PNNLOmics.UnitTests.AlgorithmTests.Solvers
{

    [TestFixture]
    public class LevenburgMarquadtSolverTests
    {
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a quadratic line shape.")]
        public void SolveQuadraticFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(CalculatedParabola(),out x, out y);

            BasisFunctionsEnum functionChoise = BasisFunctionsEnum.PolynomialQuadratic;

            BasisFunctionFactory functionSelector = BasisFunctionFactory.BasisFunctionSelector(functionChoise);
            double[] coeffs = functionSelector.Coefficients;
            alglib.ndimensional_pfunc myDelegate = functionSelector.GetFunction;

            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            solver.BasisFunction = myDelegate;
            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                //quadSolver2.FunctionDelegate(coeffs, new double[] { xValue }, ref fitValue, null);
                myDelegate.Invoke(coeffs, new double[] { xValue }, ref fitValue, null);
                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            Assert.AreEqual(-0.99999999960388553d, coeffs[0]);
            Assert.AreEqual(2.410211171560969E-10d, coeffs[1]);
            Assert.AreEqual(99.999999976322613d, coeffs[2]);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a Hanning line shape.")]
        public void SolveHanningFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(ManualHanning(), out x, out y);

            BasisFunctionsEnum functionChoise = BasisFunctionsEnum.Hanning;

            BasisFunctionFactory functionSelector = BasisFunctionFactory.BasisFunctionSelector(functionChoise);
            double[] coeffs = functionSelector.Coefficients;
            alglib.ndimensional_pfunc myDelegate = functionSelector.GetFunction;

            //important guesses
            coeffs[0] = 30;//hanningI
            coeffs[1] = 5;//hanningK
            coeffs[2] = 1234.388251;//xoffset
            
            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            solver.BasisFunction = myDelegate;
            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                //quadSolver2.FunctionDelegate(coeffs, new double[] { xValue }, ref fitValue, null);
                myDelegate.Invoke(coeffs, new double[] { xValue }, ref fitValue, null);
                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            Assert.AreEqual(Math.Round(30.521054724721569d, 7), Math.Round(coeffs[0], 7));
            Assert.AreEqual(Math.Round(37.723968728457208d,6), Math.Round(coeffs[1],6));
            Assert.AreEqual(Math.Round(1234.4579999999935d,7), Math.Round(coeffs[2],7));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a quadratic line shape.")]
        public void SolveOrbitrapLorentzianFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(ManualOrbitrap2(), out x, out y);

            BasisFunctionsEnum functionChoise = BasisFunctionsEnum.Lorentzian;

            BasisFunctionFactory functionSelector = BasisFunctionFactory.BasisFunctionSelector(functionChoise);
            double[] coeffs = functionSelector.Coefficients;
            alglib.ndimensional_pfunc myDelegate = functionSelector.GetFunction;

            //important guesses
            coeffs[0] = 5;//hanningI
            coeffs[1] = 80000;//hanningK
            coeffs[2] = 1234.388251;//xoffset

            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            solver.BasisFunction = myDelegate;
            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                //quadSolver2.FunctionDelegate(coeffs, new double[] { xValue }, ref fitValue, null);
                myDelegate.Invoke(coeffs, new double[] { xValue }, ref fitValue, null);
                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            Assert.AreEqual(0.014591732782157337d, coeffs[0]);
            Assert.AreEqual(41816.913857810927d, coeffs[1]);
            Assert.AreEqual(1234.4577771195013d, coeffs[2]);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a cubic line shape.")]
        public void SolveCubicFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(CalculatedCubic(), out x, out y);

            BasisFunctionsEnum functionChoise = BasisFunctionsEnum.PolynomialCubic;

            BasisFunctionFactory functionSelector = BasisFunctionFactory.BasisFunctionSelector(functionChoise);
            double[] coeffs = functionSelector.Coefficients;
            alglib.ndimensional_pfunc myDelegate = functionSelector.GetFunction;

            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            solver.BasisFunction = myDelegate;
            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                //quadSolver2.FunctionDelegate(coeffs, new double[] { xValue }, ref fitValue, null);
                myDelegate.Invoke(coeffs, new double[] { xValue }, ref fitValue, null);
                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            Assert.AreEqual(-0.9999999999984106d, coeffs[0]);
            Assert.AreEqual(5.0000000000444658d, coeffs[1]);
            Assert.AreEqual(99.999999999930722d, coeffs[2]);
            Assert.AreEqual(24.999999997435527d, coeffs[3]);
        }

        /// <summary>
        /// 
        /// </summary>
        //[Test]
        [Description("Tests the Levenburg Marquadt solver using Chebyshev polynomials.")]
        public void SolveCubicWithChebyshevFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(CalculatedParabola(), out x, out y);

            BasisFunctionsEnum functionChoise = BasisFunctionsEnum.Chebyshev;

            BasisFunctionFactory functionSelector = BasisFunctionFactory.BasisFunctionSelector(functionChoise);
            double[] coeffs = functionSelector.Coefficients;
            alglib.ndimensional_pfunc myDelegate = functionSelector.GetFunction;

            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            solver.BasisFunction = myDelegate;
            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                //quadSolver2.FunctionDelegate(coeffs, new double[] { xValue }, ref fitValue, null);
                myDelegate.Invoke(coeffs, new double[] { xValue }, ref fitValue, null);
                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            //Assert.AreEqual(-0.9999999999984106d, coeffs[0]);
            //Assert.AreEqual(5.0000000000444658d, coeffs[1]);
            //Assert.AreEqual(99.999999999930722d, coeffs[2]);
            //Assert.AreEqual(24.999999997435527d, coeffs[3]);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a lorentzian line shape.")]
        public void SolveLorentzianFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(ManualLortentzianA(), out x, out y);

            BasisFunctionsEnum functionChoise = BasisFunctionsEnum.Lorentzian;

            BasisFunctionFactory functionSelector = BasisFunctionFactory.BasisFunctionSelector(functionChoise);
            double[] coeffs = functionSelector.Coefficients;
            alglib.ndimensional_pfunc myDelegate = functionSelector.GetFunction;

            coeffs[0] = 6;//width
            coeffs[1] = 50;//height
            coeffs[2] = -1;//xoffset

            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            solver.BasisFunction = myDelegate;
            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                //quadSolver2.FunctionDelegate(coeffs, new double[] { xValue }, ref fitValue, null);
                myDelegate.Invoke(coeffs, new double[] { xValue }, ref fitValue, null);
                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            Assert.AreEqual(0.50000000000535016d, coeffs[0]);//real is 0.5. 
            Assert.AreEqual(150.00000000174555d, coeffs[1]);//real is 75
            Assert.AreEqual(0.99999999999999312d, coeffs[2]);//real is 1

            //using 1 instead of 0.5
            //Assert.AreEqual(0.49999999817701907d, coeffs[0]);//real is 0.5. 
            //Assert.AreEqual(74.99999972887592d, coeffs[1]);//real is 75
            //Assert.AreEqual(0.9999999999999587d, coeffs[2]);//real is 1
        }


        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a gaussian line shape.")]
        public void SolveGaussianFactory()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(ManualGaussian(), out x, out y);

            BasisFunctionsEnum functionChoise = BasisFunctionsEnum.Gaussian;

            BasisFunctionFactory functionSelector = BasisFunctionFactory.BasisFunctionSelector(functionChoise);
            double[] coeffs = functionSelector.Coefficients;

            coeffs[0] = 6;//sigma
            coeffs[1] = 50;//height
            coeffs[2] = -1;//xoffset

            alglib.ndimensional_pfunc myDelegate = functionSelector.GetFunction;

            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            solver.BasisFunction = myDelegate;
            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                //quadSolver2.FunctionDelegate(coeffs, new double[] { xValue }, ref fitValue, null);
                myDelegate.Invoke(coeffs, new double[] { xValue }, ref fitValue, null);
                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            Assert.AreEqual(0.50000000014842283d, Math.Abs(coeffs[0]));//real is 0.5.  may return a negative value
            Assert.AreEqual(99.999999955476071d, coeffs[1]);//real is 100
            Assert.AreEqual(0.99999999999999967d, coeffs[2]);//real is 1
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a quadratic line shape (legacy)")]
        public void SolveQuadratic()
        {
            double[] coeffs = new double[3];

            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(CalculatedParabola(), out x, out y);


            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            QuadraticSolver quadSolver = new QuadraticSolver();

            alglib.ndimensional_pfunc myDelegate = quadSolver.QuadraticSolve;
            solver.BasisFunction = myDelegate;
            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                quadSolver.QuadraticSolve(coeffs, new double[] { xValue }, ref fitValue, null);

                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            Assert.AreEqual(-0.99999999959999375d, coeffs[0]);
            Assert.AreEqual(2.4338897338076459E-10d, coeffs[1]);
            Assert.AreEqual(99.999999976089995d, coeffs[2]);

        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a gaussian line shape (legacy)")]
        public void SolveGaussian()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(ManualGaussian(), out x, out y);

            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            GaussianSolver gaussianSolver = new GaussianSolver();

            alglib.ndimensional_pfunc myDelegate = gaussianSolver.GaussianSolve;
            solver.BasisFunction = myDelegate;

            double[] coeffs = new double[3];

            //guess
            coeffs[0] = 6;//sigma
            coeffs[1] = 50;//height
            coeffs[2] = -1;//xoffset

            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                gaussianSolver.GaussianSolve(coeffs, new double[] { xValue }, ref fitValue, null);

                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            //sigma must be positive

            Assert.AreEqual(0.50000000014842283d, Math.Abs(coeffs[0]));//real is 0.5.  may return a negative value
            Assert.AreEqual(99.999999955476071d, coeffs[1]);//real is 100
            Assert.AreEqual(0.99999999999999967d, coeffs[2]);//real is 1

        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Description("Tests the Levenburg Marquadt solver using a lorentzian line shape (legacy)")]
        public void SolveLorentzian()
        {
            List<double> x;
            List<double> y;
            ConvertXYDataToArrays(ManualLortentzianA(), out x, out y);

            LevenburgMarquadtSolver solver = new LevenburgMarquadtSolver();
            LorentzianSolver lorentzianSolver = new LorentzianSolver();

            alglib.ndimensional_pfunc myDelegate = lorentzianSolver.LorentziannSolve;
            solver.BasisFunction = myDelegate;

            double[] coeffs = new double[3];
            //guess
            coeffs[0] = 6;//width
            coeffs[1] = 50;//height
            coeffs[2] = -1;//xoffset

            bool worked = solver.Solve(x, y, ref coeffs);

            Assert.IsTrue(worked);
            for (int i = 0; i < x.Count; i++)
            {

                // This is what we are fitting 
                double xValue = x[i];

                // This is what it should fit to
                double yValue = y[i];

                // This is the warped guy
                double fitValue = 0;
                lorentzianSolver.LorentziannSolve(coeffs, new double[] { xValue }, ref fitValue, null);

                Console.WriteLine("{0}\t{1}\t{2}", xValue, fitValue, yValue);
            }

            //sigma must be positive

            Assert.AreEqual(0.49999999817701907d, coeffs[0]);//real is 0.5. 
            Assert.AreEqual(74.99999972887592d, coeffs[1]);//real is 75
            Assert.AreEqual(0.9999999999999587d, coeffs[2]);//real is 1

        }


        private static void ConvertXYDataToArrays(List<PNNLOmics.Data.XYData> data, out List<double> x, out List<double> y)
        {
            x = new List<double>();
            y = new List<double>();
            foreach (var xyData in data)
            {
                x.Add(xyData.X);
                y.Add(xyData.Y);
            }
        }
 
        private static List<PNNLOmics.Data.XYData> ManualLortentzianB()
        {
            List<PNNLOmics.Data.XYData> manualData = new List<XYData>();
            manualData.Add(new XYData(-0.0219999999999345, 9.3977766913991E-16));
            manualData.Add(new XYData(-0.02150000000006, 2.39880902501257E-14));
            manualData.Add(new XYData(-0.0209999999999582, 5.36461868040639E-13));
            manualData.Add(new XYData(-0.0205000000000837, 1.05112574057507E-11));
            manualData.Add(new XYData(-0.0199999999999818, 1.80444278425828E-10));
            manualData.Add(new XYData(-0.0195000000001073, 2.713964676405E-09));
            manualData.Add(new XYData(-0.0190000000000055, 3.57633255625202E-08));
            manualData.Add(new XYData(-0.018500000000131, 4.12899381286205E-07));
            manualData.Add(new XYData(-0.0180000000000291, 4.17660302304908E-06));
            manualData.Add(new XYData(-0.0174999999999272, 3.70147478602181E-05));
            manualData.Add(new XYData(-0.0170000000000528, 0.000287408112628826));
            manualData.Add(new XYData(-0.0164999999999509, 0.00195522112375824));
            manualData.Add(new XYData(-0.0160000000000764, 0.0116537411913572));
            manualData.Add(new XYData(-0.0154999999999745, 0.0608565723566319));
            manualData.Add(new XYData(-0.0150000000001, 0.278433981159247));
            manualData.Add(new XYData(-0.0144999999999982, 1.11611672731448));
            manualData.Add(new XYData(-0.0140000000001237, 3.91985161576496));
            manualData.Add(new XYData(-0.0135000000000218, 12.0615262993972));
            manualData.Add(new XYData(-0.01299999999992, 32.5167805045449));
            manualData.Add(new XYData(-0.0125000000000455, 76.8042833615195));
            manualData.Add(new XYData(-0.0119999999999436, 158.94101186222));
            manualData.Add(new XYData(-0.0115000000000691, 288.176880819004));
            manualData.Add(new XYData(-0.0109999999999673, 457.77801283273));
            manualData.Add(new XYData(-0.0105000000000928, 637.123102780412));
            manualData.Add(new XYData(-0.00999999999999091, 776.898723053241));
            manualData.Add(new XYData(-0.00950000000011642, 830));
            manualData.Add(new XYData(-0.00900000000001455, 776.898723053147));
            manualData.Add(new XYData(-0.00849999999991269, 637.123102780258));
            manualData.Add(new XYData(-0.0080000000000382, 457.778012832564));
            manualData.Add(new XYData(-0.00749999999993634, 288.176880818865));
            manualData.Add(new XYData(-0.00700000000006185, 158.941011862124));
            manualData.Add(new XYData(-0.00649999999995998, 76.8042833614638));
            manualData.Add(new XYData(-0.00600000000008549, 32.5167805045173));
            manualData.Add(new XYData(-0.00549999999998363, 12.0615262993856));
            manualData.Add(new XYData(-0.00500000000010914, 3.9198516157607));
            manualData.Add(new XYData(-0.00450000000000728, 1.11611672731313));
            manualData.Add(new XYData(-0.00400000000013279, 0.278433981158876));
            manualData.Add(new XYData(-0.00350000000003092, 0.0608565723565436));
            manualData.Add(new XYData(-0.00299999999992906, 0.0116537411913389));
            manualData.Add(new XYData(-0.00250000000005457, 0.00195522112375493));
            manualData.Add(new XYData(-0.00199999999995271, 0.000287408112628304));
            manualData.Add(new XYData(-0.00150000000007822, 3.70147478601467E-05));
            manualData.Add(new XYData(-0.000999999999976353, 4.17660302304052E-06));
            manualData.Add(new XYData(-0.000500000000101863, 4.12899381285309E-07));
            manualData.Add(new XYData(0, 3.57633255624383E-08));
            manualData.Add(new XYData(0.00049999999987449, 2.71396467639845E-09));
            manualData.Add(new XYData(0.000999999999976353, 1.80444278425371E-10));
            manualData.Add(new XYData(0.00150000000007822, 1.05112574057228E-11));
            manualData.Add(new XYData(0.00199999999995271, 5.36461868039152E-13));
            manualData.Add(new XYData(0.00250000000005457, 2.39880902500561E-14));
            manualData.Add(new XYData(0.00299999999992906, 9.39777669137079E-16));

            return manualData;
        }

        private static List<PNNLOmics.Data.XYData> ManualLortentzianA()
        {
            List<PNNLOmics.Data.XYData> manualData = new List<XYData>();
            manualData.Add(new XYData(-1.5, 1.87241109519877));
            manualData.Add(new XYData(-1.4, 2.02831278366901));
            manualData.Add(new XYData(-1.3, 2.20436209268553));
            manualData.Add(new XYData(-1.2, 2.40415321891081));
            manualData.Add(new XYData(-1.1, 2.63211041497071));
            manualData.Add(new XYData(-1, 2.89372623803446));
            manualData.Add(new XYData(-0.9, 3.19588239140352));
            manualData.Add(new XYData(-0.8, 3.54728699313288));
            manualData.Add(new XYData(-0.7, 3.95907818636556));
            manualData.Add(new XYData(-0.6, 4.44566880144959));
            manualData.Add(new XYData(-0.5, 5.02594557132301));
            manualData.Add(new XYData(-0.4, 5.72499795294588));
            manualData.Add(new XYData(-0.3, 6.57665054098741));
            manualData.Add(new XYData(-0.2, 7.62723369449978));
            manualData.Add(new XYData(-0.0999999999999998, 8.94128893774693));
            manualData.Add(new XYData(1.94289029309402E-16, 10.6103295394597));
            manualData.Add(new XYData(0.1, 12.7664392854462));
            manualData.Add(new XYData(0.2, 15.6034257933231));
            manualData.Add(new XYData(0.3, 19.4091394014507));
            manualData.Add(new XYData(0.4, 24.611589137922));
            manualData.Add(new XYData(0.5, 31.8309886183791));
            manualData.Add(new XYData(0.6, 41.8828797610251));
            manualData.Add(new XYData(0.7, 55.5191661948472));
            manualData.Add(new XYData(0.8, 72.3431559508615));
            manualData.Add(new XYData(0.9, 88.4194128288308));
            manualData.Add(new XYData(1, 95.4929658551372));
            manualData.Add(new XYData(1.1, 88.4194128288307));
            manualData.Add(new XYData(1.2, 72.3431559508614));
            manualData.Add(new XYData(1.3, 55.5191661948471));
            manualData.Add(new XYData(1.4, 41.882879761025));
            manualData.Add(new XYData(1.5, 31.830988618379));
            manualData.Add(new XYData(1.6, 24.6115891379219));
            manualData.Add(new XYData(1.7, 19.4091394014506));
            manualData.Add(new XYData(1.8, 15.603425793323));
            manualData.Add(new XYData(1.9, 12.7664392854461));
            manualData.Add(new XYData(2, 10.6103295394597));
            manualData.Add(new XYData(2.1, 8.94128893774691));
            manualData.Add(new XYData(2.2, 7.62723369449976));
            manualData.Add(new XYData(2.3, 6.5766505409874));
            manualData.Add(new XYData(2.4, 5.72499795294587));
            manualData.Add(new XYData(2.5, 5.025945571323));
            manualData.Add(new XYData(2.6, 4.44566880144958));
            manualData.Add(new XYData(2.7, 3.95907818636555));
            manualData.Add(new XYData(2.8, 3.54728699313288));
            manualData.Add(new XYData(2.9, 3.19588239140352));
            manualData.Add(new XYData(3, 2.89372623803446));
            manualData.Add(new XYData(3.1, 2.6321104149707));
            manualData.Add(new XYData(3.2, 2.4041532189108));
            manualData.Add(new XYData(3.3, 2.20436209268553));
            manualData.Add(new XYData(3.4, 2.02831278366901));
            manualData.Add(new XYData(3.5, 1.87241109519877));


            return manualData;
        }

        private static List<PNNLOmics.Data.XYData> ManualGaussian()
        {
            List<PNNLOmics.Data.XYData> manualData = new List<XYData>();
            manualData.Add(new XYData(-1.5, 0.000372665317207867));
            manualData.Add(new XYData(-1.4, 0.000992950430585108));
            manualData.Add(new XYData(-1.3, 0.00254193465161993));
            manualData.Add(new XYData(-1.2, 0.00625215037748204));
            manualData.Add(new XYData(-1.1, 0.0147748360232034));
            manualData.Add(new XYData(-1, 0.0335462627902513));
            manualData.Add(new XYData(-0.9, 0.0731802418880474));
            manualData.Add(new XYData(-0.8, 0.153381067932447));
            manualData.Add(new XYData(-0.7, 0.308871540823677));
            manualData.Add(new XYData(-0.6, 0.597602289500596));
            manualData.Add(new XYData(-0.5, 1.11089965382423));
            manualData.Add(new XYData(-0.4, 1.98410947443703));
            manualData.Add(new XYData(-0.3, 3.40474547345994));
            manualData.Add(new XYData(-0.2, 5.61347628341338));
            manualData.Add(new XYData(-0.0999999999999998, 8.89216174593864));
            manualData.Add(new XYData(1.94289029309402E-16, 13.5335283236613));
            manualData.Add(new XYData(0.1, 19.7898699083615));
            manualData.Add(new XYData(0.2, 27.8037300453194));
            manualData.Add(new XYData(0.3, 37.53110988514));
            manualData.Add(new XYData(0.4, 48.6752255959972));
            manualData.Add(new XYData(0.5, 60.6530659712634));
            manualData.Add(new XYData(0.6, 72.6149037073691));
            manualData.Add(new XYData(0.7, 83.5270211411272));
            manualData.Add(new XYData(0.8, 92.3116346386636));
            manualData.Add(new XYData(0.9, 98.0198673306755));
            manualData.Add(new XYData(1, 100));
            manualData.Add(new XYData(1.1, 98.0198673306755));
            manualData.Add(new XYData(1.2, 92.3116346386635));
            manualData.Add(new XYData(1.3, 83.5270211411272));
            manualData.Add(new XYData(1.4, 72.614903707369));
            manualData.Add(new XYData(1.5, 60.6530659712633));
            manualData.Add(new XYData(1.6, 48.6752255959971));
            manualData.Add(new XYData(1.7, 37.5311098851399));
            manualData.Add(new XYData(1.8, 27.8037300453193));
            manualData.Add(new XYData(1.9, 19.7898699083614));
            manualData.Add(new XYData(2, 13.5335283236612));
            manualData.Add(new XYData(2.1, 8.89216174593859));
            manualData.Add(new XYData(2.2, 5.61347628341334));
            manualData.Add(new XYData(2.3, 3.40474547345991));
            manualData.Add(new XYData(2.4, 1.98410947443701));
            manualData.Add(new XYData(2.5, 1.11089965382422));
            manualData.Add(new XYData(2.6, 0.597602289500589));
            manualData.Add(new XYData(2.7, 0.308871540823674));
            manualData.Add(new XYData(2.8, 0.153381067932445));
            manualData.Add(new XYData(2.9, 0.0731802418880463));
            manualData.Add(new XYData(3, 0.0335462627902507));
            manualData.Add(new XYData(3.1, 0.0147748360232031));
            manualData.Add(new XYData(3.2, 0.00625215037748192));
            manualData.Add(new XYData(3.3, 0.00254193465161987));
            manualData.Add(new XYData(3.4, 0.000992950430585087));
            manualData.Add(new XYData(3.5, 0.000372665317207859));


            return manualData;
        }

        private static List<PNNLOmics.Data.XYData> CalculatedParabola()
        {
            List<PNNLOmics.Data.XYData> calculatedData = new List<XYData>();

            List<double> x = new List<double>();
            List<double> y = new List<double>();
            for (int i = -10; i < 10; i++)
            {
                double val = Convert.ToDouble(i);
                double xValue = val;
                double yValue = -(val * val) + 100;

                x.Add(xValue);
                y.Add(yValue);

                calculatedData.Add(new XYData(xValue,yValue));
            }
            return calculatedData;
        }

        private static List<PNNLOmics.Data.XYData> CalculatedCubic()
        {
            List<PNNLOmics.Data.XYData> calculatedData = new List<XYData>();

            List<double> x = new List<double>();
            List<double> y = new List<double>();
            for (int i = -10; i < 10; i++)
            {
                double val = Convert.ToDouble(i);
                double xValue = val;
                double yValue = -(val * val * val) + (5*val*val) + (100*val) + 25;

                x.Add(xValue);
                y.Add(yValue);

                calculatedData.Add(new XYData(xValue, yValue));
            }
            return calculatedData;
        }

        private static List<PNNLOmics.Data.XYData> ManualOrbitrap2()
        {
            List<PNNLOmics.Data.XYData> manualData = new List<XYData>();
            manualData.Add(new XYData(1234.388251, 1891.439726));
            manualData.Add(new XYData(1234.395524, 3418.562352));
            manualData.Add(new XYData(1234.402796, 1990.857499));
            manualData.Add(new XYData(1234.410069, 2540.506075));
            manualData.Add(new XYData(1234.417341, 1205.39776));
            manualData.Add(new XYData(1234.424614, 5217.050472));
            manualData.Add(new XYData(1234.431886, 38439.607787));
            manualData.Add(new XYData(1234.439159, 205425.605138));
            manualData.Add(new XYData(1234.446432, 507161.037318));
            manualData.Add(new XYData(1234.453705, 773208.328002));
            manualData.Add(new XYData(1234.460978, 794922.929123));
            manualData.Add(new XYData(1234.468251, 551918.127801));
            manualData.Add(new XYData(1234.475524, 237305.814052));
            manualData.Add(new XYData(1234.482797, 46593.691567));
            manualData.Add(new XYData(1234.49007, 2962.378226));
            manualData.Add(new XYData(1234.497343, 1004.645773));
            manualData.Add(new XYData(1234.504616, 1864.893943));
            manualData.Add(new XYData(1234.51189, 3653.207678));
            manualData.Add(new XYData(1234.519163, 2375.71473));
            manualData.Add(new XYData(1234.526437, 3852.932286));
            manualData.Add(new XYData(1234.53371, 2401.292172));
            manualData.Add(new XYData(1234.540984, 3638.65161));
            manualData.Add(new XYData(1234.548258, 2128.947417));

            //manualData.Add(new XYData(123438.8251, 1891.439726));
            //manualData.Add(new XYData(123439.5524, 3418.562352));
            //manualData.Add(new XYData(123440.2796, 1990.857499));
            //manualData.Add(new XYData(123441.0069, 2540.506075));
            //manualData.Add(new XYData(123441.7341, 1205.39776));
            //manualData.Add(new XYData(123442.4614, 5217.050472));
            //manualData.Add(new XYData(123443.1886, 38439.607787));
            //manualData.Add(new XYData(123443.9159, 205425.605138));
            //manualData.Add(new XYData(123444.6432, 507161.037318));
            //manualData.Add(new XYData(123445.3705, 773208.328002));
            //manualData.Add(new XYData(123446.0978, 794922.929123));
            //manualData.Add(new XYData(123446.8251, 551918.127801));
            //manualData.Add(new XYData(123447.5524, 237305.814052));
            //manualData.Add(new XYData(123448.2797, 46593.691567));
            //manualData.Add(new XYData(123449.007, 2962.378226));
            //manualData.Add(new XYData(123449.7343, 1004.645773));
            //manualData.Add(new XYData(123450.4616, 1864.893943));
            //manualData.Add(new XYData(123451.189, 3653.207678));
            //manualData.Add(new XYData(123451.9163, 2375.71473));
            //manualData.Add(new XYData(123452.6437, 3852.932286));
            //manualData.Add(new XYData(123453.371, 2401.292172));
            //manualData.Add(new XYData(123454.0984, 3638.65161));
            //manualData.Add(new XYData(123454.8258, 2128.947417));

            return manualData;
        }

        private static List<PNNLOmics.Data.XYData> ManualHanning()
        {
            List<PNNLOmics.Data.XYData> manualData = new List<XYData>();
            manualData.Add(new XYData(1234.358, 0.0227976600210893));
            manualData.Add(new XYData(1234.3581, 0.0229373596602475));
            manualData.Add(new XYData(1234.3582, 0.0230648449239964));
            manualData.Add(new XYData(1234.3583, 0.0231799691996666));
            manualData.Add(new XYData(1234.3584, 0.0232825918642144));
            manualData.Add(new XYData(1234.3585, 0.0233725783982258));
            manualData.Add(new XYData(1234.3586, 0.0234498004972908));
            manualData.Add(new XYData(1234.3587, 0.0235141361806717));
            manualData.Add(new XYData(1234.3588, 0.0235654698971914));
            manualData.Add(new XYData(1234.3589, 0.0236036926283369));
            manualData.Add(new XYData(1234.359, 0.02362870198805));
            manualData.Add(new XYData(1234.3591, 0.0236404023203566));
            manualData.Add(new XYData(1234.3592, 0.0236387047930276));
            manualData.Add(new XYData(1234.3593, 0.0236235274885868));
            manualData.Add(new XYData(1234.3594, 0.0235947954920044));
            manualData.Add(new XYData(1234.3595, 0.023552440975127));
            manualData.Add(new XYData(1234.3596, 0.0234964032777654));
            manualData.Add(new XYData(1234.3597, 0.023426628985383));
            manualData.Add(new XYData(1234.3598, 0.0233430720031194));
            manualData.Add(new XYData(1234.3599, 0.0232456936272823));
            manualData.Add(new XYData(1234.36, 0.0231344626113314));
            manualData.Add(new XYData(1234.3601, 0.0230093552300845));
            manualData.Add(new XYData(1234.3602, 0.0228703553393281));
            manualData.Add(new XYData(1234.3603, 0.02271745443185));
            manualData.Add(new XYData(1234.3604, 0.022550651689668));
            manualData.Add(new XYData(1234.3605, 0.0223699540324016));
            manualData.Add(new XYData(1234.3606, 0.0221753761617425));
            manualData.Add(new XYData(1234.3607, 0.021966940601479));
            manualData.Add(new XYData(1234.3608, 0.0217446777359418));
            manualData.Add(new XYData(1234.3609, 0.0215086258396373));
            manualData.Add(new XYData(1234.361, 0.0212588311074997));
            manualData.Add(new XYData(1234.3611, 0.0209953476787019));
            manualData.Add(new XYData(1234.3612, 0.0207182376567568));
            manualData.Add(new XYData(1234.3613, 0.0204275711254225));
            manualData.Add(new XYData(1234.3614, 0.0201234261603714));
            manualData.Add(new XYData(1234.3615, 0.0198058888365943));
            manualData.Add(new XYData(1234.3616, 0.0194750532307417));
            manualData.Add(new XYData(1234.3617, 0.0191310214229458));
            manualData.Add(new XYData(1234.3618, 0.0187739034867391));
            manualData.Add(new XYData(1234.3619, 0.0184038174820934));
            manualData.Add(new XYData(1234.362, 0.0180208894403438));
            manualData.Add(new XYData(1234.3621, 0.0176252533454365));
            manualData.Add(new XYData(1234.3622, 0.0172170511107415));
            manualData.Add(new XYData(1234.3623, 0.0167964325514198));
            manualData.Add(new XYData(1234.3624, 0.0163635553523282));
            manualData.Add(new XYData(1234.3625, 0.0159185850304266));
            manualData.Add(new XYData(1234.3626, 0.0154616948978084));
            manualData.Add(new XYData(1234.3627, 0.0149930660100963));
            manualData.Add(new XYData(1234.3628, 0.0145128871204422));
            manualData.Add(new XYData(1234.3629, 0.0140213546240184));
            manualData.Add(new XYData(1234.363, 0.0135186724990165));
            manualData.Add(new XYData(1234.3631, 0.0130050522431615));
            manualData.Add(new XYData(1234.3632, 0.0124807128057457));
            manualData.Add(new XYData(1234.3633, 0.0119458805151907));
            manualData.Add(new XYData(1234.3634, 0.0114007890008978));
            manualData.Add(new XYData(1234.3635, 0.0108456791168818));
            manualData.Add(new XYData(1234.3636, 0.0102807988485557));
            manualData.Add(new XYData(1234.3637, 0.00970640322747091));
            manualData.Add(new XYData(1234.3638, 0.00912275423550275));
            manualData.Add(new XYData(1234.3639, 0.00853012070589186));
            manualData.Add(new XYData(1234.364, 0.00792877821993895));
            manualData.Add(new XYData(1234.3641, 0.00731900899938318));
            manualData.Add(new XYData(1234.3642, 0.00670110179449088));
            manualData.Add(new XYData(1234.3643, 0.00607535176645865));
            manualData.Add(new XYData(1234.3644, 0.00544206037273611));
            manualData.Add(new XYData(1234.3645, 0.00480153523391968));
            manualData.Add(new XYData(1234.3646, 0.00415409001071467));
            manualData.Add(new XYData(1234.3647, 0.0035000442697097));
            manualData.Add(new XYData(1234.3648, 0.00283972334650891));
            manualData.Add(new XYData(1234.3649, 0.00217345820485615));
            manualData.Add(new XYData(1234.365, 0.00150158529180048));
            manualData.Add(new XYData(1234.3651, 0.00082444638895433));
            manualData.Add(new XYData(1234.3652, 0.000142388458343905));
            manualData.Add(new XYData(1234.3653, -0.000544236507765808));
            manualData.Add(new XYData(1234.3654, -0.00123507165630272));
            manualData.Add(new XYData(1234.3655, -0.00192975543099529));
            manualData.Add(new XYData(1234.3656, -0.00262792174133056));
            manualData.Add(new XYData(1234.3657, -0.00332920013351097));
            manualData.Add(new XYData(1234.3658, -0.00403321596488855));
            manualData.Add(new XYData(1234.3659, -0.00473959058180544));
            manualData.Add(new XYData(1234.366, -0.00544794150076742));
            manualData.Add(new XYData(1234.3661, -0.0061578825944918));
            manualData.Add(new XYData(1234.3662, -0.00686902427306024));
            manualData.Add(new XYData(1234.3663, -0.00758097368431987));
            manualData.Add(new XYData(1234.3664, -0.00829333490116474));
            manualData.Add(new XYData(1234.3665, -0.0090057091198196));
            manualData.Add(new XYData(1234.3666, -0.00971769485936285));
            manualData.Add(new XYData(1234.3667, -0.0104288881640119));
            manualData.Add(new XYData(1234.3668, -0.0111388828080829));
            manualData.Add(new XYData(1234.3669, -0.011847270503531));
            manualData.Add(new XYData(1234.367, -0.012553641111583));
            manualData.Add(new XYData(1234.3671, -0.0132575828487383));
            manualData.Add(new XYData(1234.3672, -0.013958682511124));
            manualData.Add(new XYData(1234.3673, -0.014656525684976));
            manualData.Add(new XYData(1234.3674, -0.015350696967255));
            manualData.Add(new XYData(1234.3675, -0.0160407801866451));
            manualData.Add(new XYData(1234.3676, -0.0167263586264406));
            manualData.Add(new XYData(1234.3677, -0.0174070152492131));
            manualData.Add(new XYData(1234.3678, -0.0180823329231528));
            manualData.Add(new XYData(1234.3679, -0.0187518946514898));
            manualData.Add(new XYData(1234.368, -0.019415283795777));
            manualData.Add(new XYData(1234.3681, -0.0200720843157252));
            manualData.Add(new XYData(1234.3682, -0.0207218809950267));
            manualData.Add(new XYData(1234.3683, -0.0213642596759248));
            manualData.Add(new XYData(1234.3684, -0.0219988074932488));
            manualData.Add(new XYData(1234.3685, -0.0226251131093278));
            manualData.Add(new XYData(1234.3686, -0.0232427669496635));
            manualData.Add(new XYData(1234.3687, -0.0238513614392416));
            manualData.Add(new XYData(1234.3688, -0.0244504912407124));
            manualData.Add(new XYData(1234.3689, -0.0250397534861888));
            manualData.Add(new XYData(1234.369, -0.025618748022918));
            manualData.Add(new XYData(1234.3691, -0.0261870776454554));
            manualData.Add(new XYData(1234.3692, -0.0267443483347052));
            manualData.Add(new XYData(1234.3693, -0.0272901694954874));
            manualData.Add(new XYData(1234.3694, -0.0278241541938703));
            manualData.Add(new XYData(1234.3695, -0.0283459193941514));
            manualData.Add(new XYData(1234.3696, -0.0288550861953485));
            manualData.Add(new XYData(1234.3697, -0.0293512800681928));
            manualData.Add(new XYData(1234.3698, -0.0298341310857817));
            manualData.Add(new XYData(1234.3699, -0.030303274164611));
            manualData.Add(new XYData(1234.37, -0.0307583492932947));
            manualData.Add(new XYData(1234.3701, -0.0311990017658336));
            manualData.Add(new XYData(1234.3702, -0.0316248824124804));
            manualData.Add(new XYData(1234.3703, -0.0320356478292074));
            manualData.Add(new XYData(1234.3704, -0.0324309606056523));
            manualData.Add(new XYData(1234.3705, -0.032810489551405));
            manualData.Add(new XYData(1234.3706, -0.0331739099213142));
            manualData.Add(new XYData(1234.3707, -0.0335209036348007));
            manualData.Add(new XYData(1234.3708, -0.0338511595013486));
            manualData.Add(new XYData(1234.3709, -0.0341643734355136));
            manualData.Add(new XYData(1234.371, -0.0344602486738102));
            manualData.Add(new XYData(1234.3711, -0.0347384959883132));
            manualData.Add(new XYData(1234.3712, -0.0349988338976875));
            manualData.Add(new XYData(1234.3713, -0.0352409888755214));
            manualData.Add(new XYData(1234.3714, -0.035464695555831));
            manualData.Add(new XYData(1234.3715, -0.0356696969360524));
            manualData.Add(new XYData(1234.3716, -0.0358557445746861));
            manualData.Add(new XYData(1234.3717, -0.0360225987903499));
            manualData.Add(new XYData(1234.3718, -0.036170028852792));
            manualData.Add(new XYData(1234.3719, -0.0362978131728387));
            manualData.Add(new XYData(1234.372, -0.0364057394882701));
            manualData.Add(new XYData(1234.3721, -0.0364936050459901));
            manualData.Add(new XYData(1234.3722, -0.0365612167803666));
            manualData.Add(new XYData(1234.3723, -0.0366083914876224));
            manualData.Add(new XYData(1234.3724, -0.0366349559961584));
            manualData.Add(new XYData(1234.3725, -0.0366407473326777));
            manualData.Add(new XYData(1234.3726, -0.0366256128840144));
            manualData.Add(new XYData(1234.3727, -0.0365894105546262));
            manualData.Add(new XYData(1234.3728, -0.0365320089193753));
            manualData.Add(new XYData(1234.3729, -0.0364532873718035));
            manualData.Add(new XYData(1234.373, -0.0363531362676137));
            manualData.Add(new XYData(1234.3731, -0.0362314570632891));
            manualData.Add(new XYData(1234.3732, -0.0360881624497418));
            manualData.Add(new XYData(1234.3733, -0.0359231764808846));
            manualData.Add(new XYData(1234.3734, -0.0357364346965751));
            manualData.Add(new XYData(1234.3735, -0.0355278842424764));
            manualData.Add(new XYData(1234.3736, -0.0352974839802444));
            manualData.Add(new XYData(1234.3737, -0.0350452045964331));
            manualData.Add(new XYData(1234.3738, -0.0347710287037813));
            manualData.Add(new XYData(1234.3739, -0.0344749509372333));
            manualData.Add(new XYData(1234.374, -0.0341569780442059));
            manualData.Add(new XYData(1234.3741, -0.0338171289690162));
            manualData.Add(new XYData(1234.3742, -0.033455434931374));
            manualData.Add(new XYData(1234.3743, -0.0330719394979722));
            manualData.Add(new XYData(1234.3744, -0.0326666986524189));
            manualData.Add(new XYData(1234.3745, -0.0322397808502262));
            manualData.Add(new XYData(1234.3746, -0.0317912670762292));
            manualData.Add(new XYData(1234.3747, -0.0313212508913976));
            manualData.Add(new XYData(1234.3748, -0.0308298384741087));
            manualData.Add(new XYData(1234.3749, -0.0303171486549644));
            manualData.Add(new XYData(1234.375, -0.0297833129450907));
            manualData.Add(new XYData(1234.3751, -0.0292284755578535));
            manualData.Add(new XYData(1234.3752, -0.0286527934226077));
            manualData.Add(new XYData(1234.3753, -0.0280564361983637));
            manualData.Add(new XYData(1234.3754, -0.0274395862675722));
            manualData.Add(new XYData(1234.3755, -0.0268024387361571));
            manualData.Add(new XYData(1234.3756, -0.0261452014202309));
            manualData.Add(new XYData(1234.3757, -0.0254680948272128));
            manualData.Add(new XYData(1234.3758, -0.0247713521300224));
            manualData.Add(new XYData(1234.3759, -0.0240552191343139));
            manualData.Add(new XYData(1234.376, -0.023319954238717));
            manualData.Add(new XYData(1234.3761, -0.0225658283863188));
            manualData.Add(new XYData(1234.3762, -0.0217931250177366));
            manualData.Add(new XYData(1234.3763, -0.0210021399999273));
            manualData.Add(new XYData(1234.3764, -0.0201931815650117));
            manualData.Add(new XYData(1234.3765, -0.0193665702334846));
            manualData.Add(new XYData(1234.3766, -0.0185226387319972));
            manualData.Add(new XYData(1234.3767, -0.0176617319040153));
            manualData.Add(new XYData(1234.3768, -0.0167842066133435));
            manualData.Add(new XYData(1234.3769, -0.0158904316405171));
            manualData.Add(new XYData(1234.377, -0.0149807875699779));
            manualData.Add(new XYData(1234.3771, -0.0140556666805178));
            manualData.Add(new XYData(1234.3772, -0.0131154728078894));
            manualData.Add(new XYData(1234.3773, -0.0121606212209616));
            manualData.Add(new XYData(1234.3774, -0.0111915384805044));
            manualData.Add(new XYData(1234.3775, -0.0102086622929571));
            manualData.Add(new XYData(1234.3776, -0.00921244135715916));
            manualData.Add(new XYData(1234.3777, -0.00820333520406981));
            manualData.Add(new XYData(1234.3778, -0.00718181402950893));
            manualData.Add(new XYData(1234.3779, -0.00614835851759156));
            manualData.Add(new XYData(1234.378, -0.0051034596690508));
            manualData.Add(new XYData(1234.3781, -0.00404761859917232));
            manualData.Add(new XYData(1234.3782, -0.002981346352393));
            manualData.Add(new XYData(1234.3783, -0.00190516369842804));
            manualData.Add(new XYData(1234.3784, -0.000819600924030129));
            manualData.Add(new XYData(1234.3785, 0.000274802381898301));
            manualData.Add(new XYData(1234.3786, 0.0013774975497761));
            manualData.Add(new XYData(1234.3787, 0.00248792705733142));
            manualData.Add(new XYData(1234.3788, 0.00360552476658515));
            manualData.Add(new XYData(1234.3789, 0.00472971615447674));
            manualData.Add(new XYData(1234.379, 0.00585991857521437));
            manualData.Add(new XYData(1234.3791, 0.00699554150343912));
            manualData.Add(new XYData(1234.3792, 0.00813598679619919));
            manualData.Add(new XYData(1234.3793, 0.00928064895842882));
            manualData.Add(new XYData(1234.3794, 0.0104289154143717));
            manualData.Add(new XYData(1234.3795, 0.0115801667848532));
            manualData.Add(new XYData(1234.3796, 0.0127337771702996));
            manualData.Add(new XYData(1234.3797, 0.0138891144420298));
            manualData.Add(new XYData(1234.3798, 0.0150455405259429));
            manualData.Add(new XYData(1234.3799, 0.0162024117179055));
            manualData.Add(new XYData(1234.38, 0.0173590789781808));
            manualData.Add(new XYData(1234.3801, 0.0185148882441786));
            manualData.Add(new XYData(1234.3802, 0.0196691807456508));
            manualData.Add(new XYData(1234.3803, 0.0208212933248297));
            manualData.Add(new XYData(1234.3804, 0.0219705587613841));
            manualData.Add(new XYData(1234.3805, 0.023116306102056));
            manualData.Add(new XYData(1234.3806, 0.0242578609974365));
            manualData.Add(new XYData(1234.3807, 0.0253945460301882));
            manualData.Add(new XYData(1234.3808, 0.0265256810734798));
            manualData.Add(new XYData(1234.3809, 0.0276505836275776));
            manualData.Add(new XYData(1234.381, 0.0287685691733989));
            manualData.Add(new XYData(1234.3811, 0.0298789515272906));
            manualData.Add(new XYData(1234.3812, 0.0309810431994706));
            manualData.Add(new XYData(1234.3813, 0.0320741557559808));
            manualData.Add(new XYData(1234.3814, 0.0331576001839871));
            manualData.Add(new XYData(1234.3815, 0.0342306872626933));
            manualData.Add(new XYData(1234.3816, 0.0352927279251056));
            manualData.Add(new XYData(1234.3817, 0.036343033647031));
            manualData.Add(new XYData(1234.3818, 0.0373809168143206));
            manualData.Add(new XYData(1234.3819, 0.0384056911048557));
            manualData.Add(new XYData(1234.382, 0.0394166718704076));
            manualData.Add(new XYData(1234.3821, 0.0404131765206448));
            manualData.Add(new XYData(1234.3822, 0.041394524909104));
            manualData.Add(new XYData(1234.3823, 0.0423600397209409));
            manualData.Add(new XYData(1234.3824, 0.0433090468644155));
            manualData.Add(new XYData(1234.3825, 0.044240875853048));
            manualData.Add(new XYData(1234.3826, 0.0451548602105342));
            manualData.Add(new XYData(1234.3827, 0.0460503378550587));
            manualData.Add(new XYData(1234.3828, 0.0469266514952863));
            manualData.Add(new XYData(1234.3829, 0.0477831490248037));
            manualData.Add(new XYData(1234.383, 0.0486191839169925));
            manualData.Add(new XYData(1234.3831, 0.0494341156201324));
            manualData.Add(new XYData(1234.3832, 0.0502273099525347));
            manualData.Add(new XYData(1234.3833, 0.0509981394992264));
            manualData.Add(new XYData(1234.3834, 0.0517459839995684));
            manualData.Add(new XYData(1234.3835, 0.0524702307517659));
            manualData.Add(new XYData(1234.3836, 0.0531702749990031));
            manualData.Add(new XYData(1234.3837, 0.0538455203234177));
            manualData.Add(new XYData(1234.3838, 0.0544953790370871));
            manualData.Add(new XYData(1234.3839, 0.0551192725715873));
            manualData.Add(new XYData(1234.384, 0.0557166318659197));
            manualData.Add(new XYData(1234.3841, 0.0562868977525923));
            manualData.Add(new XYData(1234.3842, 0.0568295213428368));
            manualData.Add(new XYData(1234.3843, 0.057343964403485));
            manualData.Add(new XYData(1234.3844, 0.0578296997436272));
            manualData.Add(new XYData(1234.3845, 0.0582862115861655));
            manualData.Add(new XYData(1234.3846, 0.0587129959427145));
            manualData.Add(new XYData(1234.3847, 0.0591095609841035));
            manualData.Add(new XYData(1234.3848, 0.0594754274075278));
            manualData.Add(new XYData(1234.3849, 0.059810128800138));
            manualData.Add(new XYData(1234.385, 0.06011321199885));
            manualData.Add(new XYData(1234.3851, 0.0603842374467392));
            manualData.Add(new XYData(1234.3852, 0.0606227795422572));
            manualData.Add(new XYData(1234.3853, 0.0608284269901136));
            manualData.Add(new XYData(1234.3854, 0.0610007831412874));
            manualData.Add(new XYData(1234.3855, 0.0611394663313771));
            manualData.Add(new XYData(1234.3856, 0.0612441102132403));
            manualData.Add(new XYData(1234.3857, 0.0613143640843574));
            manualData.Add(new XYData(1234.3858, 0.061349893208718));
            manualData.Add(new XYData(1234.3859, 0.0613503791330081));
            manualData.Add(new XYData(1234.386, 0.0613155199967827));
            manualData.Add(new XYData(1234.3861, 0.0612450308370482));
            manualData.Add(new XYData(1234.3862, 0.0611386438856595));
            manualData.Add(new XYData(1234.3863, 0.060996108860912));
            manualData.Add(new XYData(1234.3864, 0.0608171932521442));
            manualData.Add(new XYData(1234.3865, 0.0606016825974583));
            manualData.Add(new XYData(1234.3866, 0.0603493807543241));
            manualData.Add(new XYData(1234.3867, 0.0600601101628733));
            manualData.Add(new XYData(1234.3868, 0.0597337121016851));
            manualData.Add(new XYData(1234.3869, 0.0593700469350009));
            manualData.Add(new XYData(1234.387, 0.0589689943563165));
            manualData.Add(new XYData(1234.3871, 0.0585304536155393));
            manualData.Add(new XYData(1234.3872, 0.0580543437460715));
            manualData.Add(new XYData(1234.3873, 0.0575406037794396));
            manualData.Add(new XYData(1234.3874, 0.0569891929520888));
            manualData.Add(new XYData(1234.3875, 0.0564000909033719));
            manualData.Add(new XYData(1234.3876, 0.0557732978645698));
            manualData.Add(new XYData(1234.3877, 0.0551088348387656));
            manualData.Add(new XYData(1234.3878, 0.0544067437697664));
            manualData.Add(new XYData(1234.3879, 0.0536670877096607));
            manualData.Add(new XYData(1234.388, 0.0528899509606889));
            manualData.Add(new XYData(1234.3881, 0.0520754392231973));
            manualData.Add(new XYData(1234.3882, 0.0512236797257921));
            manualData.Add(new XYData(1234.3883, 0.0503348213469448));
            manualData.Add(new XYData(1234.3884, 0.0494090347263568));
            manualData.Add(new XYData(1234.3885, 0.0484465123659353));
            manualData.Add(new XYData(1234.3886, 0.0474474687202418));
            manualData.Add(new XYData(1234.3887, 0.0464121402762833));
            manualData.Add(new XYData(1234.3888, 0.0453407856200334));
            manualData.Add(new XYData(1234.3889, 0.0442336855043435));
            manualData.Add(new XYData(1234.389, 0.0430911428813675));
            manualData.Add(new XYData(1234.3891, 0.0419134829481161));
            manualData.Add(new XYData(1234.3892, 0.0407010531686828));
            manualData.Add(new XYData(1234.3893, 0.0394542232874873));
            manualData.Add(new XYData(1234.3894, 0.0381733853310445));
            manualData.Add(new XYData(1234.3895, 0.0368589535981488));
            manualData.Add(new XYData(1234.3896, 0.0355113646383925));
            manualData.Add(new XYData(1234.3897, 0.0341310772157463));
            manualData.Add(new XYData(1234.3898, 0.0327185722760971));
            manualData.Add(new XYData(1234.3899, 0.0312743528714089));
            manualData.Add(new XYData(1234.39, 0.0297989441031924));
            manualData.Add(new XYData(1234.3901, 0.0282928930383357));
            manualData.Add(new XYData(1234.3902, 0.026756768615905));
            manualData.Add(new XYData(1234.3903, 0.0251911615417638));
            manualData.Add(new XYData(1234.3904, 0.0235966841709545));
            manualData.Add(new XYData(1234.3905, 0.0219739703777983));
            manualData.Add(new XYData(1234.3906, 0.0203236754098897));
            manualData.Add(new XYData(1234.3907, 0.0186464757485869));
            manualData.Add(new XYData(1234.3908, 0.0169430689195274));
            manualData.Add(new XYData(1234.3909, 0.015214173328197));
            manualData.Add(new XYData(1234.391, 0.0134605280644016));
            manualData.Add(new XYData(1234.3911, 0.0116828926980186));
            manualData.Add(new XYData(1234.3912, 0.0098820470623037));
            manualData.Add(new XYData(1234.3913, 0.00805879102475049));
            manualData.Add(new XYData(1234.3914, 0.00621394424551108));
            manualData.Add(new XYData(1234.3915, 0.00434834591913171));
            manualData.Add(new XYData(1234.3916, 0.00246285452513816));
            manualData.Add(new XYData(1234.3917, 0.000558347523797483));
            manualData.Add(new XYData(1234.3918, -0.00136427891812036));
            manualData.Add(new XYData(1234.3919, -0.00330411023501868));
            manualData.Add(new XYData(1234.392, -0.0052602137782388));
            manualData.Add(new XYData(1234.3921, -0.00723163914398588));
            manualData.Add(new XYData(1234.3922, -0.00921741851350459));
            manualData.Add(new XYData(1234.3923, -0.0112165670052686));
            manualData.Add(new XYData(1234.3924, -0.0132280830438852));
            manualData.Add(new XYData(1234.3925, -0.0152509487179582));
            manualData.Add(new XYData(1234.3926, -0.0172841301956916));
            manualData.Add(new XYData(1234.3927, -0.0193265781064775));
            manualData.Add(new XYData(1234.3928, -0.0213772279571021));
            manualData.Add(new XYData(1234.3929, -0.023435000554897));
            manualData.Add(new XYData(1234.393, -0.0254988024424694));
            manualData.Add(new XYData(1234.3931, -0.0275675263437257));
            manualData.Add(new XYData(1234.3932, -0.029640051621213));
            manualData.Add(new XYData(1234.3933, -0.0317152447492743));
            manualData.Add(new XYData(1234.3934, -0.0337919597746216));
            manualData.Add(new XYData(1234.3935, -0.035869038834928));
            manualData.Add(new XYData(1234.3936, -0.0379453126409957));
            manualData.Add(new XYData(1234.3937, -0.0400196009930204));
            manualData.Add(new XYData(1234.3938, -0.0420907133025715));
            manualData.Add(new XYData(1234.3939, -0.0441574491247958));
            manualData.Add(new XYData(1234.394, -0.0462185987006917));
            manualData.Add(new XYData(1234.3941, -0.0482729435092669));
            manualData.Add(new XYData(1234.3942, -0.050319256834047));
            manualData.Add(new XYData(1234.3943, -0.0523563043158611));
            manualData.Add(new XYData(1234.3944, -0.0543828445614602));
            manualData.Add(new XYData(1234.3945, -0.0563976297147001));
            manualData.Add(new XYData(1234.3946, -0.0583994060599274));
            manualData.Add(new XYData(1234.3947, -0.0603869146293992));
            manualData.Add(new XYData(1234.3948, -0.0623588918191691));
            manualData.Add(new XYData(1234.3949, -0.0643140700132492));
            manualData.Add(new XYData(1234.395, -0.0662511782157387));
            manualData.Add(new XYData(1234.3951, -0.0681689426951341));
            manualData.Add(new XYData(1234.3952, -0.070066087614375));
            manualData.Add(new XYData(1234.3953, -0.0719413357117978));
            manualData.Add(new XYData(1234.3954, -0.0737934089454165));
            manualData.Add(new XYData(1234.3955, -0.0756210291659002));
            manualData.Add(new XYData(1234.3956, -0.0774229187916149));
            manualData.Add(new XYData(1234.3957, -0.079197801489963));
            manualData.Add(new XYData(1234.3958, -0.0809444028646206));
            manualData.Add(new XYData(1234.3959, -0.0826614511485127));
            manualData.Add(new XYData(1234.396, -0.0843476779059228));
            manualData.Add(new XYData(1234.3961, -0.0860018187206833));
            manualData.Add(new XYData(1234.3962, -0.0876226139274043));
            manualData.Add(new XYData(1234.3963, -0.089208809308827));
            manualData.Add(new XYData(1234.3964, -0.0907591568166079));
            manualData.Add(new XYData(1234.3965, -0.0922724152921573));
            manualData.Add(new XYData(1234.3966, -0.0937473511910193));
            manualData.Add(new XYData(1234.3967, -0.0951827393105167));
            manualData.Add(new XYData(1234.3968, -0.0965773635203136));
            manualData.Add(new XYData(1234.3969, -0.0979300174986061));
            manualData.Add(new XYData(1234.397, -0.0992395054553406));
            manualData.Add(new XYData(1234.3971, -0.100504642888028));
            manualData.Add(new XYData(1234.3972, -0.101724257308283));
            manualData.Add(new XYData(1234.3973, -0.102897188985138));
            manualData.Add(new XYData(1234.3974, -0.104022291686123));
            manualData.Add(new XYData(1234.3975, -0.105098433418896));
            manualData.Add(new XYData(1234.3976, -0.106124497173065));
            manualData.Add(new XYData(1234.3977, -0.10709938166183));
            manualData.Add(new XYData(1234.3978, -0.108022002065161));
            manualData.Add(new XYData(1234.3979, -0.108891290761745));
            manualData.Add(new XYData(1234.398, -0.109706198080561));
            manualData.Add(new XYData(1234.3981, -0.11046569302971));
            manualData.Add(new XYData(1234.3982, -0.111168764033931));
            manualData.Add(new XYData(1234.3983, -0.11181441966757));
            manualData.Add(new XYData(1234.3984, -0.112401689384814));
            manualData.Add(new XYData(1234.3985, -0.112929624246793));
            manualData.Add(new XYData(1234.3986, -0.113397297645172));
            manualData.Add(new XYData(1234.3987, -0.11380380602271));
            manualData.Add(new XYData(1234.3988, -0.114148269585134));
            manualData.Add(new XYData(1234.3989, -0.114429833017461));
            manualData.Add(new XYData(1234.399, -0.114647666185885));
            manualData.Add(new XYData(1234.3991, -0.11480096483907));
            manualData.Add(new XYData(1234.3992, -0.114888951302621));
            manualData.Add(new XYData(1234.3993, -0.114910875167355));
            manualData.Add(new XYData(1234.3994, -0.114866013970957));
            manualData.Add(new XYData(1234.3995, -0.11475367387266));
            manualData.Add(new XYData(1234.3996, -0.114573190320046));
            manualData.Add(new XYData(1234.3997, -0.114323928710364));
            manualData.Add(new XYData(1234.3998, -0.11400528503942));
            manualData.Add(new XYData(1234.3999, -0.113616686545873));
            manualData.Add(new XYData(1234.4, -0.113157592344753));
            manualData.Add(new XYData(1234.4001, -0.11262749405199));
            manualData.Add(new XYData(1234.4002, -0.11202591639921));
            manualData.Add(new XYData(1234.4003, -0.111352417838427));
            manualData.Add(new XYData(1234.4004, -0.110606591136236));
            manualData.Add(new XYData(1234.4005, -0.10978806395517));
            manualData.Add(new XYData(1234.4006, -0.108896499433354));
            manualData.Add(new XYData(1234.4007, -0.107931596733724));
            manualData.Add(new XYData(1234.4008, -0.10689309159966));
            manualData.Add(new XYData(1234.4009, -0.105780756889152));
            manualData.Add(new XYData(1234.401, -0.10459440309796));
            manualData.Add(new XYData(1234.4011, -0.103333878869595));
            manualData.Add(new XYData(1234.4012, -0.101999071491797));
            manualData.Add(new XYData(1234.4013, -0.100589907379069));
            manualData.Add(new XYData(1234.4014, -0.0991063525375271));
            manualData.Add(new XYData(1234.4015, -0.097548413032187));
            manualData.Add(new XYData(1234.4016, -0.0959161354054148));
            manualData.Add(new XYData(1234.4017, -0.094209607113524));
            manualData.Add(new XYData(1234.4018, -0.0924289569311271));
            manualData.Add(new XYData(1234.4019, -0.0905743553426518));
            manualData.Add(new XYData(1234.402, -0.0886460149175304));
            manualData.Add(new XYData(1234.4021, -0.0866441906685797));
            manualData.Add(new XYData(1234.4022, -0.0845691803933747));
            manualData.Add(new XYData(1234.4023, -0.0824213249931826));
            manualData.Add(new XYData(1234.4024, -0.0802010087988843));
            manualData.Add(new XYData(1234.4025, -0.0779086598297792));
            manualData.Add(new XYData(1234.4026, -0.0755447500827491));
            manualData.Add(new XYData(1234.4027, -0.0731097957784943));
            manualData.Add(new XYData(1234.4028, -0.0706043575936303));
            manualData.Add(new XYData(1234.4029, -0.0680290408734536));
            manualData.Add(new XYData(1234.403, -0.0653844958251278));
            manualData.Add(new XYData(1234.4031, -0.0626714176909808));
            manualData.Add(new XYData(1234.4032, -0.059890546895237));
            manualData.Add(new XYData(1234.4033, -0.0570426692021477));
            manualData.Add(new XYData(1234.4034, -0.0541286157900206));
            manualData.Add(new XYData(1234.4035, -0.051149263367344));
            manualData.Add(new XYData(1234.4036, -0.0481055342362905));
            manualData.Add(new XYData(1234.4037, -0.0449983963409719));
            manualData.Add(new XYData(1234.4038, -0.0418288632939613));
            manualData.Add(new XYData(1234.4039, -0.038597994380844));
            manualData.Add(new XYData(1234.404, -0.0353068945425659));
            manualData.Add(new XYData(1234.4041, -0.0319567143276761));
            manualData.Add(new XYData(1234.4042, -0.0285486498602198));
            manualData.Add(new XYData(1234.4043, -0.0250839427084752));
            manualData.Add(new XYData(1234.4044, -0.0215638798066944));
            manualData.Add(new XYData(1234.4045, -0.0179897933157216));
            manualData.Add(new XYData(1234.4046, -0.0143630604677485));
            manualData.Add(new XYData(1234.4047, -0.0106851033874803));
            manualData.Add(new XYData(1234.4048, -0.00695738888954818));
            manualData.Add(new XYData(1234.4049, -0.00318142825201069));
            manualData.Add(new XYData(1234.405, 0.000641223034202634));
            manualData.Add(new XYData(1234.4051, 0.00450896554888782));
            manualData.Add(new XYData(1234.4052, 0.00842015620636872));
            manualData.Add(new XYData(1234.4053, 0.0123731086317937));
            manualData.Add(new XYData(1234.4054, 0.0163660934741442));
            manualData.Add(new XYData(1234.4055, 0.0203973387877254));
            manualData.Add(new XYData(1234.4056, 0.0244650304296644));
            manualData.Add(new XYData(1234.4057, 0.0285673124822485));
            manualData.Add(new XYData(1234.4058, 0.0327022877001671));
            manualData.Add(new XYData(1234.4059, 0.0368680179827287));
            manualData.Add(new XYData(1234.406, 0.0410625248806695));
            manualData.Add(new XYData(1234.4061, 0.0452837900802471));
            manualData.Add(new XYData(1234.4062, 0.0495297560078812));
            manualData.Add(new XYData(1234.4063, 0.0537983263645977));
            manualData.Add(new XYData(1234.4064, 0.0580873667332033));
            manualData.Add(new XYData(1234.4065, 0.0623947052010937));
            manualData.Add(new XYData(1234.4066, 0.0667181330081912));
            manualData.Add(new XYData(1234.4067, 0.0710554052199922));
            manualData.Add(new XYData(1234.4068, 0.0754042414256954));
            manualData.Add(new XYData(1234.4069, 0.0797623264712902));
            manualData.Add(new XYData(1234.407, 0.0841273111680575));
            manualData.Add(new XYData(1234.4071, 0.0884968131251171));
            manualData.Add(new XYData(1234.4072, 0.0928684175077478));
            manualData.Add(new XYData(1234.4073, 0.0972396778699985));
            manualData.Add(new XYData(1234.4074, 0.101608117002076));
            manualData.Add(new XYData(1234.4075, 0.105971227802302));
            manualData.Add(new XYData(1234.4076, 0.110326474173529));
            manualData.Add(new XYData(1234.4077, 0.11467129194387));
            manualData.Add(new XYData(1234.4078, 0.119003089821462));
            manualData.Add(new XYData(1234.4079, 0.123319250324051));
            manualData.Add(new XYData(1234.408, 0.127617130830937));
            manualData.Add(new XYData(1234.4081, 0.131894064560071));
            manualData.Add(new XYData(1234.4082, 0.136147361617984));
            manualData.Add(new XYData(1234.4083, 0.140374310063158));
            manualData.Add(new XYData(1234.4084, 0.144572176992526));
            manualData.Add(new XYData(1234.4085, 0.148738209650867));
            manualData.Add(new XYData(1234.4086, 0.152869636562877));
            manualData.Add(new XYData(1234.4087, 0.156963668696925));
            manualData.Add(new XYData(1234.4088, 0.161017500604591));
            manualData.Add(new XYData(1234.4089, 0.165028311675114));
            manualData.Add(new XYData(1234.409, 0.168993267318313));
            manualData.Add(new XYData(1234.4091, 0.172909520215545));
            manualData.Add(new XYData(1234.4092, 0.176774211582362));
            manualData.Add(new XYData(1234.4093, 0.180584472451947));
            manualData.Add(new XYData(1234.4094, 0.184337424978974));
            manualData.Add(new XYData(1234.4095, 0.188030183763584));
            manualData.Add(new XYData(1234.4096, 0.191659857203323));
            manualData.Add(new XYData(1234.4097, 0.195223548823435));
            manualData.Add(new XYData(1234.4098, 0.198718358708628));
            manualData.Add(new XYData(1234.4099, 0.202141384870837));
            manualData.Add(new XYData(1234.41, 0.205489724676774));
            manualData.Add(new XYData(1234.4101, 0.208760476285075));
            manualData.Add(new XYData(1234.4102, 0.211950740100952));
            manualData.Add(new XYData(1234.4103, 0.215057620247945));
            manualData.Add(new XYData(1234.4104, 0.218078226056344));
            manualData.Add(new XYData(1234.4105, 0.221009673574407));
            manualData.Add(new XYData(1234.4106, 0.223849087062355));
            manualData.Add(new XYData(1234.4107, 0.226593600568166));
            manualData.Add(new XYData(1234.4108, 0.229240359451363));
            manualData.Add(new XYData(1234.4109, 0.23178652195485));
            manualData.Add(new XYData(1234.411, 0.234229260783886));
            manualData.Add(new XYData(1234.4111, 0.236565764698489));
            manualData.Add(new XYData(1234.4112, 0.238793240118749));
            manualData.Add(new XYData(1234.4113, 0.24090891274256));
            manualData.Add(new XYData(1234.4114, 0.242910029179637));
            manualData.Add(new XYData(1234.4115, 0.244793858574548));
            manualData.Add(new XYData(1234.4116, 0.246557694285796));
            manualData.Add(new XYData(1234.4117, 0.248198855529395));
            manualData.Add(new XYData(1234.4118, 0.249714689055304));
            manualData.Add(new XYData(1234.4119, 0.251102570828325));
            manualData.Add(new XYData(1234.412, 0.252359907717578));
            manualData.Add(new XYData(1234.4121, 0.253484139193946));
            manualData.Add(new XYData(1234.4122, 0.254472739034944));
            manualData.Add(new XYData(1234.4123, 0.255323217038149));
            manualData.Add(new XYData(1234.4124, 0.256033120731635));
            manualData.Add(new XYData(1234.4125, 0.256600037109035));
            manualData.Add(new XYData(1234.4126, 0.257021594349883));
            manualData.Add(new XYData(1234.4127, 0.257295463554469));
            manualData.Add(new XYData(1234.4128, 0.257419360480286));
            manualData.Add(new XYData(1234.4129, 0.257391047281554));
            manualData.Add(new XYData(1234.413, 0.257208334251168));
            manualData.Add(new XYData(1234.4131, 0.256869081564406));
            manualData.Add(new XYData(1234.4132, 0.256371201022438));
            manualData.Add(new XYData(1234.4133, 0.255712657802482));
            manualData.Add(new XYData(1234.4134, 0.254891472196055));
            manualData.Add(new XYData(1234.4135, 0.253905721357287));
            manualData.Add(new XYData(1234.4136, 0.252753541044524));
            manualData.Add(new XYData(1234.4137, 0.251433127360633));
            manualData.Add(new XYData(1234.4138, 0.249942738490339));
            manualData.Add(new XYData(1234.4139, 0.248280696433925));
            manualData.Add(new XYData(1234.414, 0.246445388736577));
            manualData.Add(new XYData(1234.4141, 0.244435270207904));
            manualData.Add(new XYData(1234.4142, 0.24224886465917));
            manualData.Add(new XYData(1234.4143, 0.239884766587938));
            manualData.Add(new XYData(1234.4144, 0.23734164290086));
            manualData.Add(new XYData(1234.4145, 0.234618234606301));
            manualData.Add(new XYData(1234.4146, 0.231713358502732));
            manualData.Add(new XYData(1234.4147, 0.228625908857787));
            manualData.Add(new XYData(1234.4148, 0.225354859077267));
            manualData.Add(new XYData(1234.4149, 0.221899263363354));
            manualData.Add(new XYData(1234.415, 0.218258258352838));
            manualData.Add(new XYData(1234.4151, 0.214431064785127));
            manualData.Add(new XYData(1234.4152, 0.210416989074087));
            manualData.Add(new XYData(1234.4153, 0.206215424948547));
            manualData.Add(new XYData(1234.4154, 0.201825855037626));
            manualData.Add(new XYData(1234.4155, 0.197247852448959));
            manualData.Add(new XYData(1234.4156, 0.192481082331042));
            manualData.Add(new XYData(1234.4157, 0.187525303418963));
            manualData.Add(new XYData(1234.4158, 0.182380369562773));
            manualData.Add(new XYData(1234.4159, 0.177046231225443));
            manualData.Add(new XYData(1234.416, 0.1715229370233));
            manualData.Add(new XYData(1234.4161, 0.165810635125217));
            manualData.Add(new XYData(1234.4162, 0.159909574752389));
            manualData.Add(new XYData(1234.4163, 0.153820107597146));
            manualData.Add(new XYData(1234.4164, 0.147542689231957));
            manualData.Add(new XYData(1234.4165, 0.14107788049603));
            manualData.Add(new XYData(1234.4166, 0.134426348858756));
            manualData.Add(new XYData(1234.4167, 0.12758886975932));
            manualData.Add(new XYData(1234.4168, 0.120566327905558));
            manualData.Add(new XYData(1234.4169, 0.113359718628031));
            manualData.Add(new XYData(1234.417, 0.105970149048018));
            manualData.Add(new XYData(1234.4171, 0.098398839378114));
            manualData.Add(new XYData(1234.4172, 0.0906471241162526));
            manualData.Add(new XYData(1234.4173, 0.082716453227446));
            manualData.Add(new XYData(1234.4174, 0.074608393296789));
            manualData.Add(new XYData(1234.4175, 0.0663246286530576));
            manualData.Add(new XYData(1234.4176, 0.0578669624622297));
            manualData.Add(new XYData(1234.4177, 0.0492373177704391));
            manualData.Add(new XYData(1234.4178, 0.0404377386142208));
            manualData.Add(new XYData(1234.4179, 0.0314703909020824));
            manualData.Add(new XYData(1234.418, 0.0223375634592075));
            manualData.Add(new XYData(1234.4181, 0.0130416689412812));
            manualData.Add(new XYData(1234.4182, 0.00358524473377092));
            manualData.Add(new XYData(1234.4183, -0.00602904618339678));
            manualData.Add(new XYData(1234.4184, -0.0157984144063774));
            manualData.Add(new XYData(1234.4185, -0.0257199433137758));
            manualData.Add(new XYData(1234.4186, -0.0357905883325047));
            manualData.Add(new XYData(1234.4187, -0.0460071761256607));
            manualData.Add(new XYData(1234.4188, -0.0563664040473337));
            manualData.Add(new XYData(1234.4189, -0.0668648394069243));
            manualData.Add(new XYData(1234.419, -0.0774989188861862));
            manualData.Add(new XYData(1234.4191, -0.0882649479728283));
            manualData.Add(new XYData(1234.4192, -0.0991591004339419));
            manualData.Add(new XYData(1234.4193, -0.110177417829798));
            manualData.Add(new XYData(1234.4194, -0.12131580906852));
            manualData.Add(new XYData(1234.4195, -0.132570050027875));
            manualData.Add(new XYData(1234.4196, -0.143935783090622));
            manualData.Add(new XYData(1234.4197, -0.155408516978403));
            manualData.Add(new XYData(1234.4198, -0.166983626372859));
            manualData.Add(new XYData(1234.4199, -0.178656351707616));
            manualData.Add(new XYData(1234.42, -0.190421798978601));
            manualData.Add(new XYData(1234.4201, -0.202274939598568));
            manualData.Add(new XYData(1234.4202, -0.214210610296276));
            manualData.Add(new XYData(1234.4203, -0.22622351306072));
            manualData.Add(new XYData(1234.4204, -0.238308215130827));
            manualData.Add(new XYData(1234.4205, -0.250459149058697));
            manualData.Add(new XYData(1234.4206, -0.262670612680709));
            manualData.Add(new XYData(1234.4207, -0.274936769411665));
            manualData.Add(new XYData(1234.4208, -0.28725164830969));
            manualData.Add(new XYData(1234.4209, -0.299609144326345));
            manualData.Add(new XYData(1234.421, -0.312003018576758));
            manualData.Add(new XYData(1234.4211, -0.324426898657644));
            manualData.Add(new XYData(1234.4212, -0.336874279013499));
            manualData.Add(new XYData(1234.4213, -0.34933852135125));
            manualData.Add(new XYData(1234.4214, -0.361812855131976));
            manualData.Add(new XYData(1234.4215, -0.374290377969731));
            manualData.Add(new XYData(1234.4216, -0.386764056363245));
            manualData.Add(new XYData(1234.4217, -0.399226726193343));
            manualData.Add(new XYData(1234.4218, -0.411671093411769));
            manualData.Add(new XYData(1234.4219, -0.424089734751424));
            manualData.Add(new XYData(1234.422, -0.436475098486531));
            manualData.Add(new XYData(1234.4221, -0.448819505242881));
            manualData.Add(new XYData(1234.4222, -0.461115148858298));
            manualData.Add(new XYData(1234.4223, -0.473354097321175));
            manualData.Add(new XYData(1234.4224, -0.485528293620505));
            manualData.Add(new XYData(1234.4225, -0.497629556924579));
            manualData.Add(new XYData(1234.4226, -0.509649583531815));
            manualData.Add(new XYData(1234.4227, -0.521579948011545));
            manualData.Add(new XYData(1234.4228, -0.533412104367557));
            manualData.Add(new XYData(1234.4229, -0.545137387252287));
            manualData.Add(new XYData(1234.423, -0.556747013231676));
            manualData.Add(new XYData(1234.4231, -0.568232082100628));
            manualData.Add(new XYData(1234.4232, -0.579583578274737));
            manualData.Add(new XYData(1234.4233, -0.590792372103921));
            manualData.Add(new XYData(1234.4234, -0.601849221494363));
            manualData.Add(new XYData(1234.4235, -0.612744773322326));
            manualData.Add(new XYData(1234.4236, -0.623469565027767));
            manualData.Add(new XYData(1234.4237, -0.634014026232086));
            manualData.Add(new XYData(1234.4238, -0.644368480405841));
            manualData.Add(new XYData(1234.4239, -0.654523146586264));
            manualData.Add(new XYData(1234.424, -0.664468141144393));
            manualData.Add(new XYData(1234.4241, -0.674193479623462));
            manualData.Add(new XYData(1234.4242, -0.683689078516671));
            manualData.Add(new XYData(1234.4243, -0.692944757314577));
            manualData.Add(new XYData(1234.4244, -0.701950240379479));
            manualData.Add(new XYData(1234.4245, -0.710695158979564));
            manualData.Add(new XYData(1234.4246, -0.719169053348807));
            manualData.Add(new XYData(1234.4247, -0.727361374794676));
            manualData.Add(new XYData(1234.4248, -0.735261487853315));
            manualData.Add(new XYData(1234.4249, -0.742858672491841));
            manualData.Add(new XYData(1234.425, -0.750142126373616));
            manualData.Add(new XYData(1234.4251, -0.757100967088134));
            manualData.Add(new XYData(1234.4252, -0.763724234591835));
            manualData.Add(new XYData(1234.4253, -0.770000893528055));
            manualData.Add(new XYData(1234.4254, -0.775919835676006));
            manualData.Add(new XYData(1234.4255, -0.781469882427463));
            manualData.Add(new XYData(1234.4256, -0.786639787307634));
            manualData.Add(new XYData(1234.4257, -0.791418238539693));
            manualData.Add(new XYData(1234.4258, -0.795793861652479));
            manualData.Add(new XYData(1234.4259, -0.799755222139355));
            manualData.Add(new XYData(1234.426, -0.803290828115533));
            manualData.Add(new XYData(1234.4261, -0.806389133106089));
            manualData.Add(new XYData(1234.4262, -0.809038538783828));
            manualData.Add(new XYData(1234.4263, -0.811227397793973));
            manualData.Add(new XYData(1234.4264, -0.812944016609093));
            manualData.Add(new XYData(1234.4265, -0.814176658423105));
            manualData.Add(new XYData(1234.4266, -0.814913546083716));
            manualData.Add(new XYData(1234.4267, -0.815142865062633));
            manualData.Add(new XYData(1234.4268, -0.81485276646158));
            manualData.Add(new XYData(1234.4269, -0.814031370059785));
            manualData.Add(new XYData(1234.427, -0.812666767389383));
            manualData.Add(new XYData(1234.4271, -0.810747024851096));
            manualData.Add(new XYData(1234.4272, -0.808260186862591));
            manualData.Add(new XYData(1234.4273, -0.805194279040329));
            manualData.Add(new XYData(1234.4274, -0.801537311414025));
            manualData.Add(new XYData(1234.4275, -0.797277281672954));
            manualData.Add(new XYData(1234.4276, -0.792402178443273));
            manualData.Add(new XYData(1234.4277, -0.786899984582307));
            manualData.Add(new XYData(1234.4278, -0.780758680566874));
            manualData.Add(new XYData(1234.4279, -0.773966247783632));
            manualData.Add(new XYData(1234.428, -0.766510671970639));
            manualData.Add(new XYData(1234.4281, -0.758379946625294));
            manualData.Add(new XYData(1234.4282, -0.74956207644989));
            manualData.Add(new XYData(1234.4283, -0.740045080822105));
            manualData.Add(new XYData(1234.4284, -0.729816997289488));
            manualData.Add(new XYData(1234.4285, -0.718865885087012));
            manualData.Add(new XYData(1234.4286, -0.707179828649297));
            manualData.Add(new XYData(1234.4287, -0.694746941279368));
            manualData.Add(new XYData(1234.4288, -0.681555368570223));
            manualData.Add(new XYData(1234.4289, -0.667593292109546));
            manualData.Add(new XYData(1234.429, -0.652848933072285));
            manualData.Add(new XYData(1234.4291, -0.637310555856111));
            manualData.Add(new XYData(1234.4292, -0.620966471733014));
            manualData.Add(new XYData(1234.4293, -0.603805042516));
            manualData.Add(new XYData(1234.4294, -0.585814684239848));
            manualData.Add(new XYData(1234.4295, -0.566983870811062));
            manualData.Add(new XYData(1234.4296, -0.547301137886713));
            manualData.Add(new XYData(1234.4297, -0.52675508633412));
            manualData.Add(new XYData(1234.4298, -0.505334386126456));
            manualData.Add(new XYData(1234.4299, -0.483027780035853));
            manualData.Add(new XYData(1234.43, -0.459824087375935));
            manualData.Add(new XYData(1234.4301, -0.435712207750807));
            manualData.Add(new XYData(1234.4302, -0.410681124809371));
            manualData.Add(new XYData(1234.4303, -0.384719910003855));
            manualData.Add(new XYData(1234.4304, -0.357817726289172));
            manualData.Add(new XYData(1234.4305, -0.329963832133255));
            manualData.Add(new XYData(1234.4306, -0.301147584914387));
            manualData.Add(new XYData(1234.4307, -0.271358444927647));
            manualData.Add(new XYData(1234.4308, -0.240585979087206));
            manualData.Add(new XYData(1234.4309, -0.208819864686025));
            manualData.Add(new XYData(1234.431, -0.176049893151662));
            manualData.Add(new XYData(1234.4311, -0.142265973797037));
            manualData.Add(new XYData(1234.4312, -0.107458137564958));
            manualData.Add(new XYData(1234.4313, -0.0716165406825451));
            manualData.Add(new XYData(1234.4314, -0.0347314687180537));
            manualData.Add(new XYData(1234.4315, 0.00320666018937892));
            manualData.Add(new XYData(1234.4316, 0.0422072912984368));
            manualData.Add(new XYData(1234.4317, 0.0822797296504515));
            manualData.Add(new XYData(1234.4318, 0.123433136387312));
            manualData.Add(new XYData(1234.4319, 0.165676525083963));
            manualData.Add(new XYData(1234.432, 0.209018758096734));
            manualData.Add(new XYData(1234.4321, 0.253468542928651));
            manualData.Add(new XYData(1234.4322, 0.299034428717858));
            manualData.Add(new XYData(1234.4323, 0.345724802223613));
            manualData.Add(new XYData(1234.4324, 0.393547884871483));
            manualData.Add(new XYData(1234.4325, 0.442511728786886));
            manualData.Add(new XYData(1234.4326, 0.492624213365877));
            manualData.Add(new XYData(1234.4327, 0.543893041767814));
            manualData.Add(new XYData(1234.4328, 0.596325737433493));
            manualData.Add(new XYData(1234.4329, 0.649929640629884));
            manualData.Add(new XYData(1234.433, 0.704711905022713));
            manualData.Add(new XYData(1234.4331, 0.760679494406665));
            manualData.Add(new XYData(1234.4332, 0.817839178825473));
            manualData.Add(new XYData(1234.4333, 0.87619753199864));
            manualData.Add(new XYData(1234.4334, 0.935760927510894));
            manualData.Add(new XYData(1234.4335, 0.996535535667619));
            manualData.Add(new XYData(1234.4336, 1.0585273202593));
            manualData.Add(new XYData(1234.4337, 1.12174203536207));
            manualData.Add(new XYData(1234.4338, 1.18618522217543));
            manualData.Add(new XYData(1234.4339, 1.25186220589849));
            manualData.Add(new XYData(1234.434, 1.31877809279918));
            manualData.Add(new XYData(1234.4341, 1.38693776655926));
            manualData.Add(new XYData(1234.4342, 1.4563458861854));
            manualData.Add(new XYData(1234.4343, 1.52700688244454));
            manualData.Add(new XYData(1234.4344, 1.59892495509981));
            manualData.Add(new XYData(1234.4345, 1.67210407004087));
            manualData.Add(new XYData(1234.4346, 1.74654795646052));
            manualData.Add(new XYData(1234.4347, 1.82226010407857));
            manualData.Add(new XYData(1234.4348, 1.89924376041425));
            manualData.Add(new XYData(1234.4349, 1.97750192828736));
            manualData.Add(new XYData(1234.435, 2.05703736247599));
            manualData.Add(new XYData(1234.4351, 2.13785256820808));
            manualData.Add(new XYData(1234.4352, 2.21994979792908));
            manualData.Add(new XYData(1234.4353, 2.30333104900867));
            manualData.Add(new XYData(1234.4354, 2.38799806132552));
            manualData.Add(new XYData(1234.4355, 2.47395231490777));
            manualData.Add(new XYData(1234.4356, 2.56119502763006));
            manualData.Add(new XYData(1234.4357, 2.64972715296824));
            manualData.Add(new XYData(1234.4358, 2.73954937781252));
            manualData.Add(new XYData(1234.4359, 2.83066212054878));
            manualData.Add(new XYData(1234.436, 2.92306552816008));
            manualData.Add(new XYData(1234.4361, 3.01675947546402));
            manualData.Add(new XYData(1234.4362, 3.11174356234366));
            manualData.Add(new XYData(1234.4363, 3.20801711207294));
            manualData.Add(new XYData(1234.4364, 3.30557916950029));
            manualData.Add(new XYData(1234.4365, 3.40442849929715));
            manualData.Add(new XYData(1234.4366, 3.50456358427209));
            manualData.Add(new XYData(1234.4367, 3.60598262375132));
            manualData.Add(new XYData(1234.4368, 3.70868353226153));
            manualData.Add(new XYData(1234.4369, 3.81266393710871));
            manualData.Add(new XYData(1234.437, 3.91792117836387));
            manualData.Add(new XYData(1234.4371, 4.02445230658605));
            manualData.Add(new XYData(1234.4372, 4.13225408177881));
            manualData.Add(new XYData(1234.4373, 4.24132297218551));
            manualData.Add(new XYData(1234.4374, 4.35165515315641));
            manualData.Add(new XYData(1234.4375, 4.46324650608791));
            manualData.Add(new XYData(1234.4376, 4.57609261743485));
            manualData.Add(new XYData(1234.4377, 4.69018877805722));
            manualData.Add(new XYData(1234.4378, 4.80552998133961));
            manualData.Add(new XYData(1234.4379, 4.92211092398264));
            manualData.Add(new XYData(1234.438, 5.03992600427821));
            manualData.Add(new XYData(1234.4381, 5.15896932175351));
            manualData.Add(new XYData(1234.4382, 5.27923467663354));
            manualData.Add(new XYData(1234.4383, 5.40071556938034));
            manualData.Add(new XYData(1234.4384, 5.52340520030979));
            manualData.Add(new XYData(1234.4385, 5.64729646928611));
            manualData.Add(new XYData(1234.4386, 5.77238197578047));
            manualData.Add(new XYData(1234.4387, 5.89865401758214));
            manualData.Add(new XYData(1234.4388, 6.02610459243556));
            manualData.Add(new XYData(1234.4389, 6.15472539691535));
            manualData.Add(new XYData(1234.439, 6.28450782679925));
            manualData.Add(new XYData(1234.4391, 6.41544297723795));
            manualData.Add(new XYData(1234.4392, 6.54752164300539));
            manualData.Add(new XYData(1234.4393, 6.68073431882969));
            manualData.Add(new XYData(1234.4394, 6.81507119980505));
            manualData.Add(new XYData(1234.4395, 6.95052218219396));
            manualData.Add(new XYData(1234.4396, 7.08707686276712));
            manualData.Add(new XYData(1234.4397, 7.22472454130714));
            manualData.Add(new XYData(1234.4398, 7.3634542201167));
            manualData.Add(new XYData(1234.4399, 7.50325460514458));
            manualData.Add(new XYData(1234.44, 7.64411410688683));
            manualData.Add(new XYData(1234.4401, 7.78602084136995));
            manualData.Add(new XYData(1234.4402, 7.92896263121611));
            manualData.Add(new XYData(1234.4403, 8.07292700679038));
            manualData.Add(new XYData(1234.4404, 8.21790120776083));
            manualData.Add(new XYData(1234.4405, 8.36387218308899));
            manualData.Add(new XYData(1234.4406, 8.51082659440103));
            manualData.Add(new XYData(1234.4407, 8.65875081614675));
            manualData.Add(new XYData(1234.4408, 8.80763093748504));
            manualData.Add(new XYData(1234.4409, 8.95745276392269));
            manualData.Add(new XYData(1234.441, 9.10820181903458));
            manualData.Add(new XYData(1234.4411, 9.25986334626537));
            manualData.Add(new XYData(1234.4412, 9.41242231081228));
            manualData.Add(new XYData(1234.4413, 9.56586340193868));
            manualData.Add(new XYData(1234.4414, 9.720171033621));
            manualData.Add(new XYData(1234.4415, 9.87532934876691));
            manualData.Add(new XYData(1234.4416, 10.0313222200267));
            manualData.Add(new XYData(1234.4417, 10.188133252426));
            manualData.Add(new XYData(1234.4418, 10.3457457857297));
            manualData.Add(new XYData(1234.4419, 10.5041428968854));
            manualData.Add(new XYData(1234.442, 10.6633074025453));
            manualData.Add(new XYData(1234.4421, 10.8232218616662));
            manualData.Add(new XYData(1234.4422, 10.9838685785534));
            manualData.Add(new XYData(1234.4423, 11.1452296041532));
            manualData.Add(new XYData(1234.4424, 11.3072867410768));
            manualData.Add(new XYData(1234.4425, 11.4700215450484));
            manualData.Add(new XYData(1234.4426, 11.6334153282545));
            manualData.Add(new XYData(1234.4427, 11.797449162402));
            manualData.Add(new XYData(1234.4428, 11.9621038818525));
            manualData.Add(new XYData(1234.4429, 12.1273600868296));
            manualData.Add(new XYData(1234.443, 12.2931981466995));
            manualData.Add(new XYData(1234.4431, 12.4595982037037));
            manualData.Add(new XYData(1234.4432, 12.626540174869));
            manualData.Add(new XYData(1234.4433, 12.7940037577747));
            manualData.Add(new XYData(1234.4434, 12.9619684326066));
            manualData.Add(new XYData(1234.4435, 13.1304134661719));
            manualData.Add(new XYData(1234.4436, 13.2993179156037));
            manualData.Add(new XYData(1234.4437, 13.468660632135));
            manualData.Add(new XYData(1234.4438, 13.6384202649391));
            manualData.Add(new XYData(1234.4439, 13.8085752650361));
            manualData.Add(new XYData(1234.444, 13.9791038896542));
            manualData.Add(new XYData(1234.4441, 14.149984204714));
            manualData.Add(new XYData(1234.4442, 14.3211940912561));
            manualData.Add(new XYData(1234.4443, 14.4927112480537));
            manualData.Add(new XYData(1234.4444, 14.6645131962253));
            manualData.Add(new XYData(1234.4445, 14.8365772835202));
            manualData.Add(new XYData(1234.4446, 15.0088806886636));
            manualData.Add(new XYData(1234.4447, 15.1814004257608));
            manualData.Add(new XYData(1234.4448, 15.3541133487591));
            manualData.Add(new XYData(1234.4449, 15.5269961563586));
            manualData.Add(new XYData(1234.445, 15.7000253950139));
            manualData.Add(new XYData(1234.4451, 15.8731774659199));
            manualData.Add(new XYData(1234.4452, 16.0464286281215));
            manualData.Add(new XYData(1234.4453, 16.2197550036394));
            manualData.Add(new XYData(1234.4454, 16.3931325822544));
            manualData.Add(new XYData(1234.4455, 16.5665372263414));
            manualData.Add(new XYData(1234.4456, 16.7399446757511));
            manualData.Add(new XYData(1234.4457, 16.9133305527403));
            manualData.Add(new XYData(1234.4458, 17.0866703673413));
            manualData.Add(new XYData(1234.4459, 17.2599395208056));
            manualData.Add(new XYData(1234.446, 17.4331133130331));
            manualData.Add(new XYData(1234.4461, 17.6061669461015));
            manualData.Add(new XYData(1234.4462, 17.7790755298071));
            manualData.Add(new XYData(1234.4463, 17.9518140868519));
            manualData.Add(new XYData(1234.4464, 18.1243575580684));
            manualData.Add(new XYData(1234.4465, 18.296680807681));
            manualData.Add(new XYData(1234.4466, 18.4687586286039));
            manualData.Add(new XYData(1234.4467, 18.6405657481626));
            manualData.Add(new XYData(1234.4468, 18.8120768318962));
            manualData.Add(new XYData(1234.4469, 18.9832664912971));
            manualData.Add(new XYData(1234.447, 19.1541092876734));
            manualData.Add(new XYData(1234.4471, 19.324579737994));
            manualData.Add(new XYData(1234.4472, 19.4946523203696));
            manualData.Add(new XYData(1234.4473, 19.66430147956));
            manualData.Add(new XYData(1234.4474, 19.8335016325047));
            manualData.Add(new XYData(1234.4475, 20.0022271738773));
            manualData.Add(new XYData(1234.4476, 20.1704524820417));
            manualData.Add(new XYData(1234.4477, 20.3381519231185));
            manualData.Add(new XYData(1234.4478, 20.5052998588929));
            manualData.Add(new XYData(1234.4479, 20.6718706509132));
            manualData.Add(new XYData(1234.448, 20.8378386665188));
            manualData.Add(new XYData(1234.4481, 21.0031782844991));
            manualData.Add(new XYData(1234.4482, 21.1678639007637));
            manualData.Add(new XYData(1234.4483, 21.3318699340245));
            manualData.Add(new XYData(1234.4484, 21.4951708314857));
            manualData.Add(new XYData(1234.4485, 21.6577410749112));
            manualData.Add(new XYData(1234.4486, 21.8195551848508));
            manualData.Add(new XYData(1234.4487, 21.9805877285676));
            manualData.Add(new XYData(1234.4488, 22.1408133242684));
            manualData.Add(new XYData(1234.4489, 22.3002066471868));
            manualData.Add(new XYData(1234.449, 22.4587424352958));
            manualData.Add(new XYData(1234.4491, 22.6163954950181));
            manualData.Add(new XYData(1234.4492, 22.7731407069332));
            manualData.Add(new XYData(1234.4493, 22.9289530314795));
            manualData.Add(new XYData(1234.4494, 23.0838075150005));
            manualData.Add(new XYData(1234.4495, 23.2376792940265));
            manualData.Add(new XYData(1234.4496, 23.3905436030641));
            manualData.Add(new XYData(1234.4497, 23.5423757788526));
            manualData.Add(new XYData(1234.4498, 23.69315126637));
            manualData.Add(new XYData(1234.4499, 23.8428456244723));
            manualData.Add(new XYData(1234.45, 23.9914345315162));
            manualData.Add(new XYData(1234.4501, 24.1388937909645));
            manualData.Add(new XYData(1234.4502, 24.2851993369722));
            manualData.Add(new XYData(1234.4503, 24.4303272402804));
            manualData.Add(new XYData(1234.4504, 24.5742537124421));
            manualData.Add(new XYData(1234.4505, 24.7169551133216));
            manualData.Add(new XYData(1234.4506, 24.8584079552651));
            manualData.Add(new XYData(1234.4507, 24.9985889088994));
            manualData.Add(new XYData(1234.4508, 25.1374748085696));
            manualData.Add(new XYData(1234.4509, 25.275042657748));
            manualData.Add(new XYData(1234.451, 25.4112696344101));
            manualData.Add(new XYData(1234.4511, 25.5461330963785));
            manualData.Add(new XYData(1234.4512, 25.6796105869339));
            manualData.Add(new XYData(1234.4513, 25.8116798388767));
            manualData.Add(new XYData(1234.4514, 25.9423187815834));
            manualData.Add(new XYData(1234.4515, 26.0715055449858));
            manualData.Add(new XYData(1234.4516, 26.1992184650311));
            manualData.Add(new XYData(1234.4517, 26.3254360887958));
            manualData.Add(new XYData(1234.4518, 26.4501371795551));
            manualData.Add(new XYData(1234.4519, 26.5733007218087));
            manualData.Add(new XYData(1234.452, 26.6949059262584));
            manualData.Add(new XYData(1234.4521, 26.8149322347398));
            manualData.Add(new XYData(1234.4522, 26.9333593253705));
            manualData.Add(new XYData(1234.4523, 27.0501671163079));
            manualData.Add(new XYData(1234.4524, 27.165335772144));
            manualData.Add(new XYData(1234.4525, 27.2788457075495));
            manualData.Add(new XYData(1234.4526, 27.3906775922161));
            manualData.Add(new XYData(1234.4527, 27.5008123554711));
            manualData.Add(new XYData(1234.4528, 27.6092311908344));
            manualData.Add(new XYData(1234.4529, 27.7159155605173));
            manualData.Add(new XYData(1234.453, 27.8208471998608));
            manualData.Add(new XYData(1234.4531, 27.9240081219452));
            manualData.Add(new XYData(1234.4532, 28.0253806209708));
            manualData.Add(new XYData(1234.4533, 28.1249472779157));
            manualData.Add(new XYData(1234.4534, 28.2226909637797));
            manualData.Add(new XYData(1234.4535, 28.31859484394));
            manualData.Add(new XYData(1234.4536, 28.4126423822032));
            manualData.Add(new XYData(1234.4537, 28.5048173447892));
            manualData.Add(new XYData(1234.4538, 28.5951038042443));
            manualData.Add(new XYData(1234.4539, 28.6834861432843));
            manualData.Add(new XYData(1234.454, 28.7699490587594));
            manualData.Add(new XYData(1234.4541, 28.8544775645699));
            manualData.Add(new XYData(1234.4542, 28.9370569964695));
            manualData.Add(new XYData(1234.4543, 29.017673014822));
            manualData.Add(new XYData(1234.4544, 29.0963116082729));
            manualData.Add(new XYData(1234.4545, 29.1729590971431));
            manualData.Add(new XYData(1234.4546, 29.247602136746));
            manualData.Add(new XYData(1234.4547, 29.3202277206242));
            manualData.Add(new XYData(1234.4548, 29.3908231837066));
            manualData.Add(new XYData(1234.4549, 29.4593762055376));
            manualData.Add(new XYData(1234.455, 29.5258748126525));
            manualData.Add(new XYData(1234.4551, 29.5903073824232));
            manualData.Add(new XYData(1234.4552, 29.6526626452577));
            manualData.Add(new XYData(1234.4553, 29.712929687501));
            manualData.Add(new XYData(1234.4554, 29.7710979540923));
            manualData.Add(new XYData(1234.4555, 29.8271572511363));
            manualData.Add(new XYData(1234.4556, 29.8810977483878));
            manualData.Add(new XYData(1234.4557, 29.9329099816481));
            manualData.Add(new XYData(1234.4558, 29.9825848551845));
            manualData.Add(new XYData(1234.4559, 30.0301136435024));
            manualData.Add(new XYData(1234.456, 30.0754879941533));
            manualData.Add(new XYData(1234.4561, 30.1186999293178));
            manualData.Add(new XYData(1234.4562, 30.1597418478693));
            manualData.Add(new XYData(1234.4563, 30.1986065272322));
            manualData.Add(new XYData(1234.4564, 30.2352871251467));
            manualData.Add(new XYData(1234.4565, 30.2697771813421));
            manualData.Add(new XYData(1234.4566, 30.3020706191166));
            manualData.Add(new XYData(1234.4567, 30.3321617468903));
            manualData.Add(new XYData(1234.4568, 30.3600452593287));
            manualData.Add(new XYData(1234.4569, 30.385716239052));
            manualData.Add(new XYData(1234.457, 30.4091701575603));
            manualData.Add(new XYData(1234.4571, 30.4304028764131));
            manualData.Add(new XYData(1234.4572, 30.4494106482429));
            manualData.Add(new XYData(1234.4573, 30.4661901176733));
            manualData.Add(new XYData(1234.4574, 30.4807383221404));
            manualData.Add(new XYData(1234.4575, 30.4930526926195));
            manualData.Add(new XYData(1234.4576, 30.5031310542742));
            manualData.Add(new XYData(1234.4577, 30.510971626903));
            manualData.Add(new XYData(1234.4578, 30.5165730255132));
            manualData.Add(new XYData(1234.4579, 30.5199342605638));
            manualData.Add(new XYData(1234.4581, 30.5199342605638));
            manualData.Add(new XYData(1234.4582, 30.5165730255132));
            manualData.Add(new XYData(1234.4583, 30.5109716269029));
            manualData.Add(new XYData(1234.4584, 30.5031310542742));
            manualData.Add(new XYData(1234.4585, 30.4930526926195));
            manualData.Add(new XYData(1234.4586, 30.4807383221404));
            manualData.Add(new XYData(1234.4587, 30.4661901176733));
            manualData.Add(new XYData(1234.4588, 30.4494106482429));
            manualData.Add(new XYData(1234.4589, 30.4304028764131));
            manualData.Add(new XYData(1234.459, 30.4091701575603));
            manualData.Add(new XYData(1234.4591, 30.385716239052));
            manualData.Add(new XYData(1234.4592, 30.3600452593287));
            manualData.Add(new XYData(1234.4593, 30.3321617468903));
            manualData.Add(new XYData(1234.4594, 30.3020706191166));
            manualData.Add(new XYData(1234.4595, 30.2697771813421));
            manualData.Add(new XYData(1234.4596, 30.2352871251467));
            manualData.Add(new XYData(1234.4597, 30.1986065272322));
            manualData.Add(new XYData(1234.4598, 30.1597418478693));
            manualData.Add(new XYData(1234.4599, 30.1186999293178));
            manualData.Add(new XYData(1234.46, 30.0754879941533));
            manualData.Add(new XYData(1234.4601, 30.0301136435024));
            manualData.Add(new XYData(1234.4602, 29.9825848551845));
            manualData.Add(new XYData(1234.4603, 29.9329099816481));
            manualData.Add(new XYData(1234.4604, 29.8810977483878));
            manualData.Add(new XYData(1234.4605, 29.8271572511363));
            manualData.Add(new XYData(1234.4606, 29.7710979540923));
            manualData.Add(new XYData(1234.4607, 29.712929687501));
            manualData.Add(new XYData(1234.4608, 29.6526626452577));
            manualData.Add(new XYData(1234.4609, 29.5903073824232));
            manualData.Add(new XYData(1234.461, 29.5258748126525));
            manualData.Add(new XYData(1234.4611, 29.4593762055376));
            manualData.Add(new XYData(1234.4612, 29.3908231837066));
            manualData.Add(new XYData(1234.4613, 29.3202277206242));
            manualData.Add(new XYData(1234.4614, 29.247602136746));
            manualData.Add(new XYData(1234.4615, 29.1729590971431));
            manualData.Add(new XYData(1234.4616, 29.0963116082729));
            manualData.Add(new XYData(1234.4617, 29.017673014822));
            manualData.Add(new XYData(1234.4618, 28.9370569964695));
            manualData.Add(new XYData(1234.4619, 28.8544775645699));
            manualData.Add(new XYData(1234.462, 28.7699490587594));
            manualData.Add(new XYData(1234.4621, 28.6834861432843));
            manualData.Add(new XYData(1234.4622, 28.5951038042443));
            manualData.Add(new XYData(1234.4623, 28.5048173447892));
            manualData.Add(new XYData(1234.4624, 28.4126423822032));
            manualData.Add(new XYData(1234.4625, 28.31859484394));
            manualData.Add(new XYData(1234.4626, 28.2226909637797));
            manualData.Add(new XYData(1234.4627, 28.1249472779157));
            manualData.Add(new XYData(1234.4628, 28.0253806209708));
            manualData.Add(new XYData(1234.4629, 27.9240081219452));
            manualData.Add(new XYData(1234.463, 27.8208471998608));
            manualData.Add(new XYData(1234.4631, 27.7159155605173));
            manualData.Add(new XYData(1234.4632, 27.6092311908344));
            manualData.Add(new XYData(1234.4633, 27.5008123554711));
            manualData.Add(new XYData(1234.4634, 27.3906775922161));
            manualData.Add(new XYData(1234.4635, 27.2788457075495));
            manualData.Add(new XYData(1234.4636, 27.165335772144));
            manualData.Add(new XYData(1234.4637, 27.0501671163079));
            manualData.Add(new XYData(1234.4638, 26.9333593253705));
            manualData.Add(new XYData(1234.4639, 26.8149322347398));
            manualData.Add(new XYData(1234.464, 26.6949059262584));
            manualData.Add(new XYData(1234.4641, 26.5733007218087));
            manualData.Add(new XYData(1234.4642, 26.4501371795551));
            manualData.Add(new XYData(1234.4643, 26.3254360887958));
            manualData.Add(new XYData(1234.4644, 26.1992184650311));
            manualData.Add(new XYData(1234.4645, 26.0715055449858));
            manualData.Add(new XYData(1234.4646, 25.9423187815834));
            manualData.Add(new XYData(1234.4647, 25.8116798388767));
            manualData.Add(new XYData(1234.4648, 25.679610586632));
            manualData.Add(new XYData(1234.4649, 25.5461330963785));
            manualData.Add(new XYData(1234.465, 25.4112696344101));
            manualData.Add(new XYData(1234.4651, 25.275042657748));
            manualData.Add(new XYData(1234.4652, 25.1374748085696));
            manualData.Add(new XYData(1234.4653, 24.9985889088994));
            manualData.Add(new XYData(1234.4654, 24.8584079552651));
            manualData.Add(new XYData(1234.4655, 24.7169551133216));
            manualData.Add(new XYData(1234.4656, 24.5742537124421));
            manualData.Add(new XYData(1234.4657, 24.4303272399518));
            manualData.Add(new XYData(1234.4658, 24.2851993369722));
            manualData.Add(new XYData(1234.4659, 24.1388937909645));
            manualData.Add(new XYData(1234.466, 23.9914345315162));
            manualData.Add(new XYData(1234.4661, 23.8428456244723));
            manualData.Add(new XYData(1234.4662, 23.69315126637));
            manualData.Add(new XYData(1234.4663, 23.5423757788526));
            manualData.Add(new XYData(1234.4664, 23.3905436030641));
            manualData.Add(new XYData(1234.4665, 23.2376792940265));
            manualData.Add(new XYData(1234.4666, 23.0838075146495));
            manualData.Add(new XYData(1234.4667, 22.9289530314795));
            manualData.Add(new XYData(1234.4668, 22.7731407069332));
            manualData.Add(new XYData(1234.4669, 22.6163954950181));
            manualData.Add(new XYData(1234.467, 22.4587424352958));
            manualData.Add(new XYData(1234.4671, 22.3002066471868));
            manualData.Add(new XYData(1234.4672, 22.1408133242684));
            manualData.Add(new XYData(1234.4673, 21.9805877285676));
            manualData.Add(new XYData(1234.4674, 21.8195551848508));
            manualData.Add(new XYData(1234.4675, 21.6577410749112));
            manualData.Add(new XYData(1234.4676, 21.4951708314857));
            manualData.Add(new XYData(1234.4677, 21.3318699340245));
            manualData.Add(new XYData(1234.4678, 21.1678639007637));
            manualData.Add(new XYData(1234.4679, 21.0031782844991));
            manualData.Add(new XYData(1234.468, 20.8378386665188));
            manualData.Add(new XYData(1234.4681, 20.6718706509132));
            manualData.Add(new XYData(1234.4682, 20.5052998588929));
            manualData.Add(new XYData(1234.4683, 20.3381519231185));
            manualData.Add(new XYData(1234.4684, 20.1704524820417));
            manualData.Add(new XYData(1234.4685, 20.0022271738773));
            manualData.Add(new XYData(1234.4686, 19.8335016325047));
            manualData.Add(new XYData(1234.4687, 19.66430147956));
            manualData.Add(new XYData(1234.4688, 19.4946523203696));
            manualData.Add(new XYData(1234.4689, 19.324579737994));
            manualData.Add(new XYData(1234.469, 19.1541092876734));
            manualData.Add(new XYData(1234.4691, 18.9832664912971));
            manualData.Add(new XYData(1234.4692, 18.8120768318962));
            manualData.Add(new XYData(1234.4693, 18.6405657481626));
            manualData.Add(new XYData(1234.4694, 18.4687586286039));
            manualData.Add(new XYData(1234.4695, 18.296680807681));
            manualData.Add(new XYData(1234.4696, 18.1243575580684));
            manualData.Add(new XYData(1234.4697, 17.9518140868519));
            manualData.Add(new XYData(1234.4698, 17.7790755298071));
            manualData.Add(new XYData(1234.4699, 17.6061669461015));
            manualData.Add(new XYData(1234.47, 17.4331133130331));
            manualData.Add(new XYData(1234.4701, 17.2599395208056));
            manualData.Add(new XYData(1234.4702, 17.0866703673413));
            manualData.Add(new XYData(1234.4703, 16.9133305527403));
            manualData.Add(new XYData(1234.4704, 16.7399446757511));
            manualData.Add(new XYData(1234.4705, 16.5665372263414));
            manualData.Add(new XYData(1234.4706, 16.3931325822544));
            manualData.Add(new XYData(1234.4707, 16.2197550036394));
            manualData.Add(new XYData(1234.4708, 16.0464286281215));
            manualData.Add(new XYData(1234.4709, 15.8731774659199));
            manualData.Add(new XYData(1234.471, 15.7000253950139));
            manualData.Add(new XYData(1234.4711, 15.5269961563586));
            manualData.Add(new XYData(1234.4712, 15.3541133487591));
            manualData.Add(new XYData(1234.4713, 15.1814004257608));
            manualData.Add(new XYData(1234.4714, 15.0088806886636));
            manualData.Add(new XYData(1234.4715, 14.8365772835202));
            manualData.Add(new XYData(1234.4716, 14.6645131962253));
            manualData.Add(new XYData(1234.4717, 14.4927112480537));
            manualData.Add(new XYData(1234.4718, 14.3211940912561));
            manualData.Add(new XYData(1234.4719, 14.149984204714));
            manualData.Add(new XYData(1234.472, 13.9791038896542));
            manualData.Add(new XYData(1234.4721, 13.8085752650361));
            manualData.Add(new XYData(1234.4722, 13.6384202649391));
            manualData.Add(new XYData(1234.4723, 13.468660632135));
            manualData.Add(new XYData(1234.4724, 13.2993179156037));
            manualData.Add(new XYData(1234.4725, 13.1304134661719));
            manualData.Add(new XYData(1234.4726, 12.9619684326066));
            manualData.Add(new XYData(1234.4727, 12.7940037577747));
            manualData.Add(new XYData(1234.4728, 12.626540174869));
            manualData.Add(new XYData(1234.4729, 12.4595982037037));
            manualData.Add(new XYData(1234.473, 12.2931981466995));
            manualData.Add(new XYData(1234.4731, 12.1273600868296));
            manualData.Add(new XYData(1234.4732, 11.9621038818525));
            manualData.Add(new XYData(1234.4733, 11.797449162402));
            manualData.Add(new XYData(1234.4734, 11.6334153282545));
            manualData.Add(new XYData(1234.4735, 11.4700215450484));
            manualData.Add(new XYData(1234.4736, 11.3072867410768));
            manualData.Add(new XYData(1234.4737, 11.1452296041532));
            manualData.Add(new XYData(1234.4738, 10.9838685785534));
            manualData.Add(new XYData(1234.4739, 10.8232218616662));
            manualData.Add(new XYData(1234.474, 10.6633074025453));
            manualData.Add(new XYData(1234.4741, 10.5041428968854));
            manualData.Add(new XYData(1234.4742, 10.3457457857297));
            manualData.Add(new XYData(1234.4743, 10.188133252426));
            manualData.Add(new XYData(1234.4744, 10.0313222200267));
            manualData.Add(new XYData(1234.4745, 9.87532934876691));
            manualData.Add(new XYData(1234.4746, 9.720171033621));
            manualData.Add(new XYData(1234.4747, 9.56586340193868));
            manualData.Add(new XYData(1234.4748, 9.41242231081228));
            manualData.Add(new XYData(1234.4749, 9.25986334626537));
            manualData.Add(new XYData(1234.475, 9.10820181903458));
            manualData.Add(new XYData(1234.4751, 8.95745276392269));
            manualData.Add(new XYData(1234.4752, 8.80763093748504));
            manualData.Add(new XYData(1234.4753, 8.65875081614675));
            manualData.Add(new XYData(1234.4754, 8.51082659440103));
            manualData.Add(new XYData(1234.4755, 8.36387218308899));
            manualData.Add(new XYData(1234.4756, 8.21790120776083));
            manualData.Add(new XYData(1234.4757, 8.07292700679038));
            manualData.Add(new XYData(1234.4758, 7.92896263121611));
            manualData.Add(new XYData(1234.4759, 7.78602084136995));
            manualData.Add(new XYData(1234.476, 7.64411410688683));
            manualData.Add(new XYData(1234.4761, 7.50325460514458));
            manualData.Add(new XYData(1234.4762, 7.3634542201167));
            manualData.Add(new XYData(1234.4763, 7.22472454130714));
            manualData.Add(new XYData(1234.4764, 7.08707686276712));
            manualData.Add(new XYData(1234.4765, 6.95052218219396));
            manualData.Add(new XYData(1234.4766, 6.81507119980505));
            manualData.Add(new XYData(1234.4767, 6.68073431882969));
            manualData.Add(new XYData(1234.4768, 6.54752164300539));
            manualData.Add(new XYData(1234.4769, 6.41544297723795));
            manualData.Add(new XYData(1234.477, 6.28450782679925));
            manualData.Add(new XYData(1234.4771, 6.15472539691535));
            manualData.Add(new XYData(1234.4772, 6.02610459243556));
            manualData.Add(new XYData(1234.4773, 5.89865401758214));
            manualData.Add(new XYData(1234.4774, 5.77238197578047));
            manualData.Add(new XYData(1234.4775, 5.64729646928611));
            manualData.Add(new XYData(1234.4776, 5.52340520030979));
            manualData.Add(new XYData(1234.4777, 5.40071556938034));
            manualData.Add(new XYData(1234.4778, 5.27923467663354));
            manualData.Add(new XYData(1234.4779, 5.15896932175351));
            manualData.Add(new XYData(1234.478, 5.03992600427821));
            manualData.Add(new XYData(1234.4781, 4.92211092398264));
            manualData.Add(new XYData(1234.4782, 4.80552998133961));
            manualData.Add(new XYData(1234.4783, 4.69018877805722));
            manualData.Add(new XYData(1234.4784, 4.57609261743485));
            manualData.Add(new XYData(1234.4785, 4.46324650608791));
            manualData.Add(new XYData(1234.4786, 4.35165515315641));
            manualData.Add(new XYData(1234.4787, 4.24132297218551));
            manualData.Add(new XYData(1234.4788, 4.13225408177881));
            manualData.Add(new XYData(1234.4789, 4.02445230658605));
            manualData.Add(new XYData(1234.479, 3.91792117836387));
            manualData.Add(new XYData(1234.4791, 3.81266393710871));
            manualData.Add(new XYData(1234.4792, 3.70868353226153));
            manualData.Add(new XYData(1234.4793, 3.60598262375132));
            manualData.Add(new XYData(1234.4794, 3.50456358427209));
            manualData.Add(new XYData(1234.4795, 3.40442849929715));
            manualData.Add(new XYData(1234.4796, 3.30557916950029));
            manualData.Add(new XYData(1234.4797, 3.20801711207294));
            manualData.Add(new XYData(1234.4798, 3.11174356234366));
            manualData.Add(new XYData(1234.4799, 3.01675947546402));
            manualData.Add(new XYData(1234.48, 2.92306552816008));
            manualData.Add(new XYData(1234.4801, 2.83066212054878));
            manualData.Add(new XYData(1234.4802, 2.73954937781252));
            manualData.Add(new XYData(1234.4803, 2.64972715296824));
            manualData.Add(new XYData(1234.4804, 2.56119502763006));
            manualData.Add(new XYData(1234.4805, 2.47395231490777));
            manualData.Add(new XYData(1234.4806, 2.38799806132552));
            manualData.Add(new XYData(1234.4807, 2.30333104900867));
            manualData.Add(new XYData(1234.4808, 2.21994979792908));
            manualData.Add(new XYData(1234.4809, 2.13785256820808));
            manualData.Add(new XYData(1234.481, 2.05703736247599));
            manualData.Add(new XYData(1234.4811, 1.97750192810797));
            manualData.Add(new XYData(1234.4812, 1.89924376041425));
            manualData.Add(new XYData(1234.4813, 1.82226010407857));
            manualData.Add(new XYData(1234.4814, 1.74654795646051));
            manualData.Add(new XYData(1234.4815, 1.67210407004087));
            manualData.Add(new XYData(1234.4816, 1.59892495509981));
            manualData.Add(new XYData(1234.4817, 1.52700688244454));
            manualData.Add(new XYData(1234.4818, 1.4563458861854));
            manualData.Add(new XYData(1234.4819, 1.38693776655926));
            manualData.Add(new XYData(1234.482, 1.31877809264562));
            manualData.Add(new XYData(1234.4821, 1.25186220589849));
            manualData.Add(new XYData(1234.4822, 1.18618522217543));
            manualData.Add(new XYData(1234.4823, 1.12174203536207));
            manualData.Add(new XYData(1234.4824, 1.0585273202593));
            manualData.Add(new XYData(1234.4825, 0.996535535667619));
            manualData.Add(new XYData(1234.4826, 0.935760927510894));
            manualData.Add(new XYData(1234.4827, 0.87619753199864));
            manualData.Add(new XYData(1234.4828, 0.817839178825474));
            manualData.Add(new XYData(1234.4829, 0.760679494406665));
            manualData.Add(new XYData(1234.483, 0.704711905022713));
            manualData.Add(new XYData(1234.4831, 0.649929640629884));
            manualData.Add(new XYData(1234.4832, 0.596325737433493));
            manualData.Add(new XYData(1234.4833, 0.543893041767814));
            manualData.Add(new XYData(1234.4834, 0.492624213365877));
            manualData.Add(new XYData(1234.4835, 0.442511728786886));
            manualData.Add(new XYData(1234.4836, 0.393547884871483));
            manualData.Add(new XYData(1234.4837, 0.345724802223614));
            manualData.Add(new XYData(1234.4838, 0.299034428717858));
            manualData.Add(new XYData(1234.4839, 0.253468542928651));
            manualData.Add(new XYData(1234.484, 0.209018758096734));
            manualData.Add(new XYData(1234.4841, 0.165676525083963));
            manualData.Add(new XYData(1234.4842, 0.123433136387312));
            manualData.Add(new XYData(1234.4843, 0.0822797296504515));
            manualData.Add(new XYData(1234.4844, 0.0422072912984368));
            manualData.Add(new XYData(1234.4845, 0.00320666018937892));
            manualData.Add(new XYData(1234.4846, -0.0347314687180537));
            manualData.Add(new XYData(1234.4847, -0.0716165406825452));
            manualData.Add(new XYData(1234.4848, -0.107458137564958));
            manualData.Add(new XYData(1234.4849, -0.142265973797037));
            manualData.Add(new XYData(1234.485, -0.176049893151662));
            manualData.Add(new XYData(1234.4851, -0.208819864686025));
            manualData.Add(new XYData(1234.4852, -0.240585979087206));
            manualData.Add(new XYData(1234.4853, -0.271358444927647));
            manualData.Add(new XYData(1234.4854, -0.301147584914387));
            manualData.Add(new XYData(1234.4855, -0.329963832133255));
            manualData.Add(new XYData(1234.4856, -0.357817726289172));
            manualData.Add(new XYData(1234.4857, -0.384719910003855));
            manualData.Add(new XYData(1234.4858, -0.410681124809371));
            manualData.Add(new XYData(1234.4859, -0.435712207750807));
            manualData.Add(new XYData(1234.486, -0.459824087375935));
            manualData.Add(new XYData(1234.4861, -0.483027780035853));
            manualData.Add(new XYData(1234.4862, -0.505334386126456));
            manualData.Add(new XYData(1234.4863, -0.52675508633412));
            manualData.Add(new XYData(1234.4864, -0.547301137886713));
            manualData.Add(new XYData(1234.4865, -0.566983870811062));
            manualData.Add(new XYData(1234.4866, -0.585814684239848));
            manualData.Add(new XYData(1234.4867, -0.603805042516));
            manualData.Add(new XYData(1234.4868, -0.620966471733013));
            manualData.Add(new XYData(1234.4869, -0.637310555856111));
            manualData.Add(new XYData(1234.487, -0.652848933072285));
            manualData.Add(new XYData(1234.4871, -0.667593292109546));
            manualData.Add(new XYData(1234.4872, -0.681555368570223));
            manualData.Add(new XYData(1234.4873, -0.694746941279368));
            manualData.Add(new XYData(1234.4874, -0.707179828649297));
            manualData.Add(new XYData(1234.4875, -0.718865885087012));
            manualData.Add(new XYData(1234.4876, -0.729816997289488));
            manualData.Add(new XYData(1234.4877, -0.740045080822105));
            manualData.Add(new XYData(1234.4878, -0.74956207644989));
            manualData.Add(new XYData(1234.4879, -0.758379946625294));
            manualData.Add(new XYData(1234.488, -0.766510671970639));
            manualData.Add(new XYData(1234.4881, -0.773966247783632));
            manualData.Add(new XYData(1234.4882, -0.780758680566874));
            manualData.Add(new XYData(1234.4883, -0.786899984582307));
            manualData.Add(new XYData(1234.4884, -0.792402178443273));
            manualData.Add(new XYData(1234.4885, -0.797277281672954));
            manualData.Add(new XYData(1234.4886, -0.801537311414025));
            manualData.Add(new XYData(1234.4887, -0.805194279040329));
            manualData.Add(new XYData(1234.4888, -0.808260186862591));
            manualData.Add(new XYData(1234.4889, -0.810747024851095));
            manualData.Add(new XYData(1234.489, -0.812666767389383));
            manualData.Add(new XYData(1234.4891, -0.814031370059785));
            manualData.Add(new XYData(1234.4892, -0.81485276646158));
            manualData.Add(new XYData(1234.4893, -0.815142865062633));
            manualData.Add(new XYData(1234.4894, -0.814913546083716));
            manualData.Add(new XYData(1234.4895, -0.814176658423105));
            manualData.Add(new XYData(1234.4896, -0.812944016609093));
            manualData.Add(new XYData(1234.4897, -0.811227397793973));
            manualData.Add(new XYData(1234.4898, -0.809038538783828));
            manualData.Add(new XYData(1234.4899, -0.806389133106089));
            manualData.Add(new XYData(1234.49, -0.803290828115533));
            manualData.Add(new XYData(1234.4901, -0.799755222139355));
            manualData.Add(new XYData(1234.4902, -0.795793861652479));
            manualData.Add(new XYData(1234.4903, -0.791418238539692));
            manualData.Add(new XYData(1234.4904, -0.786639787307634));
            manualData.Add(new XYData(1234.4905, -0.781469882427463));
            manualData.Add(new XYData(1234.4906, -0.775919835676005));
            manualData.Add(new XYData(1234.4907, -0.770000893528055));
            manualData.Add(new XYData(1234.4908, -0.763724234591835));
            manualData.Add(new XYData(1234.4909, -0.757100967088135));
            manualData.Add(new XYData(1234.491, -0.750142126373616));
            manualData.Add(new XYData(1234.4911, -0.742858672491841));
            manualData.Add(new XYData(1234.4912, -0.735261487853315));
            manualData.Add(new XYData(1234.4913, -0.727361374794676));
            manualData.Add(new XYData(1234.4914, -0.719169053348807));
            manualData.Add(new XYData(1234.4915, -0.710695158979564));
            manualData.Add(new XYData(1234.4916, -0.701950240379479));
            manualData.Add(new XYData(1234.4917, -0.692944757314577));
            manualData.Add(new XYData(1234.4918, -0.683689078516671));
            manualData.Add(new XYData(1234.4919, -0.674193479623462));
            manualData.Add(new XYData(1234.492, -0.664468141144393));
            manualData.Add(new XYData(1234.4921, -0.654523146586264));
            manualData.Add(new XYData(1234.4922, -0.644368480405841));
            manualData.Add(new XYData(1234.4923, -0.634014026232086));
            manualData.Add(new XYData(1234.4924, -0.623469565027767));
            manualData.Add(new XYData(1234.4925, -0.612744773322326));
            manualData.Add(new XYData(1234.4926, -0.601849221494363));
            manualData.Add(new XYData(1234.4927, -0.590792372103922));
            manualData.Add(new XYData(1234.4928, -0.579583578274737));
            manualData.Add(new XYData(1234.4929, -0.568232082100628));
            manualData.Add(new XYData(1234.493, -0.556747013231676));
            manualData.Add(new XYData(1234.4931, -0.545137387252287));
            manualData.Add(new XYData(1234.4932, -0.533412104367556));
            manualData.Add(new XYData(1234.4933, -0.521579948011545));
            manualData.Add(new XYData(1234.4934, -0.509649583531815));
            manualData.Add(new XYData(1234.4935, -0.497629556924579));
            manualData.Add(new XYData(1234.4936, -0.485528293620505));
            manualData.Add(new XYData(1234.4937, -0.473354097321175));
            manualData.Add(new XYData(1234.4938, -0.461115148858298));
            manualData.Add(new XYData(1234.4939, -0.448819505242881));
            manualData.Add(new XYData(1234.494, -0.436475098486531));
            manualData.Add(new XYData(1234.4941, -0.424089734751424));
            manualData.Add(new XYData(1234.4942, -0.411671093411769));
            manualData.Add(new XYData(1234.4943, -0.399226726193343));
            manualData.Add(new XYData(1234.4944, -0.386764056363245));
            manualData.Add(new XYData(1234.4945, -0.374290377969731));
            manualData.Add(new XYData(1234.4946, -0.361812855131975));
            manualData.Add(new XYData(1234.4947, -0.34933852135125));
            manualData.Add(new XYData(1234.4948, -0.336874279013499));
            manualData.Add(new XYData(1234.4949, -0.324426898657644));
            manualData.Add(new XYData(1234.495, -0.312003018576758));
            manualData.Add(new XYData(1234.4951, -0.299609144326345));
            manualData.Add(new XYData(1234.4952, -0.28725164830969));
            manualData.Add(new XYData(1234.4953, -0.274936769411664));
            manualData.Add(new XYData(1234.4954, -0.262670612680709));
            manualData.Add(new XYData(1234.4955, -0.250459149058697));
            manualData.Add(new XYData(1234.4956, -0.238308215130827));
            manualData.Add(new XYData(1234.4957, -0.22622351306072));
            manualData.Add(new XYData(1234.4958, -0.214210610296276));
            manualData.Add(new XYData(1234.4959, -0.202274939598568));
            manualData.Add(new XYData(1234.496, -0.190421798978601));
            manualData.Add(new XYData(1234.4961, -0.178656351707616));
            manualData.Add(new XYData(1234.4962, -0.166983626372859));
            manualData.Add(new XYData(1234.4963, -0.155408516978403));
            manualData.Add(new XYData(1234.4964, -0.143935783090622));
            manualData.Add(new XYData(1234.4965, -0.132570050002158));
            manualData.Add(new XYData(1234.4966, -0.12131580906852));
            manualData.Add(new XYData(1234.4967, -0.110177417829798));
            manualData.Add(new XYData(1234.4968, -0.0991591004339419));
            manualData.Add(new XYData(1234.4969, -0.0882649479728283));
            manualData.Add(new XYData(1234.497, -0.0774989188861862));
            manualData.Add(new XYData(1234.4971, -0.0668648394069243));
            manualData.Add(new XYData(1234.4972, -0.0563664040473338));
            manualData.Add(new XYData(1234.4973, -0.0460071761256607));
            manualData.Add(new XYData(1234.4974, -0.0357905883094391));
            manualData.Add(new XYData(1234.4975, -0.0257199433137758));
            manualData.Add(new XYData(1234.4976, -0.0157984144063774));
            manualData.Add(new XYData(1234.4977, -0.00602904618339678));
            manualData.Add(new XYData(1234.4978, 0.00358524473377092));
            manualData.Add(new XYData(1234.4979, 0.0130416689412812));
            manualData.Add(new XYData(1234.498, 0.0223375634592075));
            manualData.Add(new XYData(1234.4981, 0.0314703909020824));
            manualData.Add(new XYData(1234.4982, 0.0404377386142208));
            manualData.Add(new XYData(1234.4983, 0.0492373177902544));
            manualData.Add(new XYData(1234.4984, 0.0578669624622297));
            manualData.Add(new XYData(1234.4985, 0.0663246286530575));
            manualData.Add(new XYData(1234.4986, 0.074608393296789));
            manualData.Add(new XYData(1234.4987, 0.082716453227446));
            manualData.Add(new XYData(1234.4988, 0.0906471241162526));
            manualData.Add(new XYData(1234.4989, 0.098398839378114));
            manualData.Add(new XYData(1234.499, 0.105970149048019));
            manualData.Add(new XYData(1234.4991, 0.113359718628031));
            manualData.Add(new XYData(1234.4992, 0.120566327905558));
            manualData.Add(new XYData(1234.4993, 0.12758886975932));
            manualData.Add(new XYData(1234.4994, 0.134426348858756));
            manualData.Add(new XYData(1234.4995, 0.14107788049603));
            manualData.Add(new XYData(1234.4996, 0.147542689231957));
            manualData.Add(new XYData(1234.4997, 0.153820107597146));
            manualData.Add(new XYData(1234.4998, 0.159909574752389));
            manualData.Add(new XYData(1234.4999, 0.165810635125217));
            manualData.Add(new XYData(1234.5, 0.1715229370233));
            manualData.Add(new XYData(1234.5001, 0.177046231225443));
            manualData.Add(new XYData(1234.5002, 0.182380369562773));
            manualData.Add(new XYData(1234.5003, 0.187525303418963));
            manualData.Add(new XYData(1234.5004, 0.192481082331042));
            manualData.Add(new XYData(1234.5005, 0.197247852448959));
            manualData.Add(new XYData(1234.5006, 0.201825855037626));
            manualData.Add(new XYData(1234.5007, 0.206215424948547));
            manualData.Add(new XYData(1234.5008, 0.210416989074087));
            manualData.Add(new XYData(1234.5009, 0.214431064785127));
            manualData.Add(new XYData(1234.501, 0.218258258352838));
            manualData.Add(new XYData(1234.5011, 0.221899263363354));
            manualData.Add(new XYData(1234.5012, 0.225354859077267));
            manualData.Add(new XYData(1234.5013, 0.228625908857787));
            manualData.Add(new XYData(1234.5014, 0.231713358502732));
            manualData.Add(new XYData(1234.5015, 0.234618234606301));
            manualData.Add(new XYData(1234.5016, 0.23734164290086));
            manualData.Add(new XYData(1234.5017, 0.239884766587938));
            manualData.Add(new XYData(1234.5018, 0.24224886465917));
            manualData.Add(new XYData(1234.5019, 0.244435270207904));
            manualData.Add(new XYData(1234.502, 0.246445388736577));
            manualData.Add(new XYData(1234.5021, 0.248280696433925));
            manualData.Add(new XYData(1234.5022, 0.249942738490339));
            manualData.Add(new XYData(1234.5023, 0.251433127360633));
            manualData.Add(new XYData(1234.5024, 0.252753541044524));
            manualData.Add(new XYData(1234.5025, 0.253905721357287));
            manualData.Add(new XYData(1234.5026, 0.254891472196055));
            manualData.Add(new XYData(1234.5027, 0.255712657802482));
            manualData.Add(new XYData(1234.5028, 0.256371201022438));
            manualData.Add(new XYData(1234.5029, 0.256869081564406));
            manualData.Add(new XYData(1234.503, 0.257208334251168));
            manualData.Add(new XYData(1234.5031, 0.257391047281554));
            manualData.Add(new XYData(1234.5032, 0.257419360480286));
            manualData.Add(new XYData(1234.5033, 0.257295463554469));
            manualData.Add(new XYData(1234.5034, 0.257021594349883));
            manualData.Add(new XYData(1234.5035, 0.256600037109035));
            manualData.Add(new XYData(1234.5036, 0.256033120731635));
            manualData.Add(new XYData(1234.5037, 0.255323217038149));
            manualData.Add(new XYData(1234.5038, 0.254472739034944));
            manualData.Add(new XYData(1234.5039, 0.253484139193946));
            manualData.Add(new XYData(1234.504, 0.252359907717578));
            manualData.Add(new XYData(1234.5041, 0.251102570828325));
            manualData.Add(new XYData(1234.5042, 0.249714689055304));
            manualData.Add(new XYData(1234.5043, 0.248198855529395));
            manualData.Add(new XYData(1234.5044, 0.246557694285796));
            manualData.Add(new XYData(1234.5045, 0.244793858574548));
            manualData.Add(new XYData(1234.5046, 0.242910029179637));
            manualData.Add(new XYData(1234.5047, 0.24090891274256));
            manualData.Add(new XYData(1234.5048, 0.238793240118749));
            manualData.Add(new XYData(1234.5049, 0.236565764698489));
            manualData.Add(new XYData(1234.505, 0.234229260783886));
            manualData.Add(new XYData(1234.5051, 0.23178652195485));
            manualData.Add(new XYData(1234.5052, 0.229240359451363));
            manualData.Add(new XYData(1234.5053, 0.226593600568166));
            manualData.Add(new XYData(1234.5054, 0.223849087062355));
            manualData.Add(new XYData(1234.5055, 0.221009673574407));
            manualData.Add(new XYData(1234.5056, 0.218078226056344));
            manualData.Add(new XYData(1234.5057, 0.215057620247945));
            manualData.Add(new XYData(1234.5058, 0.211950740100952));
            manualData.Add(new XYData(1234.5059, 0.208760476285075));
            manualData.Add(new XYData(1234.506, 0.205489724676774));
            manualData.Add(new XYData(1234.5061, 0.202141384870837));
            manualData.Add(new XYData(1234.5062, 0.198718358708628));
            manualData.Add(new XYData(1234.5063, 0.195223548823435));
            manualData.Add(new XYData(1234.5064, 0.191659857203323));
            manualData.Add(new XYData(1234.5065, 0.188030183763584));
            manualData.Add(new XYData(1234.5066, 0.184337424978974));
            manualData.Add(new XYData(1234.5067, 0.180584472451947));
            manualData.Add(new XYData(1234.5068, 0.176774211582362));
            manualData.Add(new XYData(1234.5069, 0.172909520215545));
            manualData.Add(new XYData(1234.507, 0.168993267318313));
            manualData.Add(new XYData(1234.5071, 0.165028311675113));
            manualData.Add(new XYData(1234.5072, 0.161017500604591));
            manualData.Add(new XYData(1234.5073, 0.156963668696925));
            manualData.Add(new XYData(1234.5074, 0.152869636562877));
            manualData.Add(new XYData(1234.5075, 0.148738209650867));
            manualData.Add(new XYData(1234.5076, 0.144572176992526));
            manualData.Add(new XYData(1234.5077, 0.140374310063158));
            manualData.Add(new XYData(1234.5078, 0.136147361617984));
            manualData.Add(new XYData(1234.5079, 0.131894064560071));
            manualData.Add(new XYData(1234.508, 0.127617130830936));
            manualData.Add(new XYData(1234.5081, 0.123319250324051));
            manualData.Add(new XYData(1234.5082, 0.119003089821462));
            manualData.Add(new XYData(1234.5083, 0.11467129194387));
            manualData.Add(new XYData(1234.5084, 0.110326474173529));
            manualData.Add(new XYData(1234.5085, 0.105971227802302));
            manualData.Add(new XYData(1234.5086, 0.101608117002076));
            manualData.Add(new XYData(1234.5087, 0.0972396778699985));
            manualData.Add(new XYData(1234.5088, 0.0928684175077478));
            manualData.Add(new XYData(1234.5089, 0.0884968131251171));
            manualData.Add(new XYData(1234.509, 0.0841273111680575));
            manualData.Add(new XYData(1234.5091, 0.0797623264712902));
            manualData.Add(new XYData(1234.5092, 0.0754042414256954));
            manualData.Add(new XYData(1234.5093, 0.0710554052199922));
            manualData.Add(new XYData(1234.5094, 0.0667181330081911));
            manualData.Add(new XYData(1234.5095, 0.0623947052010937));
            manualData.Add(new XYData(1234.5096, 0.0580873667332033));
            manualData.Add(new XYData(1234.5097, 0.0537983263645977));
            manualData.Add(new XYData(1234.5098, 0.0495297560078812));
            manualData.Add(new XYData(1234.5099, 0.0452837900802471));
            manualData.Add(new XYData(1234.51, 0.0410625248806695));
            manualData.Add(new XYData(1234.5101, 0.0368680179827287));
            manualData.Add(new XYData(1234.5102, 0.0327022877001671));
            manualData.Add(new XYData(1234.5103, 0.0285673124822485));
            manualData.Add(new XYData(1234.5104, 0.0244650304296644));
            manualData.Add(new XYData(1234.5105, 0.0203973387877254));
            manualData.Add(new XYData(1234.5106, 0.0163660934741442));
            manualData.Add(new XYData(1234.5107, 0.0123731086317937));
            manualData.Add(new XYData(1234.5108, 0.00842015620636872));
            manualData.Add(new XYData(1234.5109, 0.00450896554888782));
            manualData.Add(new XYData(1234.511, 0.000641223034202634));
            manualData.Add(new XYData(1234.5111, -0.00318142825201069));
            manualData.Add(new XYData(1234.5112, -0.00695738888954818));
            manualData.Add(new XYData(1234.5113, -0.0106851033874803));
            manualData.Add(new XYData(1234.5114, -0.0143630604677485));
            manualData.Add(new XYData(1234.5115, -0.0179897933157216));
            manualData.Add(new XYData(1234.5116, -0.0215638798066944));
            manualData.Add(new XYData(1234.5117, -0.0250839427084752));
            manualData.Add(new XYData(1234.5118, -0.0285486498602199));
            manualData.Add(new XYData(1234.5119, -0.0319567143353597));
            manualData.Add(new XYData(1234.512, -0.0353068945425659));
            manualData.Add(new XYData(1234.5121, -0.038597994380844));
            manualData.Add(new XYData(1234.5122, -0.0418288632939613));
            manualData.Add(new XYData(1234.5123, -0.0449983963409719));
            manualData.Add(new XYData(1234.5124, -0.0481055342362905));
            manualData.Add(new XYData(1234.5125, -0.051149263367344));
            manualData.Add(new XYData(1234.5126, -0.0541286157900205));
            manualData.Add(new XYData(1234.5127, -0.0570426692021477));
            manualData.Add(new XYData(1234.5128, -0.0598905469016365));
            manualData.Add(new XYData(1234.5129, -0.0626714176909808));
            manualData.Add(new XYData(1234.513, -0.0653844958251278));
            manualData.Add(new XYData(1234.5131, -0.0680290408734536));
            manualData.Add(new XYData(1234.5132, -0.0706043575936303));
            manualData.Add(new XYData(1234.5133, -0.0731097957784943));
            manualData.Add(new XYData(1234.5134, -0.0755447500827491));
            manualData.Add(new XYData(1234.5135, -0.0779086598297792));
            manualData.Add(new XYData(1234.5136, -0.0802010087988843));
            manualData.Add(new XYData(1234.5137, -0.08242132499815));
            manualData.Add(new XYData(1234.5138, -0.0845691803933747));
            manualData.Add(new XYData(1234.5139, -0.0866441906685797));
            manualData.Add(new XYData(1234.514, -0.0886460149175304));
            manualData.Add(new XYData(1234.5141, -0.0905743553426518));
            manualData.Add(new XYData(1234.5142, -0.0924289569311271));
            manualData.Add(new XYData(1234.5143, -0.094209607113524));
            manualData.Add(new XYData(1234.5144, -0.0959161354054147));
            manualData.Add(new XYData(1234.5145, -0.0975484130321871));
            manualData.Add(new XYData(1234.5146, -0.099106352537527));
            manualData.Add(new XYData(1234.5147, -0.100589907379069));
            manualData.Add(new XYData(1234.5148, -0.101999071491797));
            manualData.Add(new XYData(1234.5149, -0.103333878869595));
            manualData.Add(new XYData(1234.515, -0.10459440309796));
            manualData.Add(new XYData(1234.5151, -0.105780756889152));
            manualData.Add(new XYData(1234.5152, -0.10689309159966));
            manualData.Add(new XYData(1234.5153, -0.107931596733724));
            manualData.Add(new XYData(1234.5154, -0.108896499433354));
            manualData.Add(new XYData(1234.5155, -0.10978806395517));
            manualData.Add(new XYData(1234.5156, -0.110606591136236));
            manualData.Add(new XYData(1234.5157, -0.111352417838427));
            manualData.Add(new XYData(1234.5158, -0.11202591639921));
            manualData.Add(new XYData(1234.5159, -0.11262749405199));
            manualData.Add(new XYData(1234.516, -0.113157592344753));
            manualData.Add(new XYData(1234.5161, -0.113616686545873));
            manualData.Add(new XYData(1234.5162, -0.11400528503942));
            manualData.Add(new XYData(1234.5163, -0.114323928710364));
            manualData.Add(new XYData(1234.5164, -0.114573190320046));
            manualData.Add(new XYData(1234.5165, -0.11475367387266));
            manualData.Add(new XYData(1234.5166, -0.114866013970957));
            manualData.Add(new XYData(1234.5167, -0.114910875167355));
            manualData.Add(new XYData(1234.5168, -0.114888951302621));
            manualData.Add(new XYData(1234.5169, -0.11480096483907));
            manualData.Add(new XYData(1234.517, -0.114647666185885));
            manualData.Add(new XYData(1234.5171, -0.114429833017461));
            manualData.Add(new XYData(1234.5172, -0.114148269585134));
            manualData.Add(new XYData(1234.5173, -0.11380380602271));
            manualData.Add(new XYData(1234.5174, -0.113397297645172));
            manualData.Add(new XYData(1234.5175, -0.112929624246793));
            manualData.Add(new XYData(1234.5176, -0.112401689384814));
            manualData.Add(new XYData(1234.5177, -0.11181441966757));
            manualData.Add(new XYData(1234.5178, -0.111168764033931));
            manualData.Add(new XYData(1234.5179, -0.11046569302971));
            manualData.Add(new XYData(1234.518, -0.109706198080561));
            manualData.Add(new XYData(1234.5181, -0.108891290761745));
            manualData.Add(new XYData(1234.5182, -0.108022002065161));
            manualData.Add(new XYData(1234.5183, -0.10709938166183));
            manualData.Add(new XYData(1234.5184, -0.106124497173065));
            manualData.Add(new XYData(1234.5185, -0.105098433418897));
            manualData.Add(new XYData(1234.5186, -0.104022291686123));
            manualData.Add(new XYData(1234.5187, -0.102897188985138));
            manualData.Add(new XYData(1234.5188, -0.101724257308283));
            manualData.Add(new XYData(1234.5189, -0.100504642888028));
            manualData.Add(new XYData(1234.519, -0.0992395054553406));
            manualData.Add(new XYData(1234.5191, -0.0979300174986061));
            manualData.Add(new XYData(1234.5192, -0.0965773635203137));
            manualData.Add(new XYData(1234.5193, -0.0951827393105169));
            manualData.Add(new XYData(1234.5194, -0.0937473511910193));
            manualData.Add(new XYData(1234.5195, -0.0922724152921572));
            manualData.Add(new XYData(1234.5196, -0.0907591568166079));
            manualData.Add(new XYData(1234.5197, -0.0892088093088271));
            manualData.Add(new XYData(1234.5198, -0.0876226139274042));
            manualData.Add(new XYData(1234.5199, -0.0860018187206832));
            manualData.Add(new XYData(1234.52, -0.0843476779059229));
            manualData.Add(new XYData(1234.5201, -0.0826614511485128));
            manualData.Add(new XYData(1234.5202, -0.0809444028646206));
            manualData.Add(new XYData(1234.5203, -0.079197801489963));
            manualData.Add(new XYData(1234.5204, -0.0774229187916149));
            manualData.Add(new XYData(1234.5205, -0.0756210291659002));
            manualData.Add(new XYData(1234.5206, -0.0737934089454165));
            manualData.Add(new XYData(1234.5207, -0.0719413357117978));
            manualData.Add(new XYData(1234.5208, -0.070066087614375));
            manualData.Add(new XYData(1234.5209, -0.0681689426951341));
            manualData.Add(new XYData(1234.521, -0.0662511782157387));
            manualData.Add(new XYData(1234.5211, -0.0643140700132492));
            manualData.Add(new XYData(1234.5212, -0.0623588918191691));
            manualData.Add(new XYData(1234.5213, -0.0603869146293992));
            manualData.Add(new XYData(1234.5214, -0.0583994060599274));
            manualData.Add(new XYData(1234.5215, -0.0563976297147001));
            manualData.Add(new XYData(1234.5216, -0.0543828445614602));
            manualData.Add(new XYData(1234.5217, -0.0523563043158611));
            manualData.Add(new XYData(1234.5218, -0.050319256834047));
            manualData.Add(new XYData(1234.5219, -0.0482729435092669));
            manualData.Add(new XYData(1234.522, -0.0462185987006917));
            manualData.Add(new XYData(1234.5221, -0.0441574491247959));
            manualData.Add(new XYData(1234.5222, -0.0420907133025715));
            manualData.Add(new XYData(1234.5223, -0.0400196009930204));
            manualData.Add(new XYData(1234.5224, -0.0379453126409956));
            manualData.Add(new XYData(1234.5225, -0.035869038834928));
            manualData.Add(new XYData(1234.5226, -0.0337919597746216));
            manualData.Add(new XYData(1234.5227, -0.0317152447492743));
            manualData.Add(new XYData(1234.5228, -0.029640051621213));
            manualData.Add(new XYData(1234.5229, -0.0275675263437257));
            manualData.Add(new XYData(1234.523, -0.0254988024424694));
            manualData.Add(new XYData(1234.5231, -0.023435000554897));
            manualData.Add(new XYData(1234.5232, -0.0213772279571021));
            manualData.Add(new XYData(1234.5233, -0.0193265781064775));
            manualData.Add(new XYData(1234.5234, -0.0172841301956916));
            manualData.Add(new XYData(1234.5235, -0.0152509487179582));
            manualData.Add(new XYData(1234.5236, -0.0132280830438852));
            manualData.Add(new XYData(1234.5237, -0.0112165670052686));
            manualData.Add(new XYData(1234.5238, -0.0092174185135046));
            manualData.Add(new XYData(1234.5239, -0.00723163914398588));
            manualData.Add(new XYData(1234.524, -0.0052602137782388));
            manualData.Add(new XYData(1234.5241, -0.00330411023501868));
            manualData.Add(new XYData(1234.5242, -0.00136427891812036));
            manualData.Add(new XYData(1234.5243, 0.000558347523797482));
            manualData.Add(new XYData(1234.5244, 0.00246285452513816));
            manualData.Add(new XYData(1234.5245, 0.00434834591913171));
            manualData.Add(new XYData(1234.5246, 0.00621394424551108));
            manualData.Add(new XYData(1234.5247, 0.00805879102475049));
            manualData.Add(new XYData(1234.5248, 0.00988204706230368));
            manualData.Add(new XYData(1234.5249, 0.0116828926980186));
            manualData.Add(new XYData(1234.525, 0.0134605280644016));
            manualData.Add(new XYData(1234.5251, 0.015214173328197));
            manualData.Add(new XYData(1234.5252, 0.0169430689195273));
            manualData.Add(new XYData(1234.5253, 0.0186464757485869));
            manualData.Add(new XYData(1234.5254, 0.0203236754098898));
            manualData.Add(new XYData(1234.5255, 0.0219739703777984));
            manualData.Add(new XYData(1234.5256, 0.0235966841709545));
            manualData.Add(new XYData(1234.5257, 0.0251911615417638));
            manualData.Add(new XYData(1234.5258, 0.026756768615905));
            manualData.Add(new XYData(1234.5259, 0.0282928930383357));
            manualData.Add(new XYData(1234.526, 0.0297989441031924));
            manualData.Add(new XYData(1234.5261, 0.0312743528714089));
            manualData.Add(new XYData(1234.5262, 0.0327185722760971));
            manualData.Add(new XYData(1234.5263, 0.0341310772157463));
            manualData.Add(new XYData(1234.5264, 0.0355113646383925));
            manualData.Add(new XYData(1234.5265, 0.0368589535981488));
            manualData.Add(new XYData(1234.5266, 0.0381733853310445));
            manualData.Add(new XYData(1234.5267, 0.0394542232874873));
            manualData.Add(new XYData(1234.5268, 0.0407010531686828));
            manualData.Add(new XYData(1234.5269, 0.0419134829481161));
            manualData.Add(new XYData(1234.527, 0.0430911428813675));
            manualData.Add(new XYData(1234.5271, 0.0442336855043435));
            manualData.Add(new XYData(1234.5272, 0.0453407856200334));
            manualData.Add(new XYData(1234.5273, 0.0464121402762833));
            manualData.Add(new XYData(1234.5274, 0.0474474687202418));
            manualData.Add(new XYData(1234.5275, 0.0484465123659353));
            manualData.Add(new XYData(1234.5276, 0.0494090347263568));
            manualData.Add(new XYData(1234.5277, 0.0503348213469448));
            manualData.Add(new XYData(1234.5278, 0.0512236797257921));
            manualData.Add(new XYData(1234.5279, 0.0520754392231973));
            manualData.Add(new XYData(1234.528, 0.0528899509606889));
            manualData.Add(new XYData(1234.5281, 0.0536670877096607));
            manualData.Add(new XYData(1234.5282, 0.0544067437714058));
            manualData.Add(new XYData(1234.5283, 0.0551088348387656));
            manualData.Add(new XYData(1234.5284, 0.0557732978645698));
            manualData.Add(new XYData(1234.5285, 0.0564000909033719));
            manualData.Add(new XYData(1234.5286, 0.0569891929520888));
            manualData.Add(new XYData(1234.5287, 0.0575406037794396));
            manualData.Add(new XYData(1234.5288, 0.0580543437460715));
            manualData.Add(new XYData(1234.5289, 0.0585304536155393));
            manualData.Add(new XYData(1234.529, 0.0589689943563165));
            manualData.Add(new XYData(1234.5291, 0.0593700469358702));
            manualData.Add(new XYData(1234.5292, 0.059733712101685));
            manualData.Add(new XYData(1234.5293, 0.0600601101628735));
            manualData.Add(new XYData(1234.5294, 0.0603493807543242));
            manualData.Add(new XYData(1234.5295, 0.0606016825974583));
            manualData.Add(new XYData(1234.5296, 0.0608171932521443));
            manualData.Add(new XYData(1234.5297, 0.0609961088609119));
            manualData.Add(new XYData(1234.5298, 0.0611386438856595));
            manualData.Add(new XYData(1234.5299, 0.0612450308370482));
            manualData.Add(new XYData(1234.53, 0.0613155199969026));
            manualData.Add(new XYData(1234.5301, 0.0613503791330082));
            manualData.Add(new XYData(1234.5302, 0.0613498932087181));
            manualData.Add(new XYData(1234.5303, 0.0613143640843575));
            manualData.Add(new XYData(1234.5304, 0.0612441102132402));
            manualData.Add(new XYData(1234.5305, 0.061139466331377));
            manualData.Add(new XYData(1234.5306, 0.0610007831412874));
            manualData.Add(new XYData(1234.5307, 0.0608284269901137));
            manualData.Add(new XYData(1234.5308, 0.060622779542257));
            manualData.Add(new XYData(1234.5309, 0.0603842374467391));
            manualData.Add(new XYData(1234.531, 0.06011321199885));
            manualData.Add(new XYData(1234.5311, 0.059810128800138));
            manualData.Add(new XYData(1234.5312, 0.0594754274075278));
            manualData.Add(new XYData(1234.5313, 0.0591095609841036));
            manualData.Add(new XYData(1234.5314, 0.0587129959427145));
            manualData.Add(new XYData(1234.5315, 0.0582862115861654));
            manualData.Add(new XYData(1234.5316, 0.0578296997436272));
            manualData.Add(new XYData(1234.5317, 0.0573439644034852));
            manualData.Add(new XYData(1234.5318, 0.0568295213428368));
            manualData.Add(new XYData(1234.5319, 0.0562868977525922));
            manualData.Add(new XYData(1234.532, 0.0557166318659197));
            manualData.Add(new XYData(1234.5321, 0.0551192725715873));
            manualData.Add(new XYData(1234.5322, 0.0544953790370871));
            manualData.Add(new XYData(1234.5323, 0.0538455203234177));
            manualData.Add(new XYData(1234.5324, 0.0531702749990031));
            manualData.Add(new XYData(1234.5325, 0.0524702307517659));
            manualData.Add(new XYData(1234.5326, 0.0517459839995684));
            manualData.Add(new XYData(1234.5327, 0.0509981394992264));
            manualData.Add(new XYData(1234.5328, 0.0502273099525347));
            manualData.Add(new XYData(1234.5329, 0.0494341156201324));
            manualData.Add(new XYData(1234.533, 0.0486191839169925));
            manualData.Add(new XYData(1234.5331, 0.0477831490248037));
            manualData.Add(new XYData(1234.5332, 0.0469266514952863));
            manualData.Add(new XYData(1234.5333, 0.0460503378550587));
            manualData.Add(new XYData(1234.5334, 0.0451548602105342));
            manualData.Add(new XYData(1234.5335, 0.044240875853048));
            manualData.Add(new XYData(1234.5336, 0.0433090468644155));
            manualData.Add(new XYData(1234.5337, 0.0423600397209409));
            manualData.Add(new XYData(1234.5338, 0.041394524909104));
            manualData.Add(new XYData(1234.5339, 0.0404131765206448));
            manualData.Add(new XYData(1234.534, 0.0394166718704076));
            manualData.Add(new XYData(1234.5341, 0.0384056911048557));
            manualData.Add(new XYData(1234.5342, 0.0373809168143206));
            manualData.Add(new XYData(1234.5343, 0.036343033647031));
            manualData.Add(new XYData(1234.5344, 0.0352927279251056));
            manualData.Add(new XYData(1234.5345, 0.0342306872626933));
            manualData.Add(new XYData(1234.5346, 0.0331576001839871));
            manualData.Add(new XYData(1234.5347, 0.0320741557559807));
            manualData.Add(new XYData(1234.5348, 0.0309810431994707));
            manualData.Add(new XYData(1234.5349, 0.0298789515272907));
            manualData.Add(new XYData(1234.535, 0.0287685691733989));
            manualData.Add(new XYData(1234.5351, 0.0276505836275775));
            manualData.Add(new XYData(1234.5352, 0.0265256810734798));
            manualData.Add(new XYData(1234.5353, 0.0253945460301882));
            manualData.Add(new XYData(1234.5354, 0.0242578609974365));
            manualData.Add(new XYData(1234.5355, 0.023116306102056));
            manualData.Add(new XYData(1234.5356, 0.0219705587613841));
            manualData.Add(new XYData(1234.5357, 0.0208212933248297));
            manualData.Add(new XYData(1234.5358, 0.0196691807456508));
            manualData.Add(new XYData(1234.5359, 0.0185148882441786));
            manualData.Add(new XYData(1234.536, 0.0173590789781808));
            manualData.Add(new XYData(1234.5361, 0.0162024117179055));
            manualData.Add(new XYData(1234.5362, 0.0150455405259429));
            manualData.Add(new XYData(1234.5363, 0.0138891144420298));
            manualData.Add(new XYData(1234.5364, 0.0127337771702996));
            manualData.Add(new XYData(1234.5365, 0.0115801667848532));
            manualData.Add(new XYData(1234.5366, 0.0104289154143717));
            manualData.Add(new XYData(1234.5367, 0.00928064895842882));
            manualData.Add(new XYData(1234.5368, 0.00813598679619919));
            manualData.Add(new XYData(1234.5369, 0.00699554150343912));
            manualData.Add(new XYData(1234.537, 0.00585991857521437));
            manualData.Add(new XYData(1234.5371, 0.00472971615447674));
            manualData.Add(new XYData(1234.5372, 0.00360552476658515));
            manualData.Add(new XYData(1234.5373, 0.00248792705733142));
            manualData.Add(new XYData(1234.5374, 0.0013774975497761));
            manualData.Add(new XYData(1234.5375, 0.000274802381898301));
            manualData.Add(new XYData(1234.5376, -0.000819600924030131));
            manualData.Add(new XYData(1234.5377, -0.00190516369842804));
            manualData.Add(new XYData(1234.5378, -0.00298134635239299));
            manualData.Add(new XYData(1234.5379, -0.00404761859917232));
            manualData.Add(new XYData(1234.538, -0.0051034596690508));
            manualData.Add(new XYData(1234.5381, -0.00614835851759156));
            manualData.Add(new XYData(1234.5382, -0.00718181402950892));
            manualData.Add(new XYData(1234.5383, -0.00820333520406981));
            manualData.Add(new XYData(1234.5384, -0.00921244135715916));
            manualData.Add(new XYData(1234.5385, -0.0102086622929571));
            manualData.Add(new XYData(1234.5386, -0.0111915384805044));
            manualData.Add(new XYData(1234.5387, -0.0121606212209616));
            manualData.Add(new XYData(1234.5388, -0.0131154728078894));
            manualData.Add(new XYData(1234.5389, -0.0140556666805178));
            manualData.Add(new XYData(1234.539, -0.0149807875699779));
            manualData.Add(new XYData(1234.5391, -0.0158904316405171));
            manualData.Add(new XYData(1234.5392, -0.0167842066133435));
            manualData.Add(new XYData(1234.5393, -0.0176617319040153));
            manualData.Add(new XYData(1234.5394, -0.0185226387319972));
            manualData.Add(new XYData(1234.5395, -0.0193665702334846));
            manualData.Add(new XYData(1234.5396, -0.0201931815650117));
            manualData.Add(new XYData(1234.5397, -0.0210021399999273));
            manualData.Add(new XYData(1234.5398, -0.0217931250177366));
            manualData.Add(new XYData(1234.5399, -0.0225658283863188));
            manualData.Add(new XYData(1234.54, -0.023319954238717));
            manualData.Add(new XYData(1234.5401, -0.024055219134314));
            manualData.Add(new XYData(1234.5402, -0.0247713521300223));
            manualData.Add(new XYData(1234.5403, -0.0254680948272128));
            manualData.Add(new XYData(1234.5404, -0.026145201420231));
            manualData.Add(new XYData(1234.5405, -0.0268024387361571));
            manualData.Add(new XYData(1234.5406, -0.0274395862675722));
            manualData.Add(new XYData(1234.5407, -0.0280564361983637));
            manualData.Add(new XYData(1234.5408, -0.0286527934226077));
            manualData.Add(new XYData(1234.5409, -0.0292284755578535));
            manualData.Add(new XYData(1234.541, -0.0297833129450907));
            manualData.Add(new XYData(1234.5411, -0.0303171486549644));
            manualData.Add(new XYData(1234.5412, -0.0308298384741087));
            manualData.Add(new XYData(1234.5413, -0.0313212508913976));
            manualData.Add(new XYData(1234.5414, -0.0317912670762292));
            manualData.Add(new XYData(1234.5415, -0.0322397808502262));
            manualData.Add(new XYData(1234.5416, -0.0326666986524189));
            manualData.Add(new XYData(1234.5417, -0.0330719394979722));
            manualData.Add(new XYData(1234.5418, -0.033455434931374));
            manualData.Add(new XYData(1234.5419, -0.0338171289690162));
            manualData.Add(new XYData(1234.542, -0.0341569780442059));
            manualData.Add(new XYData(1234.5421, -0.0344749509372333));
            manualData.Add(new XYData(1234.5422, -0.0347710287037813));
            manualData.Add(new XYData(1234.5423, -0.0350452045964331));
            manualData.Add(new XYData(1234.5424, -0.0352974839802444));
            manualData.Add(new XYData(1234.5425, -0.0355278842424764));
            manualData.Add(new XYData(1234.5426, -0.0357364346965751));
            manualData.Add(new XYData(1234.5427, -0.0359231764808846));
            manualData.Add(new XYData(1234.5428, -0.0360881624497418));
            manualData.Add(new XYData(1234.5429, -0.0362314570632891));
            manualData.Add(new XYData(1234.543, -0.0363531362676137));
            manualData.Add(new XYData(1234.5431, -0.0364532873718035));
            manualData.Add(new XYData(1234.5432, -0.0365320089193753));
            manualData.Add(new XYData(1234.5433, -0.0365894105546262));
            manualData.Add(new XYData(1234.5434, -0.0366256128840144));
            manualData.Add(new XYData(1234.5435, -0.0366407473326777));
            manualData.Add(new XYData(1234.5436, -0.0366349559961584));
            manualData.Add(new XYData(1234.5437, -0.0366083914876224));
            manualData.Add(new XYData(1234.5438, -0.0365612167803666));
            manualData.Add(new XYData(1234.5439, -0.0364936050459901));
            manualData.Add(new XYData(1234.544, -0.0364057394882701));
            manualData.Add(new XYData(1234.5441, -0.0362978131728387));
            manualData.Add(new XYData(1234.5442, -0.036170028852792));
            manualData.Add(new XYData(1234.5443, -0.0360225987903499));
            manualData.Add(new XYData(1234.5444, -0.0358557445746861));
            manualData.Add(new XYData(1234.5445, -0.0356696969356079));
            manualData.Add(new XYData(1234.5446, -0.035464695555831));
            manualData.Add(new XYData(1234.5447, -0.0352409888755214));
            manualData.Add(new XYData(1234.5448, -0.0349988338976875));
            manualData.Add(new XYData(1234.5449, -0.0347384959883132));
            manualData.Add(new XYData(1234.545, -0.0344602486738102));
            manualData.Add(new XYData(1234.5451, -0.0341643734355136));
            manualData.Add(new XYData(1234.5452, -0.0338511595013486));
            manualData.Add(new XYData(1234.5453, -0.0335209036348007));
            manualData.Add(new XYData(1234.5454, -0.0331739099205064));
            manualData.Add(new XYData(1234.5455, -0.032810489551405));
            manualData.Add(new XYData(1234.5456, -0.0324309606056523));
            manualData.Add(new XYData(1234.5457, -0.0320356478292074));
            manualData.Add(new XYData(1234.5458, -0.0316248824124804));
            manualData.Add(new XYData(1234.5459, -0.0311990017658336));
            manualData.Add(new XYData(1234.546, -0.0307583492932947));
            manualData.Add(new XYData(1234.5461, -0.030303274164611));
            manualData.Add(new XYData(1234.5462, -0.0298341310857817));
            manualData.Add(new XYData(1234.5463, -0.0293512800681928));
            manualData.Add(new XYData(1234.5464, -0.0288550861953485));
            manualData.Add(new XYData(1234.5465, -0.0283459193941514));
            manualData.Add(new XYData(1234.5466, -0.0278241541938703));
            manualData.Add(new XYData(1234.5467, -0.0272901694954874));
            manualData.Add(new XYData(1234.5468, -0.0267443483347052));
            manualData.Add(new XYData(1234.5469, -0.0261870776454554));
            manualData.Add(new XYData(1234.547, -0.025618748022918));
            manualData.Add(new XYData(1234.5471, -0.0250397534861888));
            manualData.Add(new XYData(1234.5472, -0.0244504912407124));
            manualData.Add(new XYData(1234.5473, -0.0238513614392417));
            manualData.Add(new XYData(1234.5474, -0.0232427669496635));
            manualData.Add(new XYData(1234.5475, -0.0226251131093278));
            manualData.Add(new XYData(1234.5476, -0.0219988074932488));
            manualData.Add(new XYData(1234.5477, -0.0213642596759247));
            manualData.Add(new XYData(1234.5478, -0.0207218809950267));
            manualData.Add(new XYData(1234.5479, -0.0200720843157252));
            manualData.Add(new XYData(1234.548, -0.019415283795777));
            manualData.Add(new XYData(1234.5481, -0.0187518946514898));
            manualData.Add(new XYData(1234.5482, -0.0180823329231528));
            manualData.Add(new XYData(1234.5483, -0.0174070152492131));
            manualData.Add(new XYData(1234.5484, -0.0167263586264406));
            manualData.Add(new XYData(1234.5485, -0.0160407801866451));
            manualData.Add(new XYData(1234.5486, -0.015350696967255));
            manualData.Add(new XYData(1234.5487, -0.014656525684976));
            manualData.Add(new XYData(1234.5488, -0.013958682511124));
            manualData.Add(new XYData(1234.5489, -0.0132575828487383));
            manualData.Add(new XYData(1234.549, -0.012553641111583));
            manualData.Add(new XYData(1234.5491, -0.011847270503531));
            manualData.Add(new XYData(1234.5492, -0.0111388828080829));
            manualData.Add(new XYData(1234.5493, -0.0104288881640119));
            manualData.Add(new XYData(1234.5494, -0.00971769485936282));
            manualData.Add(new XYData(1234.5495, -0.0090057091198196));
            manualData.Add(new XYData(1234.5496, -0.00829333490116474));
            manualData.Add(new XYData(1234.5497, -0.00758097368431987));
            manualData.Add(new XYData(1234.5498, -0.00686902427306024));
            manualData.Add(new XYData(1234.5499, -0.0061578825944918));
            manualData.Add(new XYData(1234.55, -0.00544794150076742));
            manualData.Add(new XYData(1234.5501, -0.00473959058180544));
            manualData.Add(new XYData(1234.5502, -0.00403321596488855));
            manualData.Add(new XYData(1234.5503, -0.00332920013351097));
            manualData.Add(new XYData(1234.5504, -0.00262792174133056));
            manualData.Add(new XYData(1234.5505, -0.00192975543099529));
            manualData.Add(new XYData(1234.5506, -0.00123507165630272));
            manualData.Add(new XYData(1234.5507, -0.000544236507765808));
            manualData.Add(new XYData(1234.5508, 0.000142388458343905));
            manualData.Add(new XYData(1234.5509, 0.00082444638895433));
            manualData.Add(new XYData(1234.551, 0.00150158529180048));
            manualData.Add(new XYData(1234.5511, 0.00217345820485615));
            manualData.Add(new XYData(1234.5512, 0.00283972334650891));
            manualData.Add(new XYData(1234.5513, 0.0035000442697097));
            manualData.Add(new XYData(1234.5514, 0.00415409001071467));
            manualData.Add(new XYData(1234.5515, 0.00480153523391968));
            manualData.Add(new XYData(1234.5516, 0.00544206037273612));
            manualData.Add(new XYData(1234.5517, 0.00607535176645865));
            manualData.Add(new XYData(1234.5518, 0.00670110179449088));
            manualData.Add(new XYData(1234.5519, 0.00731900899938318));
            manualData.Add(new XYData(1234.552, 0.00792877821993895));
            manualData.Add(new XYData(1234.5521, 0.00853012070589186));
            manualData.Add(new XYData(1234.5522, 0.00912275423550277));
            manualData.Add(new XYData(1234.5523, 0.00970640322747089));
            manualData.Add(new XYData(1234.5524, 0.0102807988485557));
            manualData.Add(new XYData(1234.5525, 0.0108456791168818));
            manualData.Add(new XYData(1234.5526, 0.0114007890008978));
            manualData.Add(new XYData(1234.5527, 0.0119458805151907));
            manualData.Add(new XYData(1234.5528, 0.0124807128057457));
            manualData.Add(new XYData(1234.5529, 0.0130050522431615));
            manualData.Add(new XYData(1234.553, 0.0135186724990165));
            manualData.Add(new XYData(1234.5531, 0.0140213546240184));
            manualData.Add(new XYData(1234.5532, 0.0145128871204422));
            manualData.Add(new XYData(1234.5533, 0.0149930660100963));
            manualData.Add(new XYData(1234.5534, 0.0154616948978084));
            manualData.Add(new XYData(1234.5535, 0.0159185850304266));
            manualData.Add(new XYData(1234.5536, 0.0163635553523282));
            manualData.Add(new XYData(1234.5537, 0.0167964325514198));
            manualData.Add(new XYData(1234.5538, 0.0172170511107415));
            manualData.Add(new XYData(1234.5539, 0.0176252533454365));
            manualData.Add(new XYData(1234.554, 0.0180208894403439));
            manualData.Add(new XYData(1234.5541, 0.0184038174820934));
            manualData.Add(new XYData(1234.5542, 0.0187739034867392));
            manualData.Add(new XYData(1234.5543, 0.0191310214229457));
            manualData.Add(new XYData(1234.5544, 0.0194750532307416));
            manualData.Add(new XYData(1234.5545, 0.0198058888365943));
            manualData.Add(new XYData(1234.5546, 0.0201234261603714));
            manualData.Add(new XYData(1234.5547, 0.0204275711254225));
            manualData.Add(new XYData(1234.5548, 0.0207182376567568));
            manualData.Add(new XYData(1234.5549, 0.0209953476787019));
            manualData.Add(new XYData(1234.555, 0.0212588311074997));
            manualData.Add(new XYData(1234.5551, 0.0215086258396373));
            manualData.Add(new XYData(1234.5552, 0.0217446777359418));
            manualData.Add(new XYData(1234.5553, 0.021966940601479));
            manualData.Add(new XYData(1234.5554, 0.0221753761617425));
            manualData.Add(new XYData(1234.5555, 0.0223699540324016));
            manualData.Add(new XYData(1234.5556, 0.022550651689668));
            manualData.Add(new XYData(1234.5557, 0.02271745443185));
            manualData.Add(new XYData(1234.5558, 0.0228703553393281));
            manualData.Add(new XYData(1234.5559, 0.0230093552300845));
            manualData.Add(new XYData(1234.556, 0.0231344626113314));
            manualData.Add(new XYData(1234.5561, 0.0232456936272823));
            manualData.Add(new XYData(1234.5562, 0.0233430720031194));
            manualData.Add(new XYData(1234.5563, 0.023426628985383));
            manualData.Add(new XYData(1234.5564, 0.0234964032777654));
            manualData.Add(new XYData(1234.5565, 0.023552440975127));
            manualData.Add(new XYData(1234.5566, 0.0235947954920044));
            manualData.Add(new XYData(1234.5567, 0.0236235274885868));
            manualData.Add(new XYData(1234.5568, 0.0236387047930276));
            manualData.Add(new XYData(1234.5569, 0.0236404023203566));
            manualData.Add(new XYData(1234.557, 0.02362870198805));
            manualData.Add(new XYData(1234.5571, 0.0236036926283369));
            manualData.Add(new XYData(1234.5572, 0.0235654698971914));
            manualData.Add(new XYData(1234.5573, 0.0235141361806717));
            manualData.Add(new XYData(1234.5574, 0.0234498004972908));
            manualData.Add(new XYData(1234.5575, 0.0233725783982258));
            manualData.Add(new XYData(1234.5576, 0.0232825918642144));
            manualData.Add(new XYData(1234.5577, 0.0231799691996666));
            manualData.Add(new XYData(1234.5578, 0.0230648449239964));
            manualData.Add(new XYData(1234.5579, 0.0229373596602475));
            manualData.Add(new XYData(1234.558, 0.0227976600210893));

            return manualData;
        }

    }     


}
