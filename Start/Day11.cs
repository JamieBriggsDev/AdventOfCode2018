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

namespace Start
{

    class Day11
    {
        const int GRID_SERIAL_NUMBER = 9221;
        public static Random rand = new Random(DateTime.Now.Second);

        public class FuelCell
        {
            public int X;
            public int Y;
            public int RackID;
            public int PowerLevel;

            public RectangleShape rect;

            public FuelCell(int x, int y, bool offset)
            {
                
                X = x;
                Y = y;

                if(offset)
                {
                    X++;
                    Y++;
                }

                FindPowerLevel();


                rect = new RectangleShape(new SFML.Window.Vector2f(3f, 3f));
                rect.Position = new SFML.Window.Vector2f(X * 3, Y * 3);
                int colour = ( PowerLevel * 51);
                if( colour < 0)
                    rect.FillColor = new Color((byte)Math.Abs(colour), 0, 0);
                else
                    rect.FillColor = new Color(0, (byte)colour, 0);


            }

            public void FindPowerLevel()
            {
                RackID = X + 10;
                PowerLevel = RackID * Y;
                PowerLevel += GRID_SERIAL_NUMBER;
                PowerLevel *= RackID;
                if (PowerLevel >= 100)
                    PowerLevel = (PowerLevel / 100) % 10;
                else
                    PowerLevel = 0;

                PowerLevel -= 5;
            }
        }

        public struct Grid
        {
            public int X;
            public int Y;
            public int Size;
            public int TotalPowerLevel;


            public Grid(int x, int y, int power)
            {
                X = x;
                Y = y;
                TotalPowerLevel = power;
                Size = 3;
            }

            public Grid(int x, int y, int power, int size)
            {
                X = x;
                Y = y;
                TotalPowerLevel = power;
                Size = size;
            }
        }


        public void Execute()
        {
            // Generate random

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~         Day 11        -");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("");

            // Load text file
            string fileContent = File.ReadAllText("Input\\Day10Input.txt");
            // Format input to remove white space and any '+' characters
            fileContent = fileContent.Replace("+", "");
            // Split string into an array
            string[] fileContentSplit =
                fileContent.Split(new char[] { '\t', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            List<string> lines = new List<string>();
            lines = fileContentSplit.ToList();

            // Print answers
            Console.WriteLine("Finding energy...");
            PartOne();
            Console.WriteLine("Finding energy with mixed size...");
            PartTwo();
        }

        //private static List<Node> positions;
        RenderWindow window = new RenderWindow(new SFML.Window.VideoMode(900, 900), "Day 11");


        private void PartOne()
        {
            List<FuelCell> AllFuelCells = new List<FuelCell>();

            for(int x = 1; x <= 300; x++)
            {
                for(int y = 1; y <= 300; y++)
                {
                    AllFuelCells.Add(new FuelCell(x, y, false));
                    
                }
            }


            int min = AllFuelCells.Min(a => a.PowerLevel);
            int max = AllFuelCells.Max(a => a.PowerLevel);

            //Console.WriteLine($"Min: {min} \t Max: {max}");

            
            //CircleShape cs = new CircleShape(100.0f);
            //cs.FillColor = Color.Green;
            window.SetActive();

            Application.EnableVisualStyles();


            window.Clear();
            window.DispatchEvents();
            foreach (var obj in AllFuelCells)
                window.Draw(obj.rect);
            window.Display();

            List<Grid> grids = new List<Grid>();

            int index = 0;

            for(int x = 1; x < 299; x++)
            {
                //Console.WriteLine($"Checking X = {x}");

                for(int y = 1; y < 299; y++)
                {
                    
                    //int[,] power = new int[3, 3];
                    int totalPower = 0;

                    for (int i = 0; i < 3; i++)
                    {
                        for(int j = 0; j < 3; j++)
                        {

                            //totalPower += AllFuelCells.Find(a => a.X == (x + i) && a.Y == (y + j)).PowerLevel;
                            totalPower += AllFuelCells[index].PowerLevel;
                            //Console.WriteLine($"Coordinate: {AllFuelCells[index].X},{AllFuelCells[index].Y}");
                            index += 300;

                        }
                        index -= 899;
                    }


                    //Thread.Sleep(5000);
                    index -= 2;

                    grids.Add(new Grid(x, y, totalPower));
                }

                index = 300 * x;
            }

            var maximum = grids.Max(a => a.TotalPowerLevel);
            var highest = grids.Find(a => a.TotalPowerLevel == maximum);

            Console.WriteLine($"Largest Power Coordinate: {highest.X}, {highest.Y}");
            Console.WriteLine($"Max fuel cell: {maximum}");

            


        }

        private void PartTwo()
        {

            FuelCell[,] AllFuelCells = new FuelCell[301, 301];
            int[,] sum = new int[301, 301];

            for(int i = 0; i < 301; i++)
            {
                for(int j = 0; j < 301; j++)
                {
                    sum[i, j] = 0;
                }
            }


            for (int y = 1; y <= 300; y++)
            {
                for (int x = 1; x <= 300; x++)
                {
                    //AllFuelCells[x, y] = new FuelCell(x, y, false);
                    int id = x + 10;
                    int p = id * y + GRID_SERIAL_NUMBER;
                    p = (p * id) / 100 % 10 - 5;
                    sum[y,x] = p + sum[y - 1, x]
                            + sum[y, x - 1]
                            - sum[y - 1, x - 1];
                }
            }

            int best = 0 ;
            int bx = 0;
            int by = 0;
            int bs = 0;


            for(int size = 1; size <= 300; size++)
            {
                Console.WriteLine(size);
                for (int y = size; y <= 300; y++)
                {
                    //Console.WriteLine($"Checking X = {x}");

                    for (int x = size; x <= 300; x++)
                    {

                        //int[,] power = new int[3, 3];
                        int totalPower = sum[y, x]
                            - sum[y - size, x]
                            - sum[y, x - size]
                            + sum[y - size, x - size];

                        if(totalPower > best)
                        {
                            bx = x;
                            by = y;
                            bs = size;
                            best = totalPower;
                        }
                    }

                }

            }

            Console.WriteLine($"Largest Power Coordinate: {bx - bs + 1},{by - bs + 1},{bs}");
            Console.WriteLine($"Max fuel cell: {best}");




        }
    }
}
