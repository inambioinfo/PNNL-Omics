﻿using System;
using System.Collections.Generic;

namespace PNNLOmics.Data
{
    /// <summary>
    /// This class encapsulates peptide level information.
    /// </summary>
    public class Peptide: Molecule
    {
        public int Id
        {
            get;
            set;
        }        
        public override void Clear()
        {
            throw new NotImplementedException();
        }        
        public List<Protein> ProteinList
        {
            get;
            set;
        }        
        public string Sequence
        {
            get; 
            set;
        }        
        public string ExtendedSequence
        {
            get;
            set;
        }
        public int CleavageState
        {
            get;
            set;            
        }
    }
}
