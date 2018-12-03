using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day1
{
    public class Day1
    {
        public void Execute()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~         Day 1        -");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("");

            // Load text file
            string fileContent = File.ReadAllText("Day1Input.txt");
            // Format input to remove white space and any '+' characters
            fileContent = fileContent.Replace("+", "");
            // Split string into an array
            string[] fileContentSplit =
                fileContent.Split(new char[] { '\t', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            // Print answers
            Console.WriteLine("Adding up frequencys...");
            Console.WriteLine("Part 1 Answer:\t" + PartOne(fileContentSplit).ToString());
            Console.WriteLine("Finding repeated frequencys...");
            Console.WriteLine("Part 2 Answer:\t" + PartTwo(fileContentSplit).ToString());
        }

        // Solution to Part One
        static int PartOne(string[] _input)
        {
            // Define output
            int output = 0;

            // Loop through each index in _input
            foreach (string i in _input)
            {
                // Add value to output
                output += int.Parse(i);
            }

            // Return output
            return output;
        }

        // Solution to Part Two
        static int PartTwo(string[] _input)
        {
            // Define output
            int output = 0;
            // Preallocate list
            List<int> list = new List<int>();

            // Loop till repeat has been found
            while (true)
            {
                // Loop through each index in _input
                foreach (string i in _input)
                {
                    // Add value to output
                    output += int.Parse(i);

                    // Check if list contains output
                    if (list.Contains(output))
                    {
                        // Return output as this is our answer
                        return output;
                    }

                    // Add output to list
                    list.Add(output);
                }
            }
        }
    }


}
