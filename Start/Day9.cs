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
    static class CircularLinkedList
    {
        public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> current)
        {
            if (current.Next == null)
                return current.List.First;
            else
                return current.Next;
        }

        public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> current)
        {
            if (current.Previous == null)
                return current.List.Last;
            else
                return current.Previous;
        }
    }

    public class Day9
    {

        public void Execute()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~         Day 9        -");
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
            foreach (var line in lines)
            {
                IntEntries.Add(int.Parse(line));
            }

            // Print answers
            Console.WriteLine("Finding high score...");
            Console.WriteLine("Part 1 Answer:\t" + Game(431, 70950).ToString());
            Console.WriteLine("Finding high score with last marble being 100 times larger...");
            Console.WriteLine("Part 2 Answer:\t" + Game(431, 70950 * 100).ToString());
        }

        public long Game(int players, int lastMarble)
        {
            int LASTMARBLE = lastMarble;
            int TOTALPLAYERS = players;
            long[] score = new long[TOTALPLAYERS];
            LinkedList<int> circle = new LinkedList<int>();
            LinkedListNode<int> cPosition = circle.AddFirst(0);
            int marble = 1;
            int player = 0;

            for (int i = 1; i <= LASTMARBLE; i++)
            {
                if (marble % 23 == 0)
                {
                    score[player] += marble;
                    cPosition = cPosition.PreviousOrLast().PreviousOrLast().PreviousOrLast();
                    cPosition = cPosition.PreviousOrLast().PreviousOrLast().PreviousOrLast();
                    var scorePos = cPosition;
                    cPosition = cPosition.PreviousOrLast();
                    circle.Remove(scorePos);
                    score[player] += scorePos.Value;

                }
                else
                {
                    cPosition = cPosition.NextOrFirst().NextOrFirst();
                    circle.AddAfter(cPosition, marble);
                }
                marble++;
                player = (player + 1) % TOTALPLAYERS;
            }

            return score.Max();
        }


    }
        
        
}
   
