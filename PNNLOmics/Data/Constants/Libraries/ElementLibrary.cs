﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using PNNLOmics.Data.Constants;
using PNNLOmics.Utilities;

/// <example>
/// dictionary implementation                        
/// Dictionary<string,ElementObject> ElementDictionary = ElementLibrary.LoadElementData();
/// double elementC12Mass = ElementDictionary["C"].IsotopeDictionary["C12"].Mass;
/// double elementC13Mass = ElementDictionary["C"].IsotopeDictionary["C13"].Mass;
/// double elementC12Abund = ElementDictionary["C"].IsotopeDictionary["C12"].NaturalAbundance;
/// double elementC13Abund = ElementDictionary["C"].IsotopeDictionary["C13"].NaturalAbundance;
/// double elemetMonoMass = ElementDictionary["C"].MonoIsotopicMass;
/// string elementName = ElementDictionary["C"].Name;
/// string elementSymbol = ElementDictionary["C"].Symbol;                     
///
/// One line implementation
/// double elementMonoMass = ElementConstantsStaticLibrary.GetMonoisotopicMass("C");
/// string elementName = ElementConstantsStaticLibrary.GetName("C");
/// string elementSymbol = ElementConstantsStaticLibrary.GetSymbol("C");
///
/// double elementMass3 = ElementStaticLibrary.GetMonoisotopicMass(SelectElement.Hydrogen);
/// </example>
namespace PNNLOmics.Data.Constants.Libraries
{
    /// <summary>
    /// Sets up the element library by loading the information from the disk
    /// </summary>
    public class ElementLibrary : MatterLibrary<Element, ElementName>
    {
		protected const string OMICS_ELEMENT_DATA_FILE = "PNNLOmicsElementData.xml";

        #region Loading Data
        /// <summary>
        /// This is a Class designed to load periodic table of the elements data from a XML file PNNLOmicsElementData.xml
        /// IUPAC 2000 Atomic Weights of the Elements (published 2003) was used.
        /// Differences from the old version:  Elements H, B, C, N, O, Na, P, S, Cl, K, Ca were updated.  Table 5 in the paper has the probabilities (best measurement column was used).
        /// </summary>
        public void LoadXML(string constantsFileName, out List<string> elementSymbolList, out List<Element> elementList)
        {
            XmlReader readerXML = XmlReader.Create(constantsFileName);

            int numberOfIsotopes = 0;
            int atomicity = 0;
            int isotopeNumber = 0;
            double massAverage = 0;
            double massAverageUncertainty = 0;
            double isotopeMass = 0;
            double isotopeProbability = 0;
            double monoIsotopicMass = 0;
            elementSymbolList = new List<string>();
            elementList = new List<Element>();

            while (readerXML.Read())
            {
                if (readerXML.NodeType == XmlNodeType.Element)
                {
                    if (readerXML.Name == "NumElements")
                    {
                        int numElements = readerXML.ReadElementContentAsInt();// Parse(Xreader.GetAttribute("Symbol"));
                    }

                    if (readerXML.Name == "Element")
                    {
                        Element newElement = new Element();
                        Dictionary<string, Isotope> newIsotopeDictionary = new Dictionary<string, Isotope>();

                        readerXML.ReadToFollowing("Symbol");
                        newElement.Symbol = readerXML.ReadElementContentAsString();

                        readerXML.ReadToFollowing("Name");
                        newElement.Name = readerXML.ReadElementContentAsString();

                        readerXML.ReadToFollowing("NumIsotopes");
                        numberOfIsotopes = readerXML.ReadElementContentAsInt();

                        readerXML.ReadToFollowing("Atomicity");
                        atomicity = readerXML.ReadElementContentAsInt();

                        readerXML.ReadToFollowing("MassAverage");
                        massAverage = readerXML.ReadElementContentAsDouble();

                        readerXML.ReadToFollowing("MassAverageUncertainty");
                        massAverageUncertainty = readerXML.ReadElementContentAsDouble();

                        //for each isotope
                        for (int i = 0; i < numberOfIsotopes; i++)
                        {
                            readerXML.ReadToFollowing("Isotope");

                            readerXML.ReadToFollowing("IsotopeNumber");

                            isotopeNumber = readerXML.ReadElementContentAsInt();

                            readerXML.ReadToFollowing("Mass");

                            isotopeMass = readerXML.ReadElementContentAsDouble();

                            if (i == 0)
                            {
                                monoIsotopicMass = isotopeMass;
                            }

                            readerXML.ReadToFollowing("Probability");
                            isotopeProbability = readerXML.ReadElementContentAsDouble();

                            Isotope NewIsotope = new Isotope(isotopeNumber, isotopeMass, isotopeProbability);

                            newIsotopeDictionary.Add(newElement.Symbol + isotopeNumber.ToString(), NewIsotope);
                            //newIsotopeDictionary.Add(newElement.Symbol + i.ToString(), NewIsotope);//used for interating through
                        }

                        newElement.IsotopeDictionary = newIsotopeDictionary;
                        newElement.MassMonoIsotopic = monoIsotopicMass;
                        newElement.MassAverage = massAverage;//IUPAC Atomic weights of the elements 2007, M. Wieser, M. Berglund
                        newElement.MassAverageUncertainty = massAverageUncertainty;

                        elementList.Add(newElement);
                        elementSymbolList.Add(newElement.Symbol);

                        readerXML.Skip();//skip white space
                    }
                }
            }
        }

