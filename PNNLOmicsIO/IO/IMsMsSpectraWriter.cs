﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PNNLOmics.Data;

namespace PNNLOmicsIO.IO
{
    public interface IMsMsSpectraWriter
    {
        /// <summary>
        /// Creates a MS/MS DTA file at the path provided.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="msmsFeatures"></param>
        void Write(string path, IEnumerable<MSSpectra> msmsFeatures);
        /// <summary>
        /// Appends the MS/MS spectra to the file path provided.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="msmsFeatures"></param>
        void Append(string path, IEnumerable<MSSpectra> msmsFeatures);
    }
}
