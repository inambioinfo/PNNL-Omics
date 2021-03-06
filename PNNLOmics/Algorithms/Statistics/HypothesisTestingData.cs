﻿namespace PNNLOmics.Algorithms.Statistics
{

    /// <summary>
    /// Holds hypothesis testing rules.
    /// </summary>
    [System.Obsolete("Code moved to MultiAlign: MultiAlignCore.Algorithms.Statistics")]
    public class HypothesisTestingData
    {
        public HypothesisTestingData(double twoTail,
                                     double leftTail,
                                     double rightTail)
        {
            RightTail = rightTail;
            LeftTail = leftTail;
            TwoTail = twoTail;
        }


        public HypothesisTestingData(double pValue)
        {
            PValue = pValue;
        }

        public double RightTail { get; private set; }
        public double LeftTail { get; private set; }
        public double TwoTail { get; private set; }

        public double PValue { get; set; }
    }
}
