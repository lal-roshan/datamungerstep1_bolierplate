/////////////////////////////////////////////////////////////
// Created On: 2020-09-03
// 2020-09-03 | Initial commit Part1 Step2 Completed
// 2020-09-04 | Functionalitites extended
/////////////////////////////////////////////////////////////

#region  Using
using System;
using System.Collections.Generic;
using System.Linq;
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
        #region Constants
        /// <summary>
        /// String to display if there are no filters in query while getting filter
        /// </summary>
        public const string NoFilterString = "Query doesn't contain any filters";

        /// <summary>
        /// String to display if there are no logical operators in the filter part of query
        /// </summary>
        public const string NoLogicalOperatorsString = "Query doesn't contain any logical operators in filter";

        /// <summary>
        /// Message to be displayed when there is no order by clause in the query
        /// </summary>
        public const string NoOrderByClause = "Query doesn't contain order by clause";

        /// <summary>
        /// Message to be displayed when there is no order by clause in the base query
        /// </summary>
        public const string NoBaseOrderByClause = "Base query doesn't contain order by clause";
        #endregion

        #region Enums
        /// <summary>
        /// enum for denoting whether the first or last index is to be found
        /// </summary>
        public enum Index
        {
            First,
            Last
        }

        /// <summary>
        /// Enum for choosing what should the split funtion return from the input string
        /// DoNothing - Return parts of string by simply splitting the string at substirng(s)
        /// RemoveThis - Return parts of string by splitting the string at substirng(s) and
        /// removing substirng(s) form the collection
        /// RemoveAllButThis - Return parts of string which only constitutes the substring(s)
        /// </summary>
        public enum SplitType
        {
            DoNothing,
            RemoveThis,
            RemoveAllButThis
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
                int selectIndex = GetStringIndex(queryString, "select");
                int fromIndex = GetStringIndex(queryString, "from");
                int whereIndex = GetStringIndex(queryString, "where");
                //Checks wether there is a select and from clause and whether there is atleast a character
                //between them (selectIndex + 6 is for 5 letters of select plus one for whitespace)
                if (selectIndex == -1 || fromIndex == -1 || selectIndex + 7 == fromIndex)
                {
                    return false;
                }
                int selectCount = StringMatchCount(queryString, "select");
                int fromCount = StringMatchCount(queryString, "from");
                int whereCount = StringMatchCount(queryString, "where");
                int orderByCount = StringMatchCount(queryString, "order by");
                int groupByCount = StringMatchCount(queryString, "group by");

                //if a query is valid there should be equal number of select and from clause
                //similarly if a query has where , order by or group clauses there count 
                //cannot be greater thaan the select clause
                if (selectCount != fromCount ||
                   (whereCount > 0 && whereCount > selectCount) ||
                   (orderByCount > 0 && orderByCount > selectCount) ||
                   (groupByCount > 0 && groupByCount > selectCount) ||
                   (whereIndex < fromIndex && whereCount == fromCount))
                {
                    return false;
                }
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
                count = Regex.Matches(source, "\\b(" + pattern + ")\\b", RegexOptions.IgnoreCase).Count;
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

        /// <summary>
        /// Method to split the source string by substring(s)
        /// </summary>
        /// <param name="source">The input string that is to be split</param>
        /// <param name="substrings">coma seperated substrings at which source has to be split
        /// (If no string is provided the source will be split by space and split type will be ignored)</param>
        /// <param name="type">Type of operation that should be performed on the returned collection</param>
        /// <returns>Collection of split words</returns>
        public static List<string> SplitByString(string source, string substrings,
            SplitType type = SplitType.DoNothing)
        {
            List<string> splitResult = new List<string>();

            //If no substrings are provided then split the source by whitespace
            if (string.IsNullOrEmpty(substrings))
            {
                splitResult = source.Split(' ').ToList();
            }
            else
            {
                //Creating a regex filter with all substrings
                StringBuilder sb = new StringBuilder();
                sb.Append("\\b(");
                foreach(string substring in substrings.Split(','))
                {
                    sb.Append(substring.Trim() + "|");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(")\\b");
               
                //Split the souce based on substrings provided
                splitResult = Regex.Split(source, sb.ToString(), RegexOptions.IgnoreCase).ToList();

                //Decide whether to remove the substrings from source or keep substrings
                if(splitResult.Count > 0)
                {
                    switch (type)
                    {
                        case SplitType.RemoveAllButThis:
                            splitResult = splitResult.Where(x => StringMatchCount(substrings, x) > 0).
                            Select(x => x.Trim()).ToList();
                            break;

                        case SplitType.RemoveThis:
                            splitResult = splitResult.Where(x => StringMatchCount(substrings, x) == 0).
                            Select(x => x.Trim()).ToList();
                            break;
                    }
                }
            }

            return splitResult;
        }
        #endregion
    }
    #endregion
}

#endregion