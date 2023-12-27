using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Day1
{
    public class Program
    {
        public static IDictionary<string,int> numMap { get; } = new Dictionary<string, int>()
                                                                   {
                                                                        {"one", 1},
                                                                        {"two", 2},
                                                                        {"three", 3},
                                                                        {"four", 4},
                                                                        {"five", 5},
                                                                        {"six", 6},
                                                                        {"seven", 7},
                                                                        {"eight", 8},
                                                                        {"nine", 9},
                                                                        {"1", 1},
                                                                        {"2", 2},
                                                                        {"3", 3},
                                                                        {"4", 4},
                                                                        {"5", 5},
                                                                        {"6", 6},
                                                                        {"7", 7}, 
                                                                        {"8", 8},
                                                                        {"9", 9}
                                                                    };

        public static void Main(string[] args)
        {
            Console.WriteLine("Total: " + FindNumber());
        }

        public static int FindNumber()
        {
            string? filePath = ConfigurationManager.AppSettings.Get(ConfigurationManager.AppSettings.Get("Current"));
            string[] input;
            try
            {
                input = File.ReadAllText(filePath).Split("\n");
            } 
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                return -1;
            }

            int firstNum;
            int secondNum;
            int total = 0;

            foreach (string dirtyLine in input) 
            {
                Console.WriteLine("dirty line: " + dirtyLine);
                MatchCollection matches = RegexFinder(dirtyLine.ToLower());
                foreach (Match match in matches)
                {
                    Console.WriteLine("Value: " + match.Groups[1].Value);
                }
                firstNum = numMap[matches[0].Groups[1].Value];
                secondNum = numMap[matches[matches.Count - 1].Groups[1].Value];

                Console.WriteLine("found num: " + firstNum + "" + secondNum);
                total += (firstNum * 10) + secondNum;
                Console.WriteLine("total: " + total  + "\n");
            }
            return total;
        }

        public static MatchCollection RegexFinder(string input)
        {
            string pattern = @"(?=(one|two|three|four|five|six|seven|eight|nine|\d))";
            return Regex.Matches(input,pattern);
        }
    }

}
