using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent
{
    public class Day3
    {
        public void Execute()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~         Day 3        -");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("");

            // Load text file
            string fileContent = File.ReadAllText("Input\\Day3Input.txt");
            // Format input to remove white space and any '+' characters
            fileContent = fileContent.Replace("+", "");
            // Split string into an array
            string[] fileContentSplit =
                fileContent.Split(new char[] { '\t', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            List<string> lines = new List<string>();
            lines = fileContentSplit.ToList();

            // Print answers
            Console.WriteLine("Finding overlaps...");
            Console.WriteLine("Part 1 Answer:\t" + PartOne(lines).ToString());
            Console.WriteLine("Finding fabric which doesnt overlap...");
            Console.WriteLine("Part 2 Answer:\t" + PartTwo(lines).ToString());
        }

        public int PartOne(List<string> _input)
        {
            // Predefine hash sets
            var coordinates = new HashSet<string>();
            var overlappedCoordinates = new HashSet<string>();


            foreach (var line in _input)
            {
                // Splits the line up into left, top, width and height
                var lineSplit = line.Split(new string[] { " @ ", ",", ": ", "x" }, StringSplitOptions.RemoveEmptyEntries);
                int left = int.Parse(lineSplit[1]);
                int top = int.Parse(lineSplit[2]);
                int width = int.Parse(lineSplit[3]);
                int height = int.Parse(lineSplit[4]);
                // Goes through every existing coordinate in the fabric rectangle
                for(var x = left; x < width + left; x++)
                {
                    for(var y = top; y < height + top; y++)
                    {
                        // Adds coordinate to the hashSet, if it already exists, it will
                        //  return false and the coordinate can then be added to the overlapped
                        //  coordinates
                        if (!coordinates.Add($"{x}x{y}"))
                        {
                            overlappedCoordinates.Add($"{x}x{y}");
                        }
                    }
                }
            }

            // Return total opverlapped coordinates
            return overlappedCoordinates.Count;
        }

        public int PartTwo(List<string> _input)
        {
            // Predefine hash sets
            var coordinates = new HashSet<string>();
            var overlappedCoordinates = new HashSet<string>();


            foreach (var line in _input)
            {
                // Splits the line up into left, top, width and height
                var lineSplit = line.Split(new string[] { " @ ", ",", ": ", "x", "#" }, StringSplitOptions.RemoveEmptyEntries); 
                int left = int.Parse(lineSplit[1]);
                int top = int.Parse(lineSplit[2]);
                int width = int.Parse(lineSplit[3]);
                int height = int.Parse(lineSplit[4]);

                // Goes through every existing coordinate in the fabric rectangle
                for (var x = left; x < width + left; x++)
                {
                    for (var y = top; y < height + top; y++)
                    {
                        // Adds coordinate to the hashSet, if it already exists, it will
                        //  return false and the coordinate can then be added to the overlapped
                        //  coordinates
                        if (!coordinates.Add($"{x}x{y}"))
                        {
                            //neverOverlapped = true;
                            overlappedCoordinates.Add($"{x}x{y}");
                        }
                    }
                }

            }

            // Now check every line again to see if any of its coordinates are
            //  within overlappedCoordinates
            foreach (var line in _input)
            {
                var lineSplit = line.Split(new string[] { " @ ", ",", ": ", "x", "#" }, StringSplitOptions.RemoveEmptyEntries);
                int ID = int.Parse(lineSplit[0]);
                int left = int.Parse(lineSplit[1]);
                int top = int.Parse(lineSplit[2]);
                int width = int.Parse(lineSplit[3]);
                int height = int.Parse(lineSplit[4]);

                // Flag to say if coordinate is already overlapped
                bool overlapped = false;
                // Goes through every existing coordinate in the fabric rectangle
                for (var x = left; x < width + left; x++)
                {
                    for (var y = top; y < height + top; y++)
                    {
                        // Adds coordinate to the hashSet, if it already exists, it will
                        //  return false and the coordinate can then be added to the overlapped
                        //  coordinates
                        if (overlappedCoordinates.Contains($"{x}x{y}"))
                        {
                            // Lets the loops know to break out of this line as the line
                            //  being checked overlaps therefore not the correct fabric
                            overlapped = true;
                            
                        }

                        // For speed efficiency, break out of loop
                        if (overlapped)
                            break;
                    }
                    // For speed efficiency, break out of loop
                    if (overlapped)
                        break;
                }

                // After this check, if the overlapped flag wasn't set to true,
                //  fabric has then been found
                if (!overlapped)
                    return ID;
            }


            // If there was a problem, return -1
            return -1;

        }
    }
}
