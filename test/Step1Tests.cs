/////////////////////////////////////////////////////////////
// Created On: 2020-09-02
// 20200902 | Initial commit Step1 task1 completion
/////////////////////////////////////////////////////////////

#region Usings
using System.Collections.Generic;
using Xunit;
using DataMunger;
#endregion

#region Namespace
namespace test
{
    #region Class
    public class Step1Tests
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
        public void SplitWordsQueryEmptyTest(string queryString)
        {
            Assert.Null(BasicQueryOperations.SplitQueryWords(queryString));
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
        public void QueryEmptyTest(string queryString)
        {
            Assert.Null(BasicQueryOperations.GetFileNameFromQuery(queryString));
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
        public void QueryEmptyTestBase(string queryString)
        {
            Assert.Null(BasicQueryOperations.GetBasePartFromQuery(queryString));
        }
        #endregion
    }
    #endregion
} 
#endregion
