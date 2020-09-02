#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using DataMunger;
#endregion

#region Namespace
namespace datamungerstep1_Console
{
    #region Class
    public class Program
    {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string queryString = "";
            Console.Write("Input string: ");
            queryString = Console.ReadLine();

            Console.WriteLine();

            /*//Split the query into words
            DisplaySplitResult(DataMungerStep1.SplitQueryWords(queryString));*/

            /*//Get filename from query
            DisplayResult(DataMungerStep1.GetFileNameFromQuery(queryString));*/

            //Get base part of query
            DisplayResult(DataMungerStep1.GetBasePartFromQuery(queryString));

            Console.WriteLine("\nPress any key to exit");
            Console.ReadLine();
        }

        /// <summary>
        /// Method to simply display output string
        /// </summary>
        /// <param name="queryResult"></param>
        static void DisplayResult(string queryResult)
        {
            if (String.IsNullOrEmpty(queryResult))
            {
                Console.WriteLine("Please enter a valid query!!!");
            }
            else
            {
                Console.WriteLine("Output string: " + queryResult);
            }
        }

        /// <summary>
        /// Method to display the list of words in the input query
        /// </summary>
        /// <param name="queryResult">List of words</param>
        static void DisplaySplitResult(List<string> queryResult)
        {
            if (queryResult == null)
            {
                Console.WriteLine("Please enter a valid query");
            }
            else
            {
                Console.WriteLine("Output string: " + queryResult.First());
                queryResult.RemoveAt(0);
                foreach (string part in queryResult)
                {
                    Console.WriteLine("\t\t" + part);
                }
            }
        }

    }
    #endregion
} 
#endregion
