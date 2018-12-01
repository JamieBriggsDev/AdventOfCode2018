using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//--- Day 1: Chronal Calibration ---
//"We've detected some temporal anomalies," one of Santa's Elves at the Temporal Anomaly Research and Detection Instrument Station tells you. She sounded pretty worried when she called you down here. "At 500-year intervals into the past, someone has been changing Santa's history!"

//"The good news is that the changes won't propagate to our time stream for another 25 days, and we have a device" - she attaches something to your wrist - "that will let you fix the changes with no such propagation delay. It's configured to send you 500 years further into the past every few days; that was the best we could do on such short notice."

//"The bad news is that we are detecting roughly fifty anomalies throughout time; the device will indicate fixed anomalies with stars. The other bad news is that we only have one device and you're the best person for the job! Good lu--" She taps a button on the device and you suddenly feel like you're falling. To save Christmas, you need to get all fifty stars by December 25th.

//Collect stars by solving puzzles.Two puzzles will be made available on each day in the advent calendar; the second puzzle is unlocked when you complete the first.Each puzzle grants one star. Good luck!


//After feeling like you've been falling for a few minutes, you look at the device's tiny screen. "Error: Device must be calibrated before first use. Frequency drift detected. Cannot maintain destination lock." Below the message, the device shows a sequence of changes in frequency (your puzzle input). A value like +6 means the current frequency increases by 6; a value like -3 means the current frequency decreases by 3.

//For example, if the device displays frequency changes of +1, -2, +3, +1, then starting from a frequency of zero, the following changes would occur:

//Current frequency  0, change of +1; resulting frequency  1.
//Current frequency  1, change of -2; resulting frequency -1.
//Current frequency -1, change of +3; resulting frequency  2.
//Current frequency  2, change of +1; resulting frequency  3.
//In this example, the resulting frequency is 3.

//Here are other example situations:

//+1, +1, +1 results in  3
//+1, +1, -2 results in  0
//-1, -2, -3 results in -6
//Starting with a frequency of zero, what is the resulting frequency after all of the changes in frequency have been applied?

//Your puzzle answer was 406.

namespace Day1
{
    class Program
    {

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

        static int PartTwo(string[] _input)
        {
            // Define output
            int output = 0;
            // Preallocate list
            List<int> list = new List<int>();

            // Loop till repeat has been found
            while(true)
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


        static void Main(string[] args)
        {
            // Load text file
            string fileContent = File.ReadAllText("input.txt");
            // Format input to remove white space and any '+' characters
            fileContent = fileContent.Replace("+", "");
            // Split string into an array
            string[] fileContentSplit =
                fileContent.Split(new char[] { '\t', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            // Print answers
            Console.WriteLine("Resulting frequency:\t" + PartOne(fileContentSplit).ToString());
            Console.WriteLine("Part 2 Answer:\t" + PartTwo(fileContentSplit).ToString());

            // Pause program at the end
            Console.ReadKey();
        }
    }
}
