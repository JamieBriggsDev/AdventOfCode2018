using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SFML;
using SFML.Graphics;
using SFML.Window;

namespace Start
{

    class Day14
    {
        //public const int INPUT = 5;
        public const int TOTAL_ELFS = 2;
        public const int TOTAL_RECIPES = 9;
        public const int RECIPES_TO_FIND = 10;
        public const bool PRINT = true; 
        public class Elf
        {
            public int CurrentRecipe;
            public int ID;
            public Elf(int index)
            {
                CurrentRecipe = index;
                ID = index;
            }
        }

        public void Execute()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~         Day 14        -");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("");

            // Print answers
            Console.WriteLine("Finding best recipe...");
            PartOne();
            Console.WriteLine("Finding even better recipe...");
            PartTwo();
        }

        public void PrintDetails(List<int> _recipes, List<Elf> _elves)
        {
            if(PRINT)
            {
                int TotalRecipes = _recipes.Count;
                for(int i = 0; i < TotalRecipes; i++)
                {
                    Console.Write($"{_recipes[i]} ");
                }
                int minIndex = _elves.Min(a => a.CurrentRecipe);
                int maxIndex = _elves.Max(a => a.CurrentRecipe);
                var lowElf = _elves.Find(a => a.CurrentRecipe == minIndex);
                var highElf = _elves.Find(a => a.CurrentRecipe == maxIndex);
                // New line
                Console.WriteLine();
                for(int i = 0; i <= maxIndex; i++)
                {
                    if (i == minIndex)
                        Console.Write($"{lowElf.ID} ");
                    else if (i == maxIndex)
                        Console.Write($"{highElf.ID} ");
                    else
                        Console.Write("  ");
                }
                // New Line
                Console.WriteLine();

            }

        }


        public void PartOne()
        {
            List<Elf> Elves = new List<Elf>();
            for(int i = 0; i < TOTAL_ELFS; i++)
            {
                Elves.Add(new Elf(i));
            }

            List<int> Recipes = new List<int>();
            Recipes.Add(3);
            Recipes.Add(7);

            // Print Recipes
            PrintDetails(Recipes, Elves);


            while(Recipes.Count < TOTAL_RECIPES + RECIPES_TO_FIND)
            {

                //Console.Write($"Itteration: {i} => ");
                //foreach (var value in Recipes)
                //    Console.Write($"{value} ");
                //Console.WriteLine("");
                // Change recipe

                // Find sum
                int sum = 0;
                for(int j = 0; j < Elves.Count; j++)
                {
                    sum += Recipes[Elves[j].CurrentRecipe];
                    //Console.WriteLine($"{sum}");
                }


                // Place sum at end of recipes
                if (sum >= 10)
                {
                    Recipes.Add(sum / 10);
                }
                Recipes.Add(sum % 10);


                // Give new recipes
                for (int e = 0; e < Elves.Count; e++)
                {
                    // Add index, index value and 1
                    int value = Elves[e].CurrentRecipe + Recipes[Elves[e].CurrentRecipe] + 1;

                    Elves[e].CurrentRecipe = value % Recipes.Count;

                    //Console.WriteLine($"{e}:\t {Recipes[Elves[e].CurrentRecipe]} + 1 = {value}");
                }


            }

            // Display score of 10 recipes after trying recipes
            //for(int i = TOTAL_RECIPES; i < TOTAL_RECIPES + RECIPES_TO_FIND; i++)
            //{
            //    Console.Write($"{Recipes[i]}");
            //}
            foreach(var Recipe in Recipes.Skip(TOTAL_RECIPES).Take(RECIPES_TO_FIND))
            {
                Console.Write($"{Recipe}");
            }
            Console.WriteLine();

        }

        public void PartTwo()
        {
            List<Elf> Elves = new List<Elf>();
            for (int i = 0; i < TOTAL_ELFS; i++)
            {
                Elves.Add(new Elf(i));
            }

            int[] numbersToCheck = new int[] { 7, 9, 3, 0, 3, 1 };
            int index = 0;
            int positionToCheck = 0;
            bool found = false;

            List<int> Recipes = new List<int>();
            Recipes.Add(3);
            Recipes.Add(7);

            // Print Recipes
            PrintDetails(Recipes, Elves);


            while (!found)
            {

                // Find sum
                int sum = 0;
                for (int j = 0; j < Elves.Count; j++)
                {
                    sum += Recipes[Elves[j].CurrentRecipe];
                    //Console.WriteLine($"{sum}");
                }




                // Place sum at end of recipes
                if (sum >= 10)
                {
                    Recipes.Add(sum / 10);
                }
                Recipes.Add(sum % 10);


                // Give new recipes
                for (int e = 0; e < Elves.Count; e++)
                {
                    // Add index, index value and 1
                    int value = Elves[e].CurrentRecipe + Recipes[Elves[e].CurrentRecipe] + 1;

                    Elves[e].CurrentRecipe = value % Recipes.Count;

                    //Console.WriteLine($"{e}:\t {Recipes[Elves[e].CurrentRecipe]} + 1 = {value}");
                }

                // Check if input is in recipes
                while(index + positionToCheck < Recipes.Count)
                {
                
                    if( numbersToCheck[positionToCheck] == Recipes[index + positionToCheck])
                    {
                        if(positionToCheck == numbersToCheck.Length - 1)
                        {
                            found = true;
                            break;
                        }
                        positionToCheck++;
                    }
                    else
                    {
                        positionToCheck = 0;
                        index++;
                    }
                }


            }

            Console.WriteLine($"{index}");
            

        }
    }

}
