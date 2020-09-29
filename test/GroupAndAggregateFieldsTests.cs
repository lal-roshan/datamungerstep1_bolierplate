/////////////////////////////////////////////////////////////
// Created On: 2020-09-04
// 2020-09-05 | Part1 all step completed
/////////////////////////////////////////////////////////////

using DataMunger;
using DataMunger.Exceptions;
using System;
using System.Collections.Generic;
using Xunit;

namespace test
{
    /// <summary>
    /// Class contatining test methods for step 3
    /// </summary>
    public class GroupAndAggregateFieldsTests
    {
        #region Group by fields
        /// <summary>
        /// Method to test getting group by field from valid query
        /// </summary>
        [Fact]
        public void QueryGroupByFieldTest()
        {
            string queryString = "select * from table1 where id =1 group by id, name order by name";
            List<string> queryResult = new List<string> { "id", "name" };
            Assert.Equal(queryResult, GroupAndAggregateFields.GetGroupByField(queryString));
        }

        [Theory]
        [InlineData("select * from table group by city where city = 'Delhi'")]
        public void InvalidQueryGroupByFieldTest(string queryString)
        {
            Exception ex = Assert.Throws<InvalidQueryException>(() =>
            GroupAndAggregateFields.GetGroupByField(queryString));
            Assert.Equal("Invalid use of group by clause!!", ex.Message);
        }
        #endregion

        #region Aggregate Functions
        /// <summary>
        /// Method to test fetting aggregate functions from valid query
        /// </summary>
        [Fact]
        public void QueryAggregateFunctionTest()
        {
            string queryString = "select avg(win_by_wickets),min(win_by_runs), sum(score) from ipl.csv";
            List<string> queryResult = new List<string> { "avg(win_by_wickets)", "min(win_by_runs)", "sum(score)" };
            Assert.Equal(queryResult, GroupAndAggregateFields.GetAggregateFunctions(queryString));
        }

        [Theory]
        [InlineData("select avg(win_by_wickets) sum(id) from ipl.csv")]
        [InlineData("select avg(win_by_wickets from ipl.csv")]
        public void InvalidQueryAggregateFunctionTest(string queryString)
        {
            Exception ex = Assert.Throws<InvalidQueryException>(() =>
            GroupAndAggregateFields.GetAggregateFunctions(queryString));
            Assert.Equal("Invalid aggregate function usage in query!!", ex.Message);
        }
        #endregion
    }
}
