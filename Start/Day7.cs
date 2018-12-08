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
    public class Step : IEqualityComparer, IComparable
    {
        public char StepID;
        public List<char> PrerequisiteID;
        public bool Found;
        public bool Processing;

        public Step(char ID)
        {
            Processing = true;
            Found = false;
            StepID = ID;
            PrerequisiteID = new List<char>();
        }
        public Step(char ID, char preID)
        {
            Processing = true;
            Found = false;
            StepID = ID;
            PrerequisiteID = new List<char>();
            PrerequisiteID.Add(preID);
        }

        public int CompareTo(object obj)
        {
            Step other = (Step)obj;

            if (StepID > other.StepID)
                return 1;
            else if (StepID == other.StepID)
                return 0;
            else
                return -1;

        }

        public new bool Equals(object x, object y)
        {
            Step a = (Step)x;
            Step b = (Step)y;

            if (a.StepID == b.StepID)
                return true;
            else
                return false;
        }

        public int GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }
    }

    public class Worker
    {
        public bool Idle;
        public int TimeLeftOnJob = 0;
        public char StepWorkingOn;
        public Worker()
        {
            Idle = true;
        }

        public void StartJob(char letter)
        {
            Idle = false;
            TimeLeftOnJob = 60 + letter - 64;
            StepWorkingOn = letter;
        }

        public void ProcessJob()
        {
            if(!Idle)
                TimeLeftOnJob--;

            if (TimeLeftOnJob <= 0)
                TimeLeftOnJob = 0;
        }

        public void FinishJob()
        {
            Idle = true;
        }

    }

    public class Day7
    {
        public void Execute()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~         Day 7        -");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("");

            // Load text file
            string fileContent = File.ReadAllText("Input\\Day7Input.txt");
            // Format input to remove white space and any '+' characters
            fileContent = fileContent.Replace("+", "");
            // Split string into an array
            string[] fileContentSplit =
                fileContent.Split(new char[] { '\t', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            List<string> lines = new List<string>();
            lines = fileContentSplit.ToList();

            // Print answers
            Console.WriteLine("Finding Instructions...");
            Console.WriteLine("Part 1 Answer:\t" + PartOne(lines).ToString());
            Console.WriteLine("Finding Efficiency...");
            Console.WriteLine("Part 2 Answer:\t" + PartTwo(lines).ToString());
        }

        private string PartOne(List<string> lines)
        {
            
            List<Step> Steps = new List<Step>();

            // First parse file and place into a List.
            foreach (var line in lines)
            {

                MatchCollection Collection = Regex.Matches(line, @"[A-Z]\b");

                char a = Collection[0].ToString().ToCharArray()[0];
                char b = Collection[1].ToString().ToCharArray()[0];


                // First add prerequisit step to hashset. If it is already in there
                //  then do not add
                if(!Steps.Exists(x => x.StepID == a))
                {
                    Steps.Add(new Step(a));
                }
                // Add step to hash table. If it is already in the hashset, add step
                //  to steps prerequisit list
                if(Steps.Exists(x => x.StepID == b))
                {
                    // Adds char 'a' to step in list with stepID == 'b'
                    Steps.Where(w => w.StepID == b).ToList().ForEach(s => s.PrerequisiteID.Add(a));
                }
                else
                {
                    Steps.Add(new Step(b, a));
                }

            }

            // Find start and end steps
            char START = Steps.Find(a => a.PrerequisiteID.Count == 0).StepID;
            char END = '0';
            foreach(var step in Steps)
            {
                bool FoundEnd = true;
                END = step.StepID;
                foreach(var nStep in Steps)
                {
                    if(nStep.PrerequisiteID.Contains(END))
                    {
                        FoundEnd = false;
                    }
                }

                // End step has been found
                if (FoundEnd)
                    break;

            }

            // List of every step
            List<Step> Output = new List<Step>();
            Steps.Find(a => a.StepID == START).Found = true;
            Output.Add(Steps.Find(x => x.StepID == START));
            // Add start to Output
            // Work from the start
            List<Step> AllAvailableIDs = new List<Step>();

            while(Output.Last().StepID != END)
            {

                // Set up all available steps
                // Go through each step
                foreach(var step in Steps)
                {
                    // Go through each prerequisit in step and add to output
                    bool Available = true;
                    foreach(var pre in step.PrerequisiteID)
                    {
                        if (!Output.Exists(a => a.StepID == pre))
                        {
                            Available = false;
                        }
                    }
                    // If available is still true, add to available IDs
                    if(!AllAvailableIDs.Exists(a => a.StepID == step.StepID) &&
                        !Output.Exists(a => a.StepID == step.StepID) &&
                        Available)
                    {
                        AllAvailableIDs.Add(step);
                    }

                }

                // Second, order the output list into alphabetical
                AllAvailableIDs.Sort();

                // Add first item in all available IDs
                Output.Add(AllAvailableIDs.First());

                // Remove from all available IDs
                AllAvailableIDs.RemoveAt(0);


            }


            string output = "";
            
            foreach(var x in Output)
            {
                output += x.StepID;
            }
            return output;
        }

        int TimeCount;

        private string PartTwo(List<string> lines)
        {
            // Time count
            TimeCount = 0;
            // All Workers
            List<Worker> AllWorkers = new List<Worker>();
            for(int i = 0; i < 5; i++)
            {
                AllWorkers.Add(new Worker());
            }

            List<Step> Steps = new List<Step>();

            // First parse file and place into a List.
            foreach (var line in lines)
            {

                MatchCollection Collection = Regex.Matches(line, @"[A-Z]\b");

                char a = Collection[0].ToString().ToCharArray()[0];
                char b = Collection[1].ToString().ToCharArray()[0];


                // First add prerequisit step to hashset. If it is already in there
                //  then do not add
                if (!Steps.Exists(x => x.StepID == a))
                {
                    Steps.Add(new Step(a));
                }
                // Add step to hash table. If it is already in the hashset, add step
                //  to steps prerequisit list
                if (Steps.Exists(x => x.StepID == b))
                {
                    // Adds char 'a' to step in list with stepID == 'b'
                    Steps.Where(w => w.StepID == b).ToList().ForEach(s => s.PrerequisiteID.Add(a));
                }
                else
                {
                    Steps.Add(new Step(b, a));
                }

            }

            // Find start and end steps
            char START = Steps.Find(a => a.PrerequisiteID.Count == 0).StepID;
            char END = '0';
            foreach (var step in Steps)
            {
                bool FoundEnd = true;
                END = step.StepID;
                foreach (var nStep in Steps)
                {
                    if (nStep.PrerequisiteID.Contains(END))
                    {
                        FoundEnd = false;
                    }
                }

                // End step has been found
                if (FoundEnd)
                    break;

            }

            // List of every step
            List<Step> Output = new List<Step>();
            Steps.Find(a => a.StepID == START).Found = true;
            //Output.Add(Steps.Find(x => x.StepID == START));
            // Add start to available IDs
            List<Step> AllAvailableIDs = new List<Step>();
            AllAvailableIDs.Add(Steps.Find(x => x.StepID == START));

            while (true)
            {
            
                // First check if workers have finished their jobs and give a new job
                while (AllWorkers.Exists(a => a.TimeLeftOnJob == 0 && a.Idle == false))
                {
                    var CompletedJob = AllWorkers.First(a => a.TimeLeftOnJob == 0 && a.Idle == false);

                    // Add to output
                    Output.Add(Steps.First(a => a.StepID == CompletedJob.StepWorkingOn));
                    //AllAvailableIDs.Remove(AllAvailableIDs.First(a => a.StepID == CompletedJob.StepWorkingOn));

                    CompletedJob.FinishJob();
                }


                // Next set up all available steps
                // Go through each step
                foreach (var step in Steps)
                {
                    // Go through each prerequisit in step and add to output
                    bool Available = true;
                    foreach (var pre in step.PrerequisiteID)
                    {
                        if (!Output.Exists(a => a.StepID == pre))
                        {
                            Available = false;
                        }
                    }
                    // If available is still true, add to available IDs
                    if (!AllAvailableIDs.Exists(a => a.StepID == step.StepID) &&
                        !Output.Exists(a => a.StepID == step.StepID) &&
                        !AllWorkers.Exists(a => a.StepWorkingOn == step.StepID) &&
                        Available)
                    {
                        AllAvailableIDs.Add(step);
                    }

                }

                // Next, order the output list into alphabetical
                AllAvailableIDs.Sort();

                // Next give all available jobs to all available workers
                while (AllWorkers.Exists(a => a.Idle == true) &&
                    AllAvailableIDs.Count > 0)
                {
                    char job = AllAvailableIDs.First().StepID;
                    AllWorkers.First(w => w.Idle == true).StartJob(job);
                    AllAvailableIDs.Remove(AllAvailableIDs.First(a => a.StepID == job));

                }

                // Process all workers jobs
                foreach (var worker in AllWorkers)
                {
                    worker.ProcessJob();
                }

                // Break if last ID has been found
                if (Output.Count > 0)
                {
                    if (Output.Last().StepID == END)
                        break;
                }

                // Count
                TimeCount++;

            }




            string output = "";

            foreach (var x in Output)
            {
                output += x.StepID;
            }

            Console.WriteLine($"(Final Result for Part 2: {output}");

            return TimeCount.ToString();
        }
    }
}
