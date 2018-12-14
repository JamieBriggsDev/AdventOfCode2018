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


namespace Start
{

    class Day12
    { 

        public void Execute()
        {
            // Generate random

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~         Day 12        -");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("");

            // Load text file
            string fileContent = File.ReadAllText("Input\\Day12Input.txt");
            // Format input to remove white space and any '+' characters
            fileContent = fileContent.Replace("+", "");
            // Split string into an array
            string[] fileContentSplit =
                fileContent.Split(new char[] { '\t', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            List<string> lines = new List<string>();
            lines = fileContentSplit.ToList();

            // Print answers
            Console.WriteLine("Finding 20 generations of plants...");
            PartOne(lines);
            Console.WriteLine("Finding 50000000000 generations of plants...");
            PartTwo(lines);
        }


        private void PartOne(List<string> parts)
        {
            Dictionary<string, string> rules = new Dictionary<string, string>();
            for(int i = 1; i < parts.Count; i++)
            {
                string[] temp = parts[i].Split(new char[] { ' ', '=', '>' }, StringSplitOptions.RemoveEmptyEntries);
                rules.Add(temp[0], temp[1]);
            }

            string currentGeneration = parts[0].Substring(15);

            long maxGens = 20;
            int currLeft = 0;
            long score = 0, lastScore = 0;
            long diff = 0, prevDiff = 0;

            for (int gen = 1; gen <= maxGens; gen++)
            {
                StringBuilder nextGen = new StringBuilder();
                for(int pos = -2; pos < currentGeneration.Length + 2; pos++)
                {
                    string state = string.Empty;
                    int distanceFromEnd = currentGeneration.Length - pos;
                    if (pos <= 1)
                        state = new string('.', 2 - pos) + currentGeneration.Substring(0, 3 + pos);
                    else if (distanceFromEnd <= 2)
                        state = currentGeneration.Substring(pos - 2, distanceFromEnd + 2) + new string('.', 3 - distanceFromEnd);
                    else
                        state = currentGeneration.Substring(pos - 2, 5);

                    nextGen.Append(rules.TryGetValue(state, out string newState) ? newState : ".");
                }

                currentGeneration = nextGen.ToString();
                currLeft -= 2;

                score = 0;
                for(int pos = 0; pos < currentGeneration.Length; pos++)
                {
                    score += currentGeneration[pos].ToString() == "." ? 0 : pos + currLeft;
                }
                diff = score - lastScore;
                if(diff == prevDiff)
                {
                    score += (maxGens - (long)gen) * prevDiff;
                    break;
                }
                prevDiff = diff;
                lastScore = score;
            }

            Console.WriteLine(score);
        }

        private void PartTwo(List<string> parts)
        {
            Dictionary<string, string> rules = new Dictionary<string, string>();
            for (int i = 1; i < parts.Count; i++)
            {
                string[] temp = parts[i].Split(new char[] { ' ', '=', '>' }, StringSplitOptions.RemoveEmptyEntries);
                rules.Add(temp[0], temp[1]);
            }

            string currentGeneration = parts[0].Substring(15);

            long maxGens = 50000000000;
            int currLeft = 0;
            long score = 0, lastScore = 0;
            long diff = 0, prevDiff = 0;

            for (int gen = 1; gen <= maxGens; gen++)
            {
                StringBuilder nextGen = new StringBuilder();
                for (int pos = -2; pos < currentGeneration.Length + 2; pos++)
                {
                    string state = string.Empty;
                    int distanceFromEnd = currentGeneration.Length - pos;
                    if (pos <= 1)
                        state = new string('.', 2 - pos) + currentGeneration.Substring(0, 3 + pos);
                    else if (distanceFromEnd <= 2)
                        state = currentGeneration.Substring(pos - 2, distanceFromEnd + 2) + new string('.', 3 - distanceFromEnd);
                    else
                        state = currentGeneration.Substring(pos - 2, 5);

                    nextGen.Append(rules.TryGetValue(state, out string newState) ? newState : ".");
                }

                currentGeneration = nextGen.ToString();
                currLeft -= 2;

                score = 0;
                for (int pos = 0; pos < currentGeneration.Length; pos++)
                {
                    score += currentGeneration[pos].ToString() == "." ? 0 : pos + currLeft;
                }
                diff = score - lastScore;
                if (diff == prevDiff)
                {
                    score += (maxGens - (long)gen) * prevDiff;
                    break;
                }
                prevDiff = diff;
                lastScore = score;
            }

            Console.WriteLine(score);
        }
    }
}
