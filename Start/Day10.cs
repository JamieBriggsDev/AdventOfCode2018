using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Start
{

    class Day10
    {
        public class Node : IEqualityComparer
        {
            public int pX;
            public int pY;
            public int vX;
            public int vY;

            public Node(int px, int py, int vx, int vy)
            {
                pX = px;
                pY = py;
                vX = vx;
                vY = vy;
            }

            public void Update()
            {
                pX += vX;
                pY += vY;
            }

            public void Revert()
            {
                pX -= vX;
                pY -= vY;
            }

            public int SmallestDistance(List<Node> _list)
            {
                List<Node> temp = new List<Node>(_list);
                temp.Remove(this);
                int SmallestDistance = temp.Min(a => a.Distance(pX, pY));
                return SmallestDistance;
            }

            public int Distance(int x, int y)
            {
                return (Math.Abs(x - pX) * Math.Abs(y - pY));
            }

            public new bool Equals(object x, object y)
            {
                Node nodex = (Node)x;
                Node nodey = (Node)y;
                if (nodex.pX == nodey.pX &&
                    nodex.pY == nodey.pY)
                    return true;
                else
                    return false;

            }

            public int GetHashCode(object obj)
            {
                throw new NotImplementedException();
            }
        }


        public void Execute()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~         Day 6        -");
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
            Console.WriteLine("Finding message...");
            PartOne(lines);
            //Console.WriteLine("Part 1 Answer:\t" + PartOne(lines).ToString());
            //Console.WriteLine("Finding common letters...");
            //Console.WriteLine("Part 2 Answer:\t" + PartTwo(lines).ToString());
        }

        //private static List<Node> positions;

        public static List<Node> Positions = new List<Node>();

        private void PartOne(List<string> lines)
        {
            Positions = new List<Node>();

            foreach (var line in lines)
            {
                MatchCollection Collection = Regex.Matches(line, @"[-\d]+");

                int px = int.Parse(Collection[0].ToString());
                int py = int.Parse(Collection[1].ToString());
                int vx = int.Parse(Collection[2].ToString());
                int vy = int.Parse(Collection[3].ToString());


                Positions.Add(new Node(px, py, vx, vy));
            }

            Application.EnableVisualStyles();
            //Application.Run(new Form10());
            Form temp = new Form10();
            temp.Show();

            int count = 0;
            //int TotalSmallDistance = 0; 
            while(true)
            {
                Positions.ForEach(a => a.Update());

                //TotalSmallDistance = 
                //    Positions.Count(a => a.SmallestDistance(Positions) <= 1);

                //if (TotalSmallDistance == Positions.Count)
                //    break;

                int minY = Positions.Min(a => a.pY);
                int maxY = Positions.Max(a => a.pY);

                int numY = maxY - minY + 1;

                count++;
                if (numY == 10)
                    break;


                if(count > 10000)
                {
                    //if(count % 3 == 0)
                        temp.Refresh();
                }

            }
            temp.Refresh();

            Console.WriteLine($"Count: {count}");




        }
    }

}
