﻿using System;
using System.Collections.Generic;

namespace PNNLOmics.Data
{
    /// <summary>
    /// Represents a peak.
    /// </summary>
    public class Peak: BaseData
    {
        private const float DEFAULT_HEIGHT = 0;
        private const float DEFAULT_WIDTH  = 0;
        private const float DEFAULT_XVALUE = 0;
        
        //TODO: Should we use an enumeration or int to identify the type of peak
        // e.g. Chromatographic, IMS, Profile, Centroid...or...
        //TODO: Should we use nullable types for float, etc.

        /// <summary>
        /// Default constructor. 
        /// </summary>
        public Peak()
        {
            Clear();
        }

        /// <summary>
        /// Gets or the height of the peak.
        /// </summary>
        public float Height
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the X-Value of the peak (e.g. time, scan, m/z)
        /// </summary>        
        public float XValue
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the width of a peak.  
        /// </summary>
        public float Width
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets local signal to noise ratio.
        /// </summary>
        public float LocalSignalToNoise 
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the background at a peak's x-value.
        /// </summary>
        public float Background
        {
            get;
            set;
        }
        /// <summary>
        /// Sets the values of the peak to its default value.
        /// </summary>
        public override void Clear()
        {
            Height = DEFAULT_HEIGHT;
            Width  = DEFAULT_WIDTH;
            XValue = DEFAULT_XVALUE;
        }
    }
}