        /// <summary>
        /// This is a Class designed to load periodic table of the elements data from a hard coded class derived from PNNLOmicsElementData.xml
        /// IUPAC 2000 Atomic Weights of the Elements (published 2003) was used.
        /// Differences from the old version:  Elements H, B, C, N, O, Na, P, S, Cl, K, Ca were updated.  Table 5 in the paper has the probabilities (best measurement column was used).
        /// </summary>
        public void LoadHardCoded(out List<string> elementSymbolList, out List<Element> elementList)
        {
            PeriodicTableLibrary.Load(out elementSymbolList, out elementList);
        }

        /// <summary>
        /// This is a Class designed to convert raw values into element objects (including masses and isotope abundances)
        /// and create an element dictionary searchable by key string such as "C" for carbon.
        /// </summary>
        public override void LoadLibrary()
        {
            m_symbolToCompoundMap = new Dictionary<string, Element>();
            m_enumToSymbolMap = new Dictionary<ElementName, string>();

            ResolveUNCPath.MappedDriveResolver uncPathCheck = new ResolveUNCPath.MappedDriveResolver();
            string asemblyDirectoryOrUNCDirectory = uncPathCheck.ResolveToUNC(PathUtilities.AssemblyDirectory);

            FileInfo constantsFileInfo = new FileInfo(System.IO.Path.Combine(asemblyDirectoryOrUNCDirectory, OMICS_ELEMENT_DATA_FILE));
			//FileInfo constantsFileInfo = new FileInfo(System.IO.Path.Combine(PathUtil.AssemblyDirectory, OMICS_ELEMENT_DATA_FILE));

            

            List<string> elementSymbolList = new List<string>();
            List<Element> elementList = new List<Element>();

            if (constantsFileInfo.Exists)
            {
                try
                {
                    LoadXML(constantsFileInfo.FullName, out elementSymbolList, out elementList);
                }
                catch
                {
                    LoadHardCoded(out elementSymbolList, out elementList);
                }
            }
            else
            {
                LoadHardCoded(out elementSymbolList, out elementList);
            }

            //if there is no file load from code
            if(elementSymbolList.Count==0)//file did not work
            {
                LoadHardCoded(out elementSymbolList, out elementList);
            }

            if (elementSymbolList.Count==0)//we still failed
            {
                throw new FileNotFoundException("The " + OMICS_ELEMENT_DATA_FILE + " file cannot be found at " + constantsFileInfo.FullName);
            }
           
            for (int i = 0; i < elementSymbolList.Count; i++)
            {
                m_symbolToCompoundMap.Add(elementSymbolList[i], elementList[i]);
            }

            int counter = 0;
            foreach (ElementName enumElement in Enum.GetValues(typeof(ElementName)))
            {
                m_enumToSymbolMap.Add(enumElement, elementList[counter].Symbol);
                counter++;
            }
        }

        #endregion
    }

}
