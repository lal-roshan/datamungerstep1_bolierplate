/////////////////////////////////////////////////////////////
// Created On: 2020-09-03
// 2020-09-04 | Initial commit part1 step 3 completed
// 2020-09-05 | Functionalities improved Part1 all step completed
/////////////////////////////////////////////////////////////

using DataMunger.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMunger
{
    /// <summary>
    /// Class containing step 3 of part 1
    /// </summary>
    public class LogicalAndOrderFields
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
                string filterPart = QueryPartsOperations.GetFilterPart(queryString);
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
                    ///Splits the filter part based on operators and removes all other part
                    queryResult = Common.SplitByString(filterPart, "and, or, not",
                                  Common.SplitType.RemoveAllButThis);
                    if (queryResult != null && queryResult.Count == 0)
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
        #endregion

        #region Order Field
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
                ///split result with count 1 means there was nt order by clause in the query
                if (queryResult?.Count == 1)
                {
                    queryResult.Clear();
                    queryResult.Add(Common.NoOrderByClause);
                }
                else if (queryResult?.Count > 2)
                {
                    ///Check whether second last part is order by, if yes then last part will be the order by field
                    ///for valid queries
                    if (string.Equals(queryResult[queryResult.Count - 2], "order by",
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        ///if the part after order by contains where or group by and if they are not
                        ///in substring , then query is invalid
                        if (Common.StringMatchCount(queryResult.Last(), "where") > 0 ||
                            Common.StringMatchCount(queryResult.Last(), "group by") > 0)
                        {
                            if (Common.IsPartOfSubQuery(queryResult.Last()))
                            {
                                queryResult.Clear();
                                queryResult.Add(Common.NoBaseOrderByClause);
                            }
                            else
                            {
                                queryResult = null;
                            }
                        }
                        ///if last part is part of subquery
                        else if (Common.IsPartOfSubQuery(queryResult.Last()) &&
                                !queryResult.Last().Contains('('))
                        {
                            queryResult.Clear();
                            queryResult.Add(Common.NoBaseOrderByClause);
                        }
                        ///else the last part will be the order by field
                        else
                        {
                            queryResult = queryResult.Last().Split(',').Select(x => x.Trim()).ToList();
                        }
                    }
                    ///If second last part of query is not order by it won't be in base query
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
}
