﻿using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PNNLOmics.Utilities
{
    public class StringUtilities
    {
        private const string SCIENTIFIC_NOTATION_CLEANUP_REGEX = "0+E";
        private static readonly Regex m_scientificNotationTrim = new Regex(SCIENTIFIC_NOTATION_CLEANUP_REGEX, RegexOptions.Compiled);

        /// <summary>
        /// Dictionary that tracks the format string used for each digitsOfPrecision value
        /// </summary>
        /// <remarks>
        /// Keys are the number of digits of precision
        /// Values are strings like "0.0", "0.0#", "0.0##", etc.
        /// </remarks>
        private static readonly ConcurrentDictionary<byte, string> mFormatStrings = new ConcurrentDictionary<byte, string>();

        /// <summary>
        /// Dictionary that tracks the format string used for each digitsOfPrecision value displayed with scientific notation
        /// </summary>
        /// <remarks>
        /// Keys are the number of digits of precision and
        ///   "false" if the format string is of the form 0.00E+00 or 
        ///   "true"  if the format string is of the form 0.00E+000
        /// Values are strings like "0.0E+00", "0.0#E+00", "0.0##E+00", "0.0#E+000", or "0.0##E+000"
        /// </remarks>
        private static readonly ConcurrentDictionary<string, string> mFormatStringsScientific = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Convert value to a string with 5 digits of precision
        /// </summary>
        /// <param name="value">Number to convert to text</param>
        /// <returns>String representation of the value</returns>
        /// <remarks>Numbers larger than 1000000 or smaller than 0.000001 will be in scientific notation</remarks>
        public static string DblToString(double value)
        {
            return DblToString(value, 5, 1000000);
        }


        /// <summary>
        /// Format the value to a string with a fixed number of decimal points
        /// </summary>
        /// <param name="value">Value to format</param>
        /// <param name="digitsAfterDecimal">Digits to show after the decimal place (0 or higher)</param>
        /// <param name="thresholdScientific">Numbers below this level will be displayed using Scientific notation</param>
        /// <returns>String representation of the value</returns>
        /// <remarks>If digitsOfPrecision is 0, will round the number to the nearest integer</remarks>
        public static string DblToString(
            double value,
            byte digitsAfterDecimal,
            double thresholdScientific)
        {
            return DblToString(value, digitsAfterDecimal, limitDecimalsForLargeValues: false, thresholdScientific: thresholdScientific, invariantCulture: true);
        }

        /// <summary>
        /// Format the value to a string with a fixed number of decimal points
        /// </summary>
        /// <param name="value">Value to format</param>
        /// <param name="digitsAfterDecimal">Digits to show after the decimal place (0 or higher)</param>
        /// <param name="limitDecimalsForLargeValues">When true, will limit the number of decimal points shown for values over 1</param>
        /// <param name="thresholdScientific">Numbers below this level will be displayed using Scientific notation</param>
        /// <param name="invariantCulture">
        /// When true (default) numbers will always use a period for the decimal point.  
        /// When false, the decimal point symbol will depend on the current system's culture settings.
        /// </param>
        /// <returns>String representation of the value</returns>
        /// <remarks>If digitsOfPrecision is 0, will round the number to the nearest integer</remarks>
        public static string DblToString(
            double value,
            byte digitsAfterDecimal,
            bool limitDecimalsForLargeValues = false,
            double thresholdScientific = 0.001,
            bool invariantCulture = true)
        {

            if (Math.Abs(value) < double.Epsilon)
                return "0";

            if (Math.Abs(value) >= 10 && Math.Abs(value - (int)value) < 1 / (Math.Pow(10, digitsAfterDecimal)) / 2.0)
            {
                // Value 10 or larger and it is nearly an integer value (at least with respect to digitsOfPrecision)
                // Return values like 10 or 150 instead of 10.000 or 150.000
                return value.ToString("0");
            }

            if (Math.Abs(value) < thresholdScientific)
            {
                return DblToStringScientific(value, digitsAfterDecimal, invariantCulture);
            }

            var effectiveDigitsAfterDecimal = digitsAfterDecimal;

            if (Math.Abs(value) > 1 && limitDecimalsForLargeValues)
            {
                var digitsRightOfDecimal = digitsAfterDecimal - (byte)(Math.Floor(Math.Log10(value)));

                if (digitsRightOfDecimal >= 0)
                    effectiveDigitsAfterDecimal = (byte)digitsRightOfDecimal;
                else
                    effectiveDigitsAfterDecimal = 0;

            }

            if (effectiveDigitsAfterDecimal <= 0)
                return value.ToString("0");

            string formatString;
            if (mFormatStrings.TryGetValue(effectiveDigitsAfterDecimal, out formatString))
            {
                return value.ToString(formatString, invariantCulture ? NumberFormatInfo.InvariantInfo : NumberFormatInfo.CurrentInfo);
            }

            formatString = "0.0";

            if (effectiveDigitsAfterDecimal > 1)
            {
                // Update format string to be of the form "0.0#######"
                formatString += new string('#', effectiveDigitsAfterDecimal - 1);
            }

            try
            {
                mFormatStrings.TryAdd(effectiveDigitsAfterDecimal, formatString);
            }
            catch
            {
                // Ignore errors here
            }

            return value.ToString(formatString, invariantCulture ? NumberFormatInfo.InvariantInfo : NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Format the value to a string using scientific notation
        /// </summary>
        /// <param name="value">Value to format</param>
        /// <param name="digitsAfterDecimal">Digits to show after the decimal place (0 or higher)</param>
        /// <param name="invariantCulture">
        /// When true (default) numbers will always use a period for the decimal point.  
        /// When false, the decimal point symbol will depend on the current system's culture settings.
        /// </param>
        /// <returns>String representation of the value</returns>
        /// <remarks>If digitsOfPrecision is 0, will round the number to the nearest integer</remarks>
        public static string DblToStringScientific(
            double value,
            byte digitsAfterDecimal,
            bool invariantCulture = true)
        {
            if (Math.Abs(value) < double.Epsilon)
                return "0";

            if (digitsAfterDecimal <= 0)
                return value.ToString("0");

            var tinyNumber = Math.Log10(Math.Abs(value)) <= -99;

            string formatString;
            if (mFormatStringsScientific.TryGetValue(digitsAfterDecimal + tinyNumber.ToString(), out formatString))
            {
                return value.ToString(formatString, invariantCulture ? NumberFormatInfo.InvariantInfo : NumberFormatInfo.CurrentInfo);
            }

            formatString = "0.0";

            if (digitsAfterDecimal > 1)
            {
                // Update format string to be of the form "0.0#######"
                formatString += new string('#', digitsAfterDecimal - 1);
            }

            if (tinyNumber)
            {
                formatString += "E+000";
            }
            else
            {
                formatString += "E+00";
            }

            try
            {
                mFormatStringsScientific.TryAdd(digitsAfterDecimal + tinyNumber.ToString(), formatString);
            }
            catch
            {
                // Ignore errors here
            }

            return value.ToString(formatString, invariantCulture ? NumberFormatInfo.InvariantInfo : NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Convert value to a string with 5 total digits of precision
        /// </summary>
        /// <param name="value">Number to convert to text</param>
        /// <returns>Number as text</returns>
        /// <remarks>Numbers larger than 1000000 or smaller than 0.000001 will be in scientific notation</remarks>
        public static string ValueToString(double value)
        {
            return ValueToString(value, 5, 1000000);
        }

        /// <summary>
        /// Convert value to a string with the specified total digits of precision
        /// </summary>
        /// <param name="value">Number to convert to text</param>
        /// <param name="digitsOfPrecision">Total digits of precision (before and after the decimal point)</param>
        /// <returns>Number as text</returns>
        /// <remarks>Numbers larger than 1000000 or smaller than 0.000001 will be in scientific notation</remarks>
        public static string ValueToString(double value, byte digitsOfPrecision)
        {
            return ValueToString(value, digitsOfPrecision, 1000000);
        }

        /// <summary>
        /// Convert value to a string with the specified total digits of precision and customized scientific notation threshold
        /// </summary>
        /// <param name="value">Number to convert to text</param>
        /// <param name="digitsOfPrecision">Total digits of precision (before and after the decimal point)</param>
        /// <param name="scientificNotationThreshold">
        /// Values larger than this threshold (positive or negative) will be converted to scientific notation
        /// Also, values less than "1 / scientificNotationThreshold" will be converted to scientific notation
        /// Thus, if this threshold is 1000000, numbers larger than 1000000 or smaller than 0.000001 will be in scientific notation
        /// </param>
        /// <returns>Number as text</returns>
        /// <remarks>This function differs from DblToString in that here digitsOfPrecision is the total digits while DblToString focuses on the number of digits after the decimal point</remarks>
        public static string ValueToString(
            double value, 
            byte digitsOfPrecision, 
            double scientificNotationThreshold)
        {
            byte totalDigitsOfPrecision;

            if (digitsOfPrecision < 1)
                totalDigitsOfPrecision = 1;
            else
                totalDigitsOfPrecision = digitsOfPrecision;

            double effectiveScientificNotationThreshold;
            if (Math.Abs(scientificNotationThreshold) < 10)
                effectiveScientificNotationThreshold = 10;
            else
                effectiveScientificNotationThreshold = Math.Abs(scientificNotationThreshold);

            try
            {
                var strMantissa = "0." + new string('0', Math.Max(totalDigitsOfPrecision - 1, 1)) + "E+00";
                string strValue;

                if (Math.Abs(value) < double.Epsilon)
                {
                    return "0";
                }

                if (Math.Abs(value) <= 1 / effectiveScientificNotationThreshold ||
                    Math.Abs(value) >= effectiveScientificNotationThreshold)
                {
                    // Use scientific notation
                    strValue = value.ToString(strMantissa);
                }
                else if (Math.Abs(value) < 1)
                {
                    var intDigitsAfterDecimal = (int)Math.Floor(-Math.Log10(Math.Abs(value))) + totalDigitsOfPrecision;
                    var strFormatString = "0." + new string('0', intDigitsAfterDecimal);

                    strValue = value.ToString(strFormatString);
                    if (Math.Abs(double.Parse(strValue)) < double.Epsilon)
                    {
                        // Value was converted to 0; use scientific notation
                        strValue = value.ToString(strMantissa);
                    }
                    else
                    {
                        strValue = strValue.TrimEnd('0').TrimEnd('.');
                    }
                }
                else
                {
                    var intDigitsAfterDecimal = totalDigitsOfPrecision - (int)Math.Ceiling(Math.Log10(Math.Abs(value)));

                    if (intDigitsAfterDecimal > 0)
                    {
                        strValue = value.ToString("0." + new string('0', intDigitsAfterDecimal));
                        strValue = strValue.TrimEnd('0').TrimEnd('.');
                    }
                    else
                    {
                        strValue = value.ToString("0");
                    }
                }

                if (totalDigitsOfPrecision <= 1)
                {
                    return strValue;
                }

                // Look for numbers in scientific notation with a series of zeroes before the E
                if (!m_scientificNotationTrim.IsMatch(strValue))
                {
                    return strValue;
                }

                // Match found, for example 1.5000E-43
                // Change it to instead be  1.5E-43

                var updatedValue = m_scientificNotationTrim.Replace(strValue, "E");

                // The number may now look like 1.E+43
                // If it does, then re-insert a zero after the decimal point
                return updatedValue.Replace(".E", ".0E");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ValueToString: " + ex.Message);
                return value.ToString(CultureInfo.InvariantCulture);
            }

        }

    }
}
