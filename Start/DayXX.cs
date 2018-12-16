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

    class DayXX
    {
      
        public void Execute()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~         Day X        -");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("");

            // Load text file
            string fileContent = File.ReadAllText("Input\\DayXXInput.txt");
            // Split string into an array
            string[] fileContentSplit =
                fileContent.Split(new char[] {'\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            List<string> lines = new List<string>();
            lines = fileContentSplit.ToList();

            // Print answers
            Console.WriteLine("Finding xxxx...");
            
        }
    }
}
