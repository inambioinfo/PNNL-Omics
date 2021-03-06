﻿using System;
using System.Collections.Generic;
using System.IO;
using PNNLOmics.Annotations;

namespace PNNLOmicsIO.IO
{
	public abstract class BaseTextFileReader<T> : ITextFileReader<T>
	{
        /// <summary>
        /// Default file delimiter.
        /// </summary>
        private const char DEFAULT_DELIMITER = ',';
	    protected BaseTextFileReader()
		{
            Delimiter = DEFAULT_DELIMITER;
		}

        #region Properties

        /// <summary>
        /// Gets or sets the file reading delimiter.
        /// </summary>
        [UsedImplicitly]
	    public char Delimiter { get; set; }

	    /// <summary>
        /// Gets or sets the file reading delimiter
        /// </summary>
		/// <remarks>
		/// The setter only uses the first character of the string
		/// </remarks>
        public string Delimeter
        {
	        get { return Delimiter.ToString(); }
	        set
	        {
	            if (string.IsNullOrEmpty(value))
	                throw new ArgumentOutOfRangeException(value, "Column delimiter cannot be empty");

                Delimiter = value[0]; 
            }
        }

        #endregion

        /// <summary>
        /// Open the file and return an enumerable of type T
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <returns>Enumerable list of data from the file</returns>        
		public IEnumerable<T> ReadFile(string fileLocation) 
		{
            IEnumerable<T> returnEnumerable;
            using (TextReader textReader = new StreamReader(new FileStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                returnEnumerable = ReadFile(textReader);
                textReader.Close();
            }
			return returnEnumerable;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textReader"></param>
        /// <returns></returns>
		public IEnumerable<T> ReadFile(TextReader textReader)
		{
			var columnMapping = CreateColumnMapping(textReader);

			if (columnMapping.Count == 0)
			{
				throw new ApplicationException("Given file does not contain any valid column headers.");
			}

			var enumerable = SaveFileToEnumerable(textReader, columnMapping);
			return enumerable;
		}

		protected abstract Dictionary<string, int> CreateColumnMapping(TextReader textReader);
		protected abstract IEnumerable<T> SaveFileToEnumerable(TextReader textReader, Dictionary<string, int> columnMapping);
	}
}
