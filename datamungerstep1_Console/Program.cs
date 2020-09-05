/////////////////////////////////////////////////////////////
// Created On: 2020-09-02
// 2020-09-02 | Initial Commit Part1 Step1 completed
// 2020-09-03 | Part1 Step2 Completed
// 2020-09-04 | Part1 step3 completed
// 2020-09-05 | Part1 all step completed
/////////////////////////////////////////////////////////////

#region Usings
using DataMunger;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ConsoleKeyInfo c;
            do
            {
                Console.Clear();
                string queryString = "";
                int operation = 0;

                Console.WriteLine();

                Console.WriteLine("Choose operation: ");
                Console.WriteLine("1. Split query");
                Console.WriteLine("2. Get file name");
                Console.WriteLine("3. Get base part");
                Console.WriteLine("4. Get selected fields");
                Console.WriteLine("5. Get filter part");
                Console.WriteLine("6. Get condition in filter part");
                Console.WriteLine("7. Get logical operators in filter part");
                Console.WriteLine("8. Get order by field");
                Console.WriteLine("9. Get group by field");
                Console.WriteLine("10. Get aggegate functions");
                Console.WriteLine();
                Console.WriteLine("-> ");

                try
                {
                    operation = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    operation = 0;
                }

                Console.WriteLine();
                Console.Write("Enter Input string: ");
                Console.WriteLine();
                queryString = Console.ReadLine();
                Console.WriteLine();

                switch (operation)
                {
                    case 1:
                        //Split the querseley into words
                        DisplaySplitResult(DataMungerStep1.SplitQueryWords(queryString));
                        break;

                    case 2:
                        //Get filename from query
                        DisplayResult(DataMungerStep1.GetFileNameFromQuery(queryString));
                        break;

                    case 3:
                        //Get base part of query
                        DisplayResult(DataMungerStep1.GetBasePartFromQuery(queryString));
                        break;

                    case 4:
                        //Get selected fields fo query
                        DisplaySplitResult(DataMungerStep2.GetSelectedFields(queryString));
                        break;

                    case 5:
                        //Get filter part
                        DisplayResult(DataMungerStep2.GetFilterPart(queryString));
                        break;

                    case 6:
                        //Get conditions in filter part
                        DisplaySplitResult(DataMungerStep2.GetConditionInFilter(queryString));
                        break;

                    case 7:
                        //Get logical operators in filter part
                        DisplaySplitResult(DataMungerStep3.GetLogicalOperators(queryString));
                        break;

                    case 8:
                        //Get order by fields
                        DisplaySplitResult(DataMungerStep3.GetOrderField(queryString));
                        break;

                    case 9:
                        //Get group by fields
                        DisplaySplitResult(DataMungerStep4.GetGroupByField(queryString));
                        break;

                    case 10:
                        //Get aggregate functions
                        DisplaySplitResult(DataMungerStep4.GetAggregateFunctions(queryString));
                        break;

                    default:
                        Console.WriteLine("Not a valid operation!!!");
                        break;
                }

                Console.WriteLine("\nPress any key to continue('esc' to stop)");
                c = Console.ReadKey(); 
            } while (c.Key != ConsoleKey.Escape);
        }

        /// <summary>
        /// Method to simply display output string
        /// </summary>
        /// <param name="queryResult"></param>
        static void DisplayResult(string queryResult)
        {
            if (string.IsNullOrEmpty(queryResult))
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
