/////////////////////////////////////////////////////////////
// Created On: 2020-09-02
// 2020-09-03 | Initial Commit Part1 Step2 Completed
// 2020-09-04 | Refractorings
// 2020-09-05 | Functionalities improved Part1 all step completed
// 2020-09-05 | Conditions part further modified
/////////////////////////////////////////////////////////////

#region Usings
using System.Collections.Generic;
using System.Linq;
using DataMunger.Utilities;
#endregion

#region Namespace
namespace DataMunger
{
    #region Class
    /// <summary>
    /// Class containing parts of step 2
    /// </summary>
    public class DataMungerStep2
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
                    ///select the part of query starting from select upto first from
                    ///(even if there is no from in query it will be handled later)
                    queryString = queryString.Trim().Substring(selectIndex + 6, fromIndex - 6);

                    ///if select keyword is found at first position and if there is a from in the query and
                    ///if there are no other select or from keyword between the first select and from
                    if (Common.StringMatchCount(queryString, "select") == Common.StringMatchCount(queryString, "from"))
                    {
                        //Get the strings between the select and from keyword as a list
                        queryResult = queryString.Split(',').Select(part => part.Trim()).ToList();

                        ///Checks whether there are any selected field ending with ',' and
                        ///whether there are any selected field keyword with space 
                        ///(a part that has space will be an assignment)
                        if (queryResult.Any(part => string.IsNullOrEmpty(part) ||
                            (part.Contains(' ') && !part.Contains('='))))
                        {
                            queryResult = null;
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
            }
            else
            {
                queryResult = null;
            }
            return queryResult;
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
            string queryResult = "";
            if (Common.IsValidQueryBasic(queryString))
            {
                /// get index after four letters of where word (5 as index starts from 0)
                int whereIndex = Common.GetStringIndex(queryString, "where", Common.Index.Last) + 5;

                ///a valid query will have a where clause minimum at 16th position.Eg: select * from t 
                if (whereIndex > 15)
                {
                    int endIndex = -1;
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
                        queryResult = null;
                    }
                    ///if 'order by' or 'group by' comes before 'where' keyword
                    ///either it means it has a subquery or the query is wrong
                    else if (endIndex > whereIndex)
                    {
                        //Getting the part of query between 'where' and 'orderby' or 'group by' clause
                        queryString = queryString.Substring(whereIndex, endIndex - whereIndex).Trim();

                        //if there is a bracket it means where keyword is inside a subquery and
                        //outer query only has order by which means there are no filters
                        if (queryString.Contains(")") ||
                            queryString.Length == 0)
                        {
                            queryResult = Common.NoFilterString;
                        }
                        else
                        {
                            queryString = Common.StringReplace(queryString, "where").Trim();
                            // minimum length for a filter without white space is 3 and with white space is 5 Eg: 1=3,1 = 3
                            if ((queryString.Length < 5 && queryString.Contains(" ")) ||
                                (queryString.Length < 3 && !queryString.Contains(" ")))
                            {
                                queryResult = null;
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
                        queryResult = null;
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
                queryResult = null;
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
                    queryResult = null;
                }
            }
            return queryResult;
        }
        #endregion
    }
    #endregion
}
#endregion
