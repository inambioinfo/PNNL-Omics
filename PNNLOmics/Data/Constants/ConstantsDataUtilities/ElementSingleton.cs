﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data.Constants.ConstantsDataUtilities;

namespace PNNLOmics.Data.Constants.ConstantsDataLayer
{
    /// <summary>
    /// This class loads the elements constants once and is accessble through the dictionary property.  Thread-safe singleton example created at first call
    /// </summary> 
    public sealed class ElementSingleton
    {
        /// <summary>
        /// Utilizes the get and set auto implemented properties.
        /// Note that set; can be any other operator as long as it's
        /// less accessible than public.
        /// </summary>
        public static ElementSingleton Instance { get; private set; }

        /// <summary>
        /// A static constructor is automatically initialized on referenceto the class.
        /// </summary>
        static ElementSingleton() { Instance = new ElementSingleton(); }

        //the part of the singleton that does the work once.
        ElementSingleton()
        {
            Dictionary<string, Element> elementDictionary = ElementLibrary.LoadElementData();
            this.ConstantsDictionary = elementDictionary;//accessable outside by getter below

            int count = 0;
            string names = "";
            Dictionary<int, string> enumDictionary = new Dictionary<int, string>();
            foreach (KeyValuePair<string, Element> item in elementDictionary)
            {
                names += item.Key + ",";
                enumDictionary.Add(count, item.Key);
                count++;
            }
            names = "";
            for (int i = 0; i < elementDictionary.Count; i++)
            {
                names += ConstantsDictionary[enumDictionary[i]].Name + ",";
            }
            this.ConstantsEnumDictionary = enumDictionary;//accessable outside by getter below
        }

        public Dictionary<string, Element> ConstantsDictionary { get; set; }
        public Dictionary<int, string> ConstantsEnumDictionary { get; set; }
    }
}