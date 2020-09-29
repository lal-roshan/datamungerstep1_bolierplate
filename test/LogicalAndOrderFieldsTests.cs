/////////////////////////////////////////////////////////////
// Created On: 2020-09-03
// 2020-09-04 | Initial commit part1 step 3 completed
/////////////////////////////////////////////////////////////

using DataMunger;
using DataMunger.Exceptions;
using System;
using System.Collections.Generic;
using Xunit;

namespace test
{
    /// <summary>
    /// Class containing test methods for step3 of part1
    /// </summary>
    public class LogicalAndOrderFieldsTests
    {
        #region Logical Operators
        /// <summary>
        /// Method to test getting of logical operators from valid query
        /// </summary>
        [Fact]
        public void QueryLogicalOperatorsTest()
        {
            string queryString = "select * from table1 where col = 'Delhi' and area > 10000 or population < 500";
            List<string> queryResult = new List<string> { "and", "or" };
            Assert.Equal(queryResult, LogicalAndOrderFields.GetLogicalOperators(queryString));
        }

        /// <summary>
        /// Method to test getting of logical operators from invalid query
        /// </summary>
        /// <param name="queryString"></param>
        [Theory]
        [InlineData("select * from table1 where col")]
        [InlineData("select * from table1 where col = 'Delhi' if area > 10000  ")]
        public void InvalidQueryLogicalOperatorsTest(string queryString)
        {
            Exception ex = Assert.Throws<InvalidQueryException>(() => LogicalAndOrderFields.GetLogicalOperators(queryString));
            Assert.Equal("Invalid conditions in query!!", ex.Message);
        }
        #endregion

        #region Order Fields
        /// <summary>
        /// Method to test getting order by fields from valid query
        /// </summary>
        [Fact]
        public void QueryOrderFieldTest()
        {
            string queryString = "select * from ipl.csv where season > 2016 and city= 'Bangalore' order by win_by_runs, city";
            List<string> queryResult = new List<string> { "win_by_runs", "city" };
            Assert.Equal(queryResult, LogicalAndOrderFields.GetOrderField(queryString));
        }

        [Theory]
        [InlineData("select * from table order by city where city = 'Delhi'")]
        public void InvalidQueryOrderFieldTest(string queryString)
        {
            Exception ex = Assert.Throws<InvalidQueryException>(() => LogicalAndOrderFields.GetOrderField(queryString));
            Assert.Equal("Invalid use of order by clause!!", ex.Message);
        } 
        #endregion
    }
}