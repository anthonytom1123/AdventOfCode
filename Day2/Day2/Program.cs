using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Drawing;

namespace Day2
{
    class Program
    {
        static void Main()
        {
            GetIdSum();
        }

        /*
         * fewest number of cubes of each color that could make the game possible
         * Find the sum of the power of the set
         * power of a set is equal to the numbers of red,green, and blue cubes multiplied together. (4 red * 2 green * 4 blue = 32 total)
         */
        public static void GetIdSum()
        {
            
            string? filePath = ConfigurationManager.AppSettings.Get(ConfigurationManager.AppSettings.Get("Current"));
            GameModel model;
            int total = 0;

            string[] input;
            try
            {
                input = File.ReadAllText(filePath).Split("\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                return;
            }

            foreach(string line in input)
            {
                Console.WriteLine($"Current Line: {line}");
                model = ParseGame(line);
                if (model.ErrorCode != 0) 
                {
                    Console.WriteLine(model.ErrorMessage);
                    return;
                }
                else
                {
                    total += model.minBlue * model.minGreen * model.minRed;
                    Console.WriteLine($"Is good. Total is {total}");
                }
            }
            Console.WriteLine($"Total is {total}");
        }

        public static GameModel ParseGame(string line)
        {
            GameModel model = new GameModel();
            model.ErrorCode = 0;
            int id;
            string[] idAndRounds = line.Split(": ");
            string[] rounds;
            if (!Int32.TryParse(idAndRounds[0].Split(" ")[1], out id))
            {
                model.ErrorCode = -1;
                model.ErrorMessage = $"[ERROR] Issue parsing id in ParseInput(): {line}";
                foreach(string splitLine in idAndRounds)
                {
                    Console.WriteLine($"id split is {splitLine}");
                }
                return model;
            }
            model.Id = id;
            rounds = idAndRounds[1].Split("; ");
            foreach(var round in rounds)
            {
                if(!ParseRound(round, model))
                {
                    break;
                }
            }
            return model;
        }

        public static bool ParseRound(string round, GameModel model)
        {
            string[] pickedCubes = round.Split(", ");
            int quantity;
            foreach(string pickedCube in pickedCubes) 
            {
                //parse cube (3 blue)
                Console.WriteLine($"Current cube: {pickedCube}");
                string[] parsedCube = pickedCube.Split(" ");
                if(Int32.TryParse(parsedCube[0], out quantity))
                {
                    FindMinMarbles(quantity, parsedCube[1].ToLower().Replace("\r", string.Empty), model);
                    if (!model.IsPossible) { return false; }
                }
                else
                {
                    model.ErrorCode = -1;
                    model.ErrorMessage = $"[ERROR] Issue parsing in ParseRound: {round}";
                    return false;
                }
            }
            return true;
        }

        public static void FindMinMarbles(int quantity, string color, GameModel model)
        {
            switch (color) 
            {
                case "red":
                    model.minRed = Math.Max(quantity, model.minRed);
                    break;
                case "blue":
                    model.minBlue = Math.Max(quantity, model.minBlue);
                    break;
                case "green":
                    model.minGreen = Math.Max(quantity, model.minGreen);
                    break;
                default:
                    model.IsPossible = false;
                    model.ErrorCode= -1;
                    model.ErrorMessage = $"[ERROR] FindMinMarbles: Color \"{color}\" is invalid.";
                    break;
            }
            model.IsPossible = true;

        }
    }
}
