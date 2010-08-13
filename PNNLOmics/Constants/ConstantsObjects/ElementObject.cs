﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Constants
{
    public class ElementObject:AbstractMatter
    {
        public double MassAverage { get; set; }
        
        public double MassMostAbundantIsotope { get; set; }
        public Dictionary<string, Isotope> IsotopeDictionary { get; set; }
    }
}