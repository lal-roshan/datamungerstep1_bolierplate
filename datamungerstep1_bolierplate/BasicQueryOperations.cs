/////////////////////////////////////////////////////////////
// Created On: 2020-09-02
// 2020-09-02 | Initial commit Step1 task1 completion
// 2020-09-03 | Refractorings
// 2020-09-05 | Functionalities improved Part1 all step completed
/////////////////////////////////////////////////////////////

using DataMunger.Exceptions;
using DataMunger.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace DataMunger
{
    public class BasicQueryOperations
    {
        #region Split into words
        /// <summary>
        /// Method to split queries into list of words in it
        /// </summary>
        /// <param name="queryString">input string</param>
        /// <returns>List of words in query or null</returns>
        public static List<string> SplitQueryWords(string queryString)
        {
            List<string> queryResult = null;
            if (Common.IsValidQueryBasic(queryString))
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
            if (Common.IsValidQueryBasic(queryString))
            {
                queryString = queryString.Split(' ').FirstOrDefault(part => part.EndsWith(".csv"));
                if (string.IsNullOrEmpty(queryString))
                {
                    return Common.NoFileName;
                }
                return queryString;
            }
            return null; ;
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
            if (Common.IsValidQueryBasic(queryString))
            {
                int index = -1;

                int whereIndex = Common.GetStringIndex(queryString, "where", Common.Index.Last);
                if (whereIndex > -1)
                {
                    index = whereIndex;
                }
                else
                {
                    int groupIndex = Common.GetStringIndex(queryString, "group by", Common.Index.Last);
                    if (groupIndex > -1)
                    {
                        index = groupIndex;
                    }
                    else
                    {
                        int orderIndex = Common.GetStringIndex(queryString, "order by", Common.Index.Last);
                        if (orderIndex > -1)
                        {
                            index = orderIndex;
                        }
                    }
                }
                ///if the query contains either of where, order by or group by clauses
                ///
                if (index > -1)
                {
                    queryString = queryString.Substring(0, index).Trim();

                    if ((Common.StringMatchCount(queryString, "where") > 0 ||
                       Common.StringMatchCount(queryString, "order by") > 0 ||
                       Common.StringMatchCount(queryString, "group by") > 0) &&
                       (!queryString.Contains('(') && !queryString.Contains(')')))
                    {
                        throw new InvalidQueryException($"Invalid use of where, order by or group by clause!!");
                    }
                    else
                    {
                        return queryString; ;
                    }
                }
                else
                {
                    return queryString.Trim();
                }
            }
            return null;
        }
        #endregion
    }
}