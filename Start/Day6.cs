using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Start
{
    public class Vector2D
    {
        public int ID;
        public int X;
        public int Y;

        public int TempDistance;

        public Vector2D(int id, int x, int y)
        {
            ID = id;
            X = x;
            Y = y;
        }
    }

    public class Coordinate
    {
        public int area;
        public bool infinite;

        public Coordinate()
        {
            area = 0;
            infinite = false;
        }
    }


    class Day6
    {
        public void Execute()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~         Day 6        -");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("");

            // Load text file
            string fileContent = File.ReadAllText("Input\\Day6Input.txt");
            // Format input to remove white space and any '+' characters
            fileContent = fileContent.Replace("+", "");
            // Split string into an array
            string[] fileContentSplit =
                fileContent.Split(new char[] { '\t', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            List<string> lines = new List<string>();
            lines = fileContentSplit.ToList();

            // Print answers
            //Console.WriteLine("Finding finite areas...");
            //Console.WriteLine("Part 1 Answer:\t" + BothParts(lines).ToString());
            //Console.WriteLine("Finding common letters...");
            //Console.WriteLine("Part 2 Answer:\t" + PartTwo(lines).ToString());

            BothParts(lines);
        }



        private void BothParts(List<string> lines)
        {
            Console.WriteLine("Finding finite areas...");
            List<Vector2D> Coordinates = new List<Vector2D>();

            // First parse file and place into a List.
            int count = 0;
            foreach (var line in lines)
            {

                MatchCollection Collection = Regex.Matches(line, @"\d*[^\s,]");

                int x = int.Parse(Collection[0].ToString());
                int y = int.Parse(Collection[1].ToString());

                Coordinates.Add(new Vector2D(count, x, y));

                count++;
            }

            // Find furthest coordinate from 0,0
            int MAX_X = Coordinates.Max(x => x.X) + 1;
            int MAX_Y = Coordinates.Max(x => x.Y) + 1;
            int MIN_X = Coordinates.Min(x => x.X) - 1;
            int MIN_Y = Coordinates.Min(x => x.Y) - 1;
            int MAP_WIDTH = MAX_X - MIN_X + 1;
            int MAP_HEIGHT = MAX_Y - MIN_Y + 1;

            int[,] areas = new int[MAP_WIDTH, MAP_HEIGHT];
            for (int x = MIN_X; x <= MAX_X; x++)
            {
                for (int y = MIN_Y; y <= MAX_Y; y++)
                {
                    int minDist = int.MaxValue;
                    int minDistId = 0;
                    foreach (var c in Coordinates)
                    {
                        int dist = Math.Abs(x - c.X) + Math.Abs(y - c.Y);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            minDistId = c.ID;
                        }
                        else if (dist == minDist)
                        {
                            minDistId = -1;
                        }
                    }
                    areas[x - MIN_X, y - MIN_Y] = minDistId;
                }
            }

            // Sum up all areas
            Coordinate[] mapCoords = new Coordinate[Coordinates.Count];
            for (int x = MIN_X; x <= MAX_X; x++)
            {
                for (int y = MIN_Y; y <= MAX_Y; y++)
                {
                    int closest = areas[x - MIN_X, y - MIN_Y];
                    if (closest >= 0)
                    {
                        if (mapCoords[closest] == null)
                            mapCoords[closest] = new Coordinate();

                        mapCoords[closest].area++;
                    }
                }
            }

            // Find infite areas
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                int e1 = areas[x, 0];
                if (e1 >= 0)
                {
                    mapCoords[e1].infinite = true;
                }
                int e2 = areas[x, MAP_HEIGHT - 1];
                if (e2 >= 0)
                {
                    mapCoords[e2].infinite = true;
                }
            }
            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                int e1 = areas[0, y];
                if (e1 >= 0)
                {
                    mapCoords[e1].infinite = true;
                }
                int e2 = areas[MAP_WIDTH - 1, y];
                if (e2 >= 0)
                {
                    mapCoords[e2].infinite = true;
                }
            }

            int maxFiniteArea = 0;
            foreach (var coord in mapCoords)
            {
                if ((coord.area > maxFiniteArea) && !coord.infinite)
                {
                    maxFiniteArea = coord.area;
                }
            }

            Console.WriteLine($"Part 1 Answer:\t{maxFiniteArea}");


            // Part B
            Console.WriteLine("Finding close region...");

            int[,] totalDistanceMap = new int[MAP_WIDTH, MAP_HEIGHT];
            int TotalCloseEnough = 0;
            for(int x = MIN_X; x <= MAX_X; x++)
            {
                for(int y = MIN_Y; y <= MAX_Y; y++)
                {
                    foreach(var c in Coordinates)
                    {
                        int distance = Math.Abs(x - c.X) + Math.Abs(y - c.Y);
                        totalDistanceMap[x - MIN_X, y - MIN_Y] += distance;
                    }
                    if(totalDistanceMap[x - MIN_X, y - MIN_Y] < 10000)
                    {
                        TotalCloseEnough++;
                    }
                }
            }

            Console.WriteLine($"Part 2 Answer:\t{TotalCloseEnough}");















        }


    }

}
