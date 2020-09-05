/////////////////////////////////////////////////////////////
// Created On: 2020-09-04
// 2020-09-05 | Part1 all step completed
/////////////////////////////////////////////////////////////

#region Usings
using DataMunger;
using System.Collections.Generic;
using Xunit;
#endregion

#region Namespace
namespace test
{
    #region Class
    /// <summary>
    /// Class contatining test methods for step 3
    /// </summary>
    public class Step4Tests
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
            Assert.Equal(queryResult, DataMungerStep4.GetGroupByField(queryString));
        }

        [Theory]
        [InlineData("select * form")]
        [InlineData("select * from table group by city where city = 'Delhi'")]
        [InlineData("select * from table where city = 'Delhi' order by id group by id")]
        [InlineData("")]
        [InlineData(null)]
        public void InvalidQueryGroupByFieldTest(string queryString)
        {
            Assert.Null(DataMungerStep4.GetGroupByField(queryString));
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
            Assert.Equal(queryResult, DataMungerStep4.GetAggregateFunctions(queryString));
        }

        [Theory]
        [InlineData("select * form")]
        [InlineData("select * from table group by city where city = 'Delhi'")]
        [InlineData("select * from table where city = 'Delhi' order by id group by id")]
        [InlineData("select avg(win_by_wickets) where min(win_by_runs), sum(score) from ipl.csv")]
        [InlineData("")]
        [InlineData(null)]
        public void InvalidQueryAggregateFunctionTest(string queryString)
        {
            Assert.Null(DataMungerStep4.GetAggregateFunctions(queryString));
        }
        #endregion
    } 
    #endregion
} 
#endregion
