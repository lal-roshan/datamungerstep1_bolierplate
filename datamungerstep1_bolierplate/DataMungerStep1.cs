/////////////////////////////////////////////////////////////
// Created On: 2020-09-02
// 20200902 | Initial commit Step1 task1 completion
// 20200903 | Refractorings
/////////////////////////////////////////////////////////////

#region Usings
using DataMunger.Utilities;
using System.Collections.Generic;
using System.Linq;
#endregion

#region Namespace
namespace DataMunger
{
    #region Class
    public class DataMungerStep1
    {
        #region Split into words
        /// <summary>
        /// Method to split queries into list of words in it
        /// </summary>
        /// <param name="queryString">input string</param>
        /// <returns>List of words in query or null</returns>
        public static List<string> SplitQueryWords(string queryString)
        {
            List<string> queryResult;
            if (string.IsNullOrEmpty(queryString))
            {
                queryResult = null;
            }
            else
            {
                queryResult = queryString.Split(' ').ToList();
            }
            return queryResult;
        }
        #endregion

        #region Get File Name
        /// <summary>
        /// Method to get the file name from input query
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns>File name or null</returns>
        public static string GetFileNameFromQuery(string queryString)
        {
            return string.IsNullOrEmpty(queryString) ? null :
                queryString.Split(' ').FirstOrDefault(part => part.EndsWith(".csv"));
        }
        #endregion

        #region Base Part
        /// <summary>
        /// Method to get the base part of the wuery
        /// </summary>
        /// <param name="queryString">input query</param>
        /// <returns>Base part of string or null</returns>
        public static string GetBasePartFromQuery(string queryString)
        {
            if(Common.IsValidQueryBasic(queryString))
            {
                int index = Common.GetStringIndex(queryString, "where");
                if (index > -1)
                {
                    if (queryString.Split(' ').ToList().IndexOf("where") < 3)
                    {
                        return "Please enter a valid query!!!";
                    }
                    else
                    {
                        return queryString.Substring(0, index).Trim();
                    }
                }
                else
                {
                    return queryString.Trim();
                }
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
    #endregion
} 
#endregion
