﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Algorithms.PeakDetection
{
    /// <summary>
    /// options for fitting to the top three peak top points
    /// </summary>
    public enum PeakFitType
    {
        /// <summary>
        /// //uses the parabola fit directly to find the apex and centroid
        /// </summary>
        Parabola,
        
        /// <summary>
        /// //takes the log first and then uses the parabola fit to find the apex and centroid
        /// </summary>
        Lorentzian
    }
}
