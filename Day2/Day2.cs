using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day2
{
    public class Day2
    {
        public void Execute()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~         Day 2        -");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("");

            // Load text file
            string fileContent = File.ReadAllText("Day2Input.txt");
            // Format input to remove white space and any '+' characters
            fileContent = fileContent.Replace("+", "");
            // Split string into an array
            string[] fileContentSplit =
                fileContent.Split(new char[] { '\t', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            List<string> lines = new List<string>();
            lines = fileContentSplit.ToList();

            // Print answers
            Console.WriteLine("Finding checksum...");
            Console.WriteLine("Part 1 Answer:\t" + PartOne(lines).ToString());
            Console.WriteLine("Finding common letters...");
            Console.WriteLine("Part 2 Answer:\t" + PartTwo(lines).ToString());
        }

        private bool ContainsNumberDuplicates(string _input, int duplicatesRequired)
        {
            // Grabs all duplicate characters
            var duplicateChars = _input.GroupBy(x => x).Where(group => group.Count() == duplicatesRequired);

            return duplicateChars.Any();
        }

        private int PartOne(List<string> _input)
        {
            // Two counters for two and three letter instances
            int twoLettersCount = 0;
            int threeLettersCount = 0;
            // Increases counters dependent on if the line has 2 or 3 duplicates
            foreach (var i in _input)
            {
                if (ContainsNumberDuplicates(i, 2))
                    twoLettersCount++;
                if (ContainsNumberDuplicates(i, 3))
                    threeLettersCount++;
            }

            // Return checksum
            return twoLettersCount * threeLettersCount;
        }

        private string PartTwo(List<string> _input)
        {
            // Output string
            string output = "";
            // Counter
            int count = 0;
            
            // Loops through every line in input
            foreach (string line in _input)
            {
                // loops through every line in input not including first line
                foreach (string otherLine in _input.Skip(count))
                {
                    int difference = 0;
                    string lineRemovedDifferent = "";
                    
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] != otherLine[i])
                        {
                            difference++;
                        }
                        else
                        {
                            lineRemovedDifferent += line[i];
                        }

                        if(difference > 1)
                        {
                            break;
                        }
                    }


                    if(difference == 1)
                    {
                        output += lineRemovedDifferent;
                    }
                }

                count++;
            }



            return output;
        }
    }
}
