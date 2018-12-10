using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Start
{

    public class Day8
    {
        public class Node
        {
            public List<Node> children;
            public List<int> metadata;
            public int sum;

            public Node()
            {
                children = new List<Node>();
                metadata = new List<int>();
                sum = 0;

            }
        }


        public void Execute()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~         Day 8        -");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("");

            // Load text file
            string fileContent = File.ReadAllText("Input\\Day8Input.txt");
            // Format input to remove white space and any '+' characters
            fileContent = fileContent.Replace("+", "");
            // Split string into an array
            string[] fileContentSplit =
                fileContent.Split(new char[] { '\t', '\r', '\n', ' ' },
                StringSplitOptions.RemoveEmptyEntries);

            List<string> lines = new List<string>();
            lines = fileContentSplit.ToList();
            List<int> IntEntries = new List<int>();
            foreach(var line in lines)
            {
                IntEntries.Add(int.Parse(line));
            }

            // Print answers
            Console.WriteLine("Finding sum of metadata entries...");
            Console.WriteLine("Part 1 Answer:\t" + PartOne(IntEntries).ToString());
            Console.WriteLine("Finding head node...");
            Console.WriteLine("Part 2 Answer:\t" + PartTwo(IntEntries).ToString());
        }

        public void ReadNodeA(ref Queue<int> queue, ref List<Node> nodes)
        {
            int children = queue.Dequeue();
            int metadata = queue.Dequeue();
            Node node = new Node();
            // Loop through all children
            for(int i = 0; i < children; i++)
            {
                ReadNodeA(ref queue, ref nodes);
            }
            for(int i = 0; i < metadata; i++)
            {
                node.metadata.Add(queue.Dequeue());
            }
            nodes.Add(node);
        }

        public Node ReadNodeB(ref Queue<int> queue, ref List<Node> nodes)
        {
            int children = queue.Dequeue();
            int metadata = queue.Dequeue();
            Node node = new Node();
            // Loop through all children
            for (int i = 0; i < children; i++)
            {
                node.children.Add(ReadNodeB(ref queue, ref nodes));
            }
            for (int i = 0; i < metadata; i++)
            {
                node.metadata.Add(queue.Dequeue());
            }

            if(children > 0)
            {
                foreach (int i in node.metadata)
                {
                    if((i > 0) && (i <= children))
                    {
                        node.sum += node.children[i - 1].sum;
                    }
                }
            }
            else
            {
                node.sum = node.metadata.Sum();
            }
            return node;
        }


        private int PartOne(List<int> entries)
        {
            Queue<int> input = new Queue<int>(entries);
            List<Node> nodes = new List<Node>();

            ReadNodeA(ref input, ref nodes);
            int sum = nodes.Sum(a => a.metadata.Sum());

            return sum;
        }

        private int PartTwo(List<int> entries)
        {
            Queue<int> input = new Queue<int>(entries);
            List<Node> nodes = new List<Node>();

            var Node = ReadNodeB(ref input, ref nodes);

            

            return Node.sum;
        }


    }
}
