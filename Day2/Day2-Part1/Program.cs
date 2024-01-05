using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Day2
{
    class Program
    {
        static void Main()
        {
            GetIdSum();
        }

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
                model = ParseInput(line);
                if (model.ErrorCode != 0) 
                {
                    Console.WriteLine(model.ErrorMessage);
                    return;
                }
                else if (model.IsPossible) 
                {
                    total += model.Id;
                    Console.WriteLine($"Is good. Total is {total}");
                }
            }
            Console.WriteLine($"Total is {total}");
        }

        public static GameModel ParseInput(string line)
        {
            /* Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
             * <Game ID>: <round 1>; <round 2> 
             */
            GameModel model = new GameModel();
            model.ErrorCode = 0;
            int id;
            string[] idAndRounds = line.Split(": ");
            string[] rounds;
            if (!Int32.TryParse(idAndRounds[0].Split(" ")[1], out id))
            {
                model.ErrorCode = -1;
                model.ErrorMessage = $"[ERROR] Issue parsing id in ParseInput(): {line}";
                foreach(string m in idAndRounds)
                {
                    Console.WriteLine($"id split is {m}");
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
            //3 blue, 4 red = split by ", "
            //if not possible, set model.IsPossible to false
            string[] pickedCubes = round.Split(", ");
            int quantity;
            foreach(string pickedCube in pickedCubes) 
            {
                //parse cube (3 blue)
                Console.WriteLine($"{pickedCube}");
                string[] parsedCube = pickedCube.Split(" ");
                if(Int32.TryParse(parsedCube[0], out quantity))
                {
                    model.IsPossible = IsRoundPossible(quantity, parsedCube[1].ToLower());
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

        public static bool IsRoundPossible(int num, string color)
        {
            int maxRed = 12;
            int maxGreen = 13;
            int maxBlue = 14;

            //Console.WriteLine($"num: {num} color: {color.TrimEnd()}");
            //Console.WriteLine($"red is {color.Equals("red")} green is {color.Equals("green")} blue is {color.Equals("blue")}");
            if(color.TrimEnd().Equals("red"))
            {
                return num <= maxRed;
            }
            else if (color.TrimEnd().Equals("blue"))
            {
                return num <= maxBlue;
            }
            else if (color.TrimEnd().Equals("green"))
            {
                return num <= maxGreen;
            }
            return true;
        }
    }
}
