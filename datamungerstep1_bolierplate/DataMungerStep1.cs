#region Usings
using System;
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
        /// <returns></returns>
        public static List<string> SplitQueryWords(string queryString)
        {
            List<string> queryResult;
            if (String.IsNullOrEmpty(queryString))
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
        /// <returns></returns>
        public static string GetFileNameFromQuery(string queryString)
        {
            return String.IsNullOrEmpty(queryString) ? null : queryString.Split(' ').FirstOrDefault(part => part.EndsWith(".csv"));
        }
        #endregion

        #region Base Part
        public static string GetBasePartFromQuery(string queryString)
        {
            if (String.IsNullOrEmpty(queryString))
            {
                return null;
            }
            else
            {
                int index = queryString.IndexOf("where", StringComparison.InvariantCultureIgnoreCase);
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
        }
        #endregion
    }
    #endregion
} 
#endregion
