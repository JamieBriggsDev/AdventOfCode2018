using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Start
{
    class Program
    {
        static void Main(string[] args)
        {
            int number = 0;
            while (number != -1)
            {
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("~         Menu         -");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("");
                // Display options
                Console.WriteLine("Pick an option:");
                Console.WriteLine("Day\t1");
                Console.WriteLine("Day\t2");
                Console.WriteLine("\nExit\t-1");

                // Take user input
                Console.Write("Option:\t");
                string input = Console.ReadLine();
                // Parse user input
                Int32.TryParse(input, out number);

                Console.WriteLine("\n");

                // Do process depending on user input
                switch (number)
                {
                    case -1:
                        Console.WriteLine("Exiting..");
                        System.Threading.Thread.Sleep(100);
                        break;
                    case 1:
                        Day1.Day1 one = new Day1.Day1();
                        one.Execute();
                        break;
                    case 2:
                        Day2.Day2 two = new Day2.Day2();
                        two.Execute();
                        break;
                    default:
                        Console.WriteLine("Unknown value entered!");
                        break;

                }

                Console.WriteLine("\n");


            }
        }
    }
}
