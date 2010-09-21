﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataUtilities;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    /// <summary>
    /// This is an abstract Class designed to cover the most basic parameters of compound objects.
    /// Compounds are made up of elements.
    /// </summary>
    public abstract class Compound : Matter
    {
        public string ChemicalFormula { get; set; }
        //This naming standard was changed so that the number of elements are grouped together in Intelli Sense
        public int nCarbon { get; set; }
        public int nHydrogen { get; set; }
        public int nNitrogen { get; set; }
        public int nOxygen { get; set; }
        public int nSulfur { get; set; }
        public int nPhosphorus { get; set; }
        public int nPotassium { get; set; }
        public int nSodium { get; set; }

        /// <summary>
        /// This static class is used to calculate the Monoisotopic mass from the element values.
        /// </summary>
        public static double GetMonoisotopicMass(Compound GeneralCompound)
        {
            Dictionary<string, Element> elementDictionary = ElementLibrary.LoadElementData();

            double ExactMass =
                GeneralCompound.nCarbon * elementDictionary["C"].MonoIsotopicMass +
                GeneralCompound.nHydrogen * elementDictionary["H"].MonoIsotopicMass +
                GeneralCompound.nNitrogen * elementDictionary["N"].MonoIsotopicMass +
                GeneralCompound.nOxygen * elementDictionary["O"].MonoIsotopicMass +
                GeneralCompound.nSulfur * elementDictionary["S"].MonoIsotopicMass +
                GeneralCompound.nPotassium * elementDictionary["K"].MonoIsotopicMass +
                GeneralCompound.nSodium * elementDictionary["Na"].MonoIsotopicMass +
                GeneralCompound.nPhosphorus * elementDictionary["P"].MonoIsotopicMass;

            return ExactMass;
        }
        
        public void NewElements(int C, int H, int N, int O, int S, int P)
        {
            nCarbon = C;
            nHydrogen = H;
            nNitrogen = N;
            nOxygen = O;
            nSulfur = S;
            nPhosphorus = P;
        }
    }


}