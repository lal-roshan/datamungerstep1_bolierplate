/////////////////////////////////////////////////////////////
// Created On: 2020-09-02
// 20200902 | Initial commit Step1 task1 completion
/////////////////////////////////////////////////////////////

using System.Collections.Generic;
using Xunit;
using DataMunger;
using System;
using DataMunger.Exceptions;

namespace test
{
    public class BasicQueryTests
    {
        #region QuerySplit
        /// <summary>
        /// Test for checking normal query string
        /// </summary>
        [Fact]
        public void QuerySplitWordsTest()
        {
            //string1: "select * from ipl.csv where season > 2014 and city = 'Bangalore'"
            //result1:  {"select", "*", "from", "ipl.csv", "where", "season", ">", "2014", "and", "city", "=", "'Bangalore'"}

            string queryString = "select * from ipl.csv where season > 2014 and city = 'Bangalore'";
            List<string> queryResult = new List<string>{"select", "*", "from", "ipl.csv", "where", "season", ">", "2014",
                                    "and", "city", "=", "'Bangalore'"};
            Assert.Equal(queryResult, BasicQueryOperations.SplitQueryWords(queryString));
        }

        /// <summary>
        /// Test for checking for empty or null query string for spliting
        /// </summary>
        /// <param name="queryString">input string</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("select from where")]
        public void SplitWordsQueryInvalidTest(string queryString)
        {
            Exception ex = Assert.Throws<InvalidQueryException>(() => BasicQueryOperations.SplitQueryWords(queryString));
            Assert.Equal(ex.Message, $"The query is invalid!!");
        }
        #endregion

        #region GetFileName
        /// <summary>
        /// Test to get file name from query
        /// </summary>
        [Fact]
        public void GetFileNameFromQuery()
        {
            string queryString = "select * from ipl.csv where season > 2014 and city = 'Bangalore'";
            string fileName = "ipl.csv";
            Assert.Equal(fileName, BasicQueryOperations.GetFileNameFromQuery(queryString));
        }

        /// <summary>
        /// Test for checking whether the input query is empty or null
        /// </summary>
        /// <param name="queryString">input string</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("select from where")]
        public void QueryInvalidTest(string queryString)
        {
            Exception ex = Assert.Throws<InvalidQueryException>(() => BasicQueryOperations.GetFileNameFromQuery(queryString));
            Assert.Equal(ex.Message, $"The query is invalid!!");
        }

        /// <summary>
        /// Test for checking whether the input query has a filename
        /// </summary>
        /// <param name="queryString">input string</param>
        [Fact]
        public void QueryNoFileTest()
        {
            string queryString = "select * from t where season > 2014 and city = 'Bangalore'";
            Assert.Equal(BasicQueryOperations.GetFileNameFromQuery(queryString), $"Query doesn't contain any file name");
        }
        #endregion

        #region GetFileName
        /// <summary>
        /// Test to get base part from query
        /// </summary>
        [Fact]
        public void GetBaseFromQuery()
        {
            string queryString = "select * from ipl.csv where season > 2014 and city = 'Bangalore'";
            string fileName = "select * from ipl.csv";
            Assert.Equal(fileName, BasicQueryOperations.GetBasePartFromQuery(queryString));
        }

        /// <summary>
        /// Test for checking whether the input query is empty or null
        /// </summary>
        /// <param name="queryString">input string</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("select from where")]
        public void QueryEmptyTestBase(string queryString)
        {
            Exception ex = Assert.Throws<InvalidQueryException>(() => BasicQueryOperations.GetBasePartFromQuery(queryString));
            Assert.Equal(ex.Message, $"The query is invalid!!");
        }
        #endregion
    }
}
