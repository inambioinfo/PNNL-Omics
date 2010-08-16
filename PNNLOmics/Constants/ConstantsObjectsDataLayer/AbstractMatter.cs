﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Constants.ConstantsObjectsDataLayer
{
    public abstract class AbstractMatter
    {
        public string Name { get; set; }
        public double MonoIsotopicMass { get; set; }
        public string Symbol { get; set; } 
    }
}
