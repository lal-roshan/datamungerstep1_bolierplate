/////////////////////////////////////////////////////////////
// Created On: 2020-09-04
// 2020-09-05 | Part1 all step completed
/////////////////////////////////////////////////////////////

#region Usings
using DataMunger.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

#region Namespace
namespace DataMunger
{
    #region  Class
    /// <summary>
    /// Classs containing methods for step 4
    /// </summary>
    public class DataMungerStep4
    {
        #region Group field
        /// <summary>
        /// Method to get group by fields from a query
        /// </summary>
        /// <param name="queryString">Input query</param>
        /// <returns>returns list of fields used in group by clause if any
        /// or message if there is no order by clause or null if query is invalid</returns>
        public static List<string> GetGroupByField(string queryString)
        {
            List<string> queryResult = new List<string>();
            if (Common.IsValidQueryBasic(queryString))
            {
                ///Split the query based on group by clause
                queryResult = Common.SplitByString(queryString, "group by");

                ///If the split result count is one, it means there is no group by clause in it
                if (queryResult?.Count == 1)
                {
                    queryResult.Clear();
                    queryResult.Add(Common.NoGroupByClause);
                }
                else if (queryResult?.Count > 2)
                {
                    ///If the second last item in list is group by, for valid query the last item
                    ///in the list will be the group by fields
                    if (string.Equals(queryResult[queryResult.Count - 2], "group by",
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        /// checks whether the group by part contains any where clause
                        if (Common.StringMatchCount(queryResult.Last(), "where") > 0)
                        {
                            ///If last part contains where and ')' it means the only group by in the string is
                            ///a part of substring and hence no group by in base query
                            if (Common.IsPartOfSubQuery(queryResult.Last()))
                            {
                                queryResult.Clear();
                                queryResult.Add(Common.NoBaseGroupByClause);
                            }
                            ///If group by isnt a part of subquery and where clause is present at last part, then the query is invalid
                            else
                            {
                                queryResult = null;
                            }
                        }
                        ///Checks whether the lsat part contains a ')' but no '(', implying that the group by is in a substring
                        else if (Common.IsPartOfSubQuery(queryResult.Last()) &&
                                !queryResult.Last().Contains('('))
                        {
                            queryResult.Clear();
                            queryResult.Add(Common.NoBaseGroupByClause);
                        }
                        else
                        {
                            ///If there is a order by clause after group by then split the string at order by
                            ///so that we get the group by field only
                            string endPart = Common.SplitByString(queryResult.Last(), "order by")?.First();
                            if (!string.IsNullOrEmpty(endPart))
                            {
                                if (char.Equals(endPart.Last(), ')'))
                                {
                                    endPart = endPart.Trim(')');
                                }
                                ///Get all the coma separated group by fields
                                queryResult = endPart.Split(',').Select(x => x.Trim()).ToList();
                            }
                        }
                    }
                    else if (queryResult != null)
                    {
                        queryResult.Clear();
                        queryResult.Add(Common.NoBaseGroupByClause);
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

        #region Aggregate functions
        /// <summary>
        /// Method to get the aggregate functions from query
        /// </summary>
        /// <param name="queryString">Input query</param>
        /// <returns>List of aggregate functions in query</returns>
        public static List<string> GetAggregateFunctions(string queryString)
        {
            List<string> queryResult = new List<string>();
            if (Common.IsValidQueryBasic(queryString))
            {
                ///Get the base part of query as the aggregate functions on selected fields are to be found
                string basePart = DataMungerStep1.GetBasePartFromQuery(queryString);
                if (!string.IsNullOrEmpty(basePart))
                {
                    ///Create coma seperated pattern string for each aggregate function
                    StringBuilder aggregatePatterns = new StringBuilder();
                    aggregatePatterns.Append("avg\\([a-zA-Z0-9_]+\\),");
                    aggregatePatterns.Append("min\\([a-zA-Z0-9_]+\\),");
                    aggregatePatterns.Append("max\\([a-zA-Z0-9_]+\\),");
                    aggregatePatterns.Append("count\\([a-zA-Z0-9_]+\\),");
                    aggregatePatterns.Append("sum\\([a-zA-Z0-9_]+\\)");

                    ///Splits the actual query into parts seperated by aggregate functions if any
                    ///and get only the parts that are aggregate functions
                    queryResult = Common.SplitByString(basePart, aggregatePatterns.ToString(),
                                  Common.SplitType.RemoveAllButThis, true);


                    if (queryResult != null && queryResult.Count == 0)
                    {
                        queryResult.Clear();
                        queryResult.Add(Common.NoAggregateFunctions);
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
