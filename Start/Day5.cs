using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Start
{
    class Day5
    {
        public class Char
        {
            public char Character;
            public bool RemoveFlag;
            public int Index;

            public Char(char _char, int _index)
            {
                Character = _char;
                Index = _index;
            }

        }

        public void Execute()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~         Day 5        -");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("");

            // Load text file
            string fileContent = File.ReadAllText("Input\\Day5Input.txt");

            // Print answers
            Console.WriteLine("Finding total polymer units...");
            Console.WriteLine("Part 1 Answer:\t" + PartOne(fileContent).ToString());
            Console.WriteLine("Finding common letters...");
            Console.WriteLine("Part 2 Answer:\t" + PartTwo(fileContent).ToString());
        }

        private string PartOne(string _input)
        {
            // Index, Needs Replacing, Character
            Char[] AllCharacters = new Char[_input.Length];

            char[] TempCharacters = _input.ToCharArray();

            // Fill all characters dictionary
            for(int i = 0; i < _input.Length; i++)
            {
                AllCharacters[i] = new Char(TempCharacters[i], i);
            }


            // Flag to see if we need to loop again
            bool Loop = true;

            // Keep looping foreach loop till cant remove anymore
            while(Loop)
            {

                // Stores last index checked
                int LastIndex = 0;
                while (AllCharacters[LastIndex].RemoveFlag)
                    LastIndex++;

                Loop = false;
                // Loop through all characters
                foreach (var character in AllCharacters)
                {
                    if (character.RemoveFlag)
                        continue;
                    // 'A' + 22 = 'a'
                    if (character.Character == (AllCharacters[LastIndex].Character + 32) ||
                        character.Character == (AllCharacters[LastIndex].Character - 32))
                    {
                        Loop = true;
                        character.RemoveFlag = true;
                        AllCharacters[LastIndex].RemoveFlag = true;
  
                    }

                    if (Loop)
                        break;

                    LastIndex = character.Index;
                }
            }


            // Now count how many character are left
            int count = 0;
            StringBuilder builder = new StringBuilder();
            foreach(var character in AllCharacters)
            {
                if (!character.RemoveFlag)
                {
                    count++;
                    builder.Append(character.Character);
                }
            }

            Console.WriteLine(count.ToString());
            //Console.WriteLine(builder.ToString());

            return count.ToString();
        }

        private int PartTwo(string _input)
        {
            var Alphabet = new Dictionary<char, int>();

// int count = 0;
            for(int count = 0; count < 26; count++)
            {
                char LetterCheck = (char)(count + 'a');
                Console.WriteLine($"Checkking the letter {LetterCheck}.");

                string temp = _input;
                temp = temp.Replace(LetterCheck.ToString(), "");
                temp = temp.Replace(LetterCheck.ToString().ToUpper(), "");

                Alphabet.Add(LetterCheck, int.Parse(PartOne(temp)) );
            }

            // Get max value
            int min = int.MaxValue;
            foreach(var letter in Alphabet)
            {
                if(letter.Value < min)
                    min = letter.Value;
            }


            return min;
        }
    }
}

