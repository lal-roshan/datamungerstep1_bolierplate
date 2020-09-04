﻿/////////////////////////////////////////////////////////////
// Created On: 2020-09-03
// 2020-09-04 | Initial commit part1 step 3 completed
/////////////////////////////////////////////////////////////

#region Using
using DataMunger.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

#region Namespace
namespace DataMunger
{
    #region Class
    /// <summary>
    /// Class containing step 3 of part 1
    /// </summary>
    public class DataMungerStep3
    {
        #region Logical Operators
        /// <summary>
        /// Method to get the logical operators used in filter part
        /// </summary>
        /// <param name="queryString">Input query/param>
        /// <returns>List of all logical operators used in the query</returns>
        public static List<string> GetLogicalOperators(string queryString)
        {
            List<string> queryResult = new List<string>();
            if (Common.IsValidQueryBasic(queryString))
            {
                string filterPart = DataMungerStep2.GetFilterPart(queryString);
                if (string.IsNullOrEmpty(filterPart))
                {
                    queryResult = null;
                }
                else if (string.Equals(filterPart, Common.NoFilterString))
                {
                    queryResult.Add(filterPart);
                }
                else
                {
                    queryResult = Common.SplitByString(filterPart, "and, or, not",
                                  Common.SplitType.RemoveAllButThis);
                    if (queryResult.Count == 0)
                    {
                        queryResult.Add(Common.NoLogicalOperatorsString);
                    }
                }
            }
            else
            {
                queryResult = null;
            }
            return queryResult;
        }

        /// <summary>
        /// Method to get fields used in order clause
        /// </summary>
        /// <param name="queryString">input query</param>
        /// <returns>returns list of fields used in order clause if any
        /// or message if there is no order by clause or null if query is invalid</returns>
        public static List<string> GetOrderField(string queryString)
        {
            List<string> queryResult = new List<string>();
            if (Common.IsValidQueryBasic(queryString))
            {
                queryResult = Common.SplitByString(queryString, "order by");
                if (queryResult.Count == 1)
                {
                    queryResult.Clear();
                    queryResult.Add(Common.NoOrderByClause);
                }
                else if (queryResult.Count > 2)
                {
                    if (string.Equals(queryResult[queryResult.Count - 2], "order by",
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (Common.StringMatchCount(queryResult.Last(), "where") > 0 ||
                            Common.StringMatchCount(queryResult.Last(), "group by") > 0)
                        {
                            if (queryResult.Last().Contains(')'))
                            {
                                queryResult.Clear();
                                queryResult.Add(Common.NoBaseOrderByClause);
                            }
                            else
                            {
                                queryResult = null;
                            }
                        }
                        else if (queryResult.Last().Contains(')') &&
                                !queryResult.Last().Contains('('))
                        {
                            queryResult.Clear();
                            queryResult.Add(Common.NoBaseOrderByClause);
                        }
                        else
                        {
                            queryResult = queryResult.Last().Split(',').Select(x => x.Trim()).ToList();
                        }
                    }
                    else
                    {
                        queryResult.Clear();
                        queryResult.Add(Common.NoBaseOrderByClause);
                    }
                }
                else
                {
                    queryResult = null;
                }
            }
            else
            {
                queryResult = null;
            }
            return queryResult;
        }
        #endregion
    }
    #endregion
}
#endregion
