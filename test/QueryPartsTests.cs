/////////////////////////////////////////////////////////////
// Created On: 2020-09-02
// 2020-09-03 | Initial Commit Part1 Step2 Completed
/////////////////////////////////////////////////////////////

using DataMunger;
using DataMunger.Exceptions;
using System;
using System.Collections.Generic;
using Xunit;

namespace test
{
    /// <summary>
    /// Class contaitning test for step 2
    /// </summary>
    public class QueryPartsTests
    {
        #region SelectedFieldsTest
        /// <summary>
        /// Test for normal query selected fields selecter
        /// </summary>
        [Fact]
        public void QuerySelectedFieldsTests()
        {
            //string1 :"select city,winner,player_match from ipl.csv where season > 2014 and city = 'Bangalore'"
            //result1 : { "city", "winner", "player_match" }

            //string2 : "SELECT city,player From ipl.csv"
            //result 2: {"city", "player" }
            string queryString = "SELECT col1 = (select * from ipl.csv) From ipl.csv";
            List<string> queryResult = new List<string> { "col1 = (select * from ipl.csv)" };

            Assert.Equal(queryResult, QueryPartsOperations.GetSelectedFields(queryString));
        }

        /// <summary>
        /// Test for empty or null query selected fields selecter
        /// </summary>
        /// <param name="queryString">input string</param>
        [Theory]
        [InlineData("select from")]
        [InlineData("")]
        [InlineData(null)]
        public void InvalidQuerySelectedFieldsTests(string queryString)
        {
            Exception ex = Assert.Throws<InvalidQueryException>(() => QueryPartsOperations.GetSelectedFields(queryString));
            Assert.Equal(ex.Message, $"The query is invalid!!");
        }

        /// <summary>
        /// Test for invalid select clause in query
        /// </summary>
        /// <param name="queryString">input query</param>
        [Theory]
        [InlineData("select city, select from")]
        public void InvalidSelectFieldsTests(string queryString)
        {
            Exception ex = Assert.Throws<InvalidQueryException>(() => QueryPartsOperations.GetSelectedFields(queryString));
            Assert.Equal(ex.Message, $"Invalid use of select or from clause in the query!!");
        }

        /// <summary>
        /// Test for checking errors in select fields part
        /// </summary>
        /// <param name="queryString">input query</param>
        [Theory]
        [InlineData("select city, from")]
        [InlineData("select city name from")]
        public void InvalidSelectedFieldsTests(string queryString)
        {
            Exception ex = Assert.Throws<InvalidQueryException>(() => QueryPartsOperations.GetSelectedFields(queryString));
            Assert.Equal(ex.Message, $"Error in selected fields parts of the query!!");
        }
        #endregion

        #region Filter Part tests
        /// <summary>
        /// Method to test filter part return of valid query
        /// </summary>
        [Fact]
        public void QueryFilterPartTest()
        {
            //string 1: "select city from ipl.csv where season > 2014 or city = 'Bangalore' order by city"
            //result 1: "season > 2014 or city = 'Bangalore'"
            string queryString = "Select city1 from city where city = 'Banglore' group by city order by city";
            string queryResult = "city = 'Banglore'";
            Assert.Equal(queryResult, QueryPartsOperations.GetFilterPart(queryString));
        }

        /// <summary>
        /// Method to test getting filter part of invalid query or empty
        /// </summary>
        /// <param name="queryString">input query</param>
        [Theory]
        [InlineData("Select city1 from city where city = 'Banglore' order by city group by city")]
        public void InvalidQueryFilterPartTest(string queryString)
        {
            Exception ex = Assert.Throws<InvalidQueryException>(() => QueryPartsOperations.GetFilterPart(queryString));
            Assert.Equal(ex.Message, $"Invalid use of order by or group by clause in the query!!");
        }

        /// <summary>
        /// test for invalid group by or order by in query
        /// </summary>
        /// <param name="queryString">input query</param>
        [Theory]
        [InlineData("Select city1 from order by city where city = 'Banglore'")]
        [InlineData("Select city1 from group by city where city = 'Banglore'")]
        public void InvalidOrderGroupByTest(string queryString)
        {
            Exception ex = Assert.Throws<InvalidQueryException>(() => QueryPartsOperations.GetFilterPart(queryString));
            Assert.Equal(ex.Message, $"Invalid filter part in the query!!");
        }

        /// <summary>
        /// test for invalid where part
        /// </summary>
        /// <param name="queryString">input query</param>
        [Theory]
        [InlineData("Select city where from")]
        public void InvalidWhereFilterPartTest(string queryString)
        {
            Exception ex = Assert.Throws<InvalidQueryException>(() => QueryPartsOperations.GetFilterPart(queryString));
            Assert.Equal(ex.Message, $"Invalid use of where clause in the query!!");
        }
        #endregion

        #region Condition
        /// <summary>
        /// Method to test getting filter part of query
        /// </summary>
        [Fact]
        public void QueryFilterConditionTest()
        {
            string queryString = "select * from ipl.csv where season > 2014 and city = 'Bangalore'";
            List<string> queryResult = new List<string> { "season > 2014", "city = 'Bangalore'" };
            Assert.Equal(queryResult, QueryPartsOperations.GetConditionInFilter(queryString));
        }

        /// <summary>
        /// Method to test getting filter part of invalid query or empty
        /// </summary>
        /// <param name="queryString"></param>
        [Theory]
        [InlineData("Select city1 from city where city")]
        [InlineData("Select city1 from city where city, id")]
        public void InvalidQueryConditionFilterPartTest(string queryString)
        {
            Exception ex = Assert.Throws<InvalidQueryException>(() => QueryPartsOperations.GetConditionInFilter(queryString));
            Assert.Equal("Invalid conditions in query!!", ex.Message);
        } 
        #endregion

    }
}
