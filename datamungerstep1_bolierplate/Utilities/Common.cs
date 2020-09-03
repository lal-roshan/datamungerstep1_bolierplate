/////////////////////////////////////////////////////////////
// Created On: 2020-09-03
// 2020-09-03 | Initial commit Part1 Step2 Completed
/////////////////////////////////////////////////////////////

#region  Using
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
#endregion

#region Namespace
namespace DataMunger.Utilities
{
    #region Class
    /// <summary>
    /// Class containing utility functions
    /// </summary>
    public class Common
    {
        #region Enums
        /// <summary>
        /// enum for denoting whether the first or last index is to be found
        /// </summary>
        public enum Index
        {
            First,
            Last
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Method to do a basic checking whether or not a query is valid
        /// </summary>
        /// <param name="queryString">input query</param>
        /// <returns></returns>true or false
        public static bool IsValidQueryBasic(string queryString)
        {
            // valid query will have atleast 16 characters before where Eg: select * from t
            //(taking 15 as it is 0 based index)
            if (string.IsNullOrEmpty(queryString) || queryString.Length < 15)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Method to find the first or last index of a substring in a string
        /// </summary>
        /// <param name="source">String in which substring is to be searched</param>
        /// <param name="pattern">Substring whose index is to be found</param>
        /// <param name="type">Enum value representing whether first or last index is required(Default is first)</param>
        /// <returns>Index of the pattern if present else -1</returns>
        public static int GetStringIndex(string source, string pattern, Index type = Index.First)
        {
            int index = -1;
            if (string.IsNullOrEmpty(source))
            {
                return -1;
            }
            switch (type)
            {
                case Index.First:
                    index = source.Trim().IndexOf(pattern, StringComparison.InvariantCultureIgnoreCase);
                    break;
                case Index.Last:
                    index = source.Trim().LastIndexOf(pattern, StringComparison.InvariantCultureIgnoreCase);
                    break;
            }
            return index;
        }

        /// <summary>
        /// Method to get the count of matched substring in source string
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="pattern">substring to be checked</param>
        /// <returns>Number of matches of substring found in source</returns>
        public static int StringMatchCount(string source, string pattern)
        {
            int count;
            if (string.IsNullOrEmpty(source))
            {
                count = 0;
            }
            else 
            {
                count = Regex.Matches(source, pattern, RegexOptions.IgnoreCase).Count;
            }
            return count;
        }

        /// <summary>
        /// Method to replace the occurence of a substring from the source with a substitute
        /// </summary>
        /// <param name="source">source string</param>
        /// <param name="substring">part to be replaced</param>
        /// <param name="substitute">part to be added (Default is empty string)</param>
        /// <returns>new string with replaced substring</returns>
        public static string StringReplace(string source,string substring, string substitute ="")
        {
            return Regex.Replace(source, substring, substitute, RegexOptions.IgnoreCase);
        }
        #endregion
    }
    #endregion
}

#endregion