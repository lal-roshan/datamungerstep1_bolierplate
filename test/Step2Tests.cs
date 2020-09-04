/////////////////////////////////////////////////////////////
// Created On: 2020-09-02
// 2020-09-03 | Initial Commit Part1 Step2 Completed
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
    /// Class contaitning test for step 2
    /// </summary>
    public class Step2Tests
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

            Assert.Equal(queryResult, DataMungerStep2.GetSelectedFields(queryString));
        }

        /// <summary>
        /// Test for empty or null query selected fields selecter
        /// </summary>
        /// <param name="queryString">input string</param>
        [Theory]
        [InlineData("select from")]
        [InlineData("select city, select from")]
        [InlineData("select city, from")]
        [InlineData("select city name from")]
        [InlineData("select city name, place_name from")]
        [InlineData(null)]
        [InlineData("")]
        public void InvalidQuerySelectedFieldsTests(string queryString)
        {
            Assert.Null(DataMungerStep2.GetSelectedFields(queryString));
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
            Assert.Equal(queryResult, DataMungerStep2.GetFilterPart(queryString));
        }

        /// <summary>
        /// Method to test getting filter part of invalid query or empty
        /// </summary>
        /// <param name="queryString"></param>
        [Theory]
        [InlineData("Select city where from")]
        [InlineData("Select city1 from order by city where city = 'Banglore'")]
        [InlineData("Select city1 from group by city where city = 'Banglore'")]
        [InlineData("Select city1 from city where city = 'Banglore' order by city group by city")]
        [InlineData("")]
        public void InvalidQueryFilterPartTest(string queryString)
        {
            Assert.Null(DataMungerStep2.GetFilterPart(queryString));
        }
        #endregion

        #region Filter Part
        /// <summary>
        /// Method to test getting filter part of query
        /// </summary>
        [Fact]
        public void QueryFilterConditionTest()
        {
            string queryString = "select * from ipl.csv where season > 2014 and city = 'Bangalore'";
            List<string> queryResult = new List<string> { "season > 2014", "city = 'Bangalore'" };
            Assert.Equal(queryResult, DataMungerStep2.GetConditionInFilter(queryString));
        }

        /// <summary>
        /// Method to test getting filter part of invalid query or empty
        /// </summary>
        /// <param name="queryString"></param>
        [Theory]
        [InlineData("Select city where from")]
        [InlineData("Select city1 from order by city where city = 'Banglore'")]
        [InlineData("Select city1 from group by city where city = 'Banglore'")]
        [InlineData("Select city1 from city where city = 'Banglore' order by city group by city")]
        [InlineData("")]
        public void InvalidQueryConditionFilterPartTest(string queryString)
        {
            Assert.Null(DataMungerStep2.GetConditionInFilter(queryString));
        } 
        #endregion

    }
    #endregion
}
#endregion
