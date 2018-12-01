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
                Console.WriteLine("Exit\t-1");
                Console.WriteLine("Day\t1");

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
                        Day1.Day1 day = new Day1.Day1();
                        day.Execute();
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
