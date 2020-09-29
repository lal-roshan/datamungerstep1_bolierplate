/////////////////////////////////////////////////////////////
// Created On: 2020-09-02
// 2020-09-03 | Initial Commit Part1 Step2 Completed
// 2020-09-04 | Refractorings
// 2020-09-05 | Functionalities improved Part1 all step completed
// 2020-09-05 | Conditions part further modified
/////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Linq;
using DataMunger.Exceptions;
using DataMunger.Utilities;

namespace DataMunger
{
    /// <summary>
    /// Class containing parts of step 2
    /// </summary>
    public class QueryPartsOperations
    {

        #region Get Selected Fields
        /// <summary>
        /// Method to get selected fields forma input query
        /// </summary>
        /// <param name="queryString">input string</param>
        /// <returns>List of fields selected or null</returns>
        public static List<string> GetSelectedFields(string queryString)
        {
            List<string> queryResult = new List<string>();
            if (Common.IsValidQueryBasic(queryString))
            {
                ///Look whether the query starts with select and contains from
                int selectIndex = Common.GetStringIndex(queryString, "select");
                int fromIndex = Common.GetStringIndex(queryString, "from", Common.Index.Last);

                if ((selectIndex == 0) && (fromIndex > -1))
                {
                    ///select the part of query starting from select upto last from
                    queryString = queryString.Trim().Substring(selectIndex + 6, fromIndex - 6);

                    ///Get the strings between the select and from keyword as a list
                    queryResult = queryString.Split(',').Select(part => part.Trim()).ToList();

                    ///Checks whether there are any selected field ending with ','
                    ///and if there are no comma seperated fields and contains whitespace
                    if (queryResult.Any(part => string.IsNullOrEmpty(part)) ||
                        (queryResult.Count == 1 && (queryResult.First().Contains(' ') &&
                        !queryResult.First().Contains('('))))
                    {
                        throw new InvalidQueryException($"Error in selected fields parts of the query!!");
                    }
                    else
                    {
                        return queryResult;
                    }
                }
                else
                {
                    throw new InvalidQueryException($"Query without from clause is invalid!!");
                }
            }
            return null;
        }
        #endregion

        #region Filter part
        /// <summary>
        /// Method to get filer part form query
        /// </summary>
        /// <param name="queryString">input string</param>
        /// <returns></returns>
        public static string GetFilterPart(string queryString)
        {
            string queryResult = null;
            if (Common.IsValidQueryBasic(queryString))
            {
                /// get index after four letters of where word (5 as index starts from 0)
                int whereIndex = Common.GetStringIndex(queryString, "where", Common.Index.Last) + 5;

                ///a valid query will have a where clause minimum at 16th position.Eg: select * from t 
                if (whereIndex > 15)
                {
                    int endIndex;
                    int orderIndex = Common.GetStringIndex(queryString, "order by", Common.Index.Last);
                    int groupIndex = Common.GetStringIndex(queryString, "group by", Common.Index.Last);

                    ///get the index of whichever keyword that comes first 'order by' | 'group by'
                    if ((orderIndex > groupIndex && groupIndex == -1))
                    {
                        endIndex = orderIndex;
                    }
                    else
                    {
                        endIndex = groupIndex;
                    }

                    ///if the query contains both order by , group by clause and order by comes before group by
                    ///and if order by is not part of any subquery in that case the query is wrong as 
                    ///ORDER BY clause should be the last part of query
                    if (orderIndex > -1 && groupIndex > -1 &&
                        groupIndex > orderIndex && !queryString.Substring(orderIndex, groupIndex - orderIndex).Contains(")"))
                    {
                        throw new InvalidQueryException($"Invalid usage of group by or order by clause!!");
                    }
                    ///'order by' or 'group by' comes after 'where' keyword
                    else if (endIndex > whereIndex)
                    {
                        ///Getting the part of query between 'where' and 'orderby' or 'group by' clause
                        queryString = queryString.Substring(whereIndex, endIndex - whereIndex).Trim();

                        ///if there is a bracket it means where keyword is inside a subquery and
                        ///outer query only has order by which means there are no filters
                        if (queryString.Contains(")") ||
                            queryString.Length == 0)
                        {
                            queryResult = Common.NoFilterString;
                        }
                        else
                        {
                            queryString = Common.StringReplace(queryString, "where").Trim();
                            /// minimum length for a filter without white space is 3 and with white space is 5 Eg: 1=3,1 = 3
                            if ((queryString.Length < 5 && queryString.Contains(" ")) ||
                                (queryString.Length < 3 && !queryString.Contains(" ")))
                            {
                                throw new InvalidQueryException($"Invalid filter part in the query!!");
                            }
                            else
                            {
                                queryResult = queryString;
                            }
                        }
                    }
                    ///if 'order by' or 'group by' comes before 'where' keyword
                    ///either it means it has a subquery or the query is wrong
                    ///if it has a subquery there must be more than one 'select' keyword or else it is wrong
                    else if (endIndex == -1 ||
                            Common.StringMatchCount(queryString.Substring(0, endIndex), "select") > 1)
                    {
                        queryResult = queryString.Substring(whereIndex);
                    }
                    ///the query is wrong
                    else
                    {
                        throw new InvalidQueryException($"Invalid filter part in the query!!");
                    }
                }
                ///If query doesnt have a where clause
                ///(checking with 4 as we added 5 to the index of where
                ///and hence if it was -1 by adding 5 to it it becomes 4
                else if (whereIndex == 4)
                {
                    queryResult = Common.NoFilterString;
                }
                else
                {
                    throw new InvalidQueryException($"Invalid use of where clause in the query!!");
                }
            }
            return queryResult;
        }
        #endregion

        #region Conditions filter
        /// <summary>
        /// Method to get the conditions in the filter part of the query
        /// </summary>
        /// <param name="queryString">input query</param>
        /// <returns></returns>
        public static List<string> GetConditionInFilter(string queryString)
        {
            List<string> queryResult = new List<string>();

            ///Get the filter part of the query
            string queryFilter = GetFilterPart(queryString);

            ///if the query is not valid
            if (string.IsNullOrEmpty(queryFilter))
            {
                throw new InvalidQueryException("Invalid filter part in query!!");
            }
            ///if there is no filter part in the query
            else if (string.Equals(queryFilter, Common.NoFilterString))
            {
                queryResult.Add(Common.NoFilterString);
            }
            ///find the conditions in filter part
            else
            {
                ///Split the filter part of the query with 'and' and 'or' keywords and
                ///remove those keywords from resultant list
                queryResult = Common.SplitByString(queryFilter, "and, or, not",
                              Common.SplitType.RemoveThis);

                ///checking whether the conditions in filter part are valid
                if (queryResult != null &&
                    queryResult.Any(x => Common.SplitConditionWords(x) == null))
                {
                    throw new InvalidQueryException("Invalid conditions in query!!");
                }
            }
            return queryResult;
        }
        #endregion
    }
}
