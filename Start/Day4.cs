using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Start
{
    // Entry
    public class Day4Entry : IComparable<Day4Entry>
    {
        public DateTime TimeStamp;
        public string Message;

        public Day4Entry(int year, int month, int day, int hour, int minute, string message)
        {
            TimeStamp = new DateTime(year, month, day, hour, minute, 0);
            Message = message;
        }

        public int CompareTo(Day4Entry _other)
        {
            if (this > _other)
                return 1;
            else if (this < _other)
                return -1;
            else
                return 0;
        }

        public static bool operator <(Day4Entry _this, Day4Entry _other)
        {
            if (_this.TimeStamp < _other.TimeStamp)
                return true;
            else
                return false;
        }

        public static bool operator >(Day4Entry _this, Day4Entry _other)
        {
            if (_this.TimeStamp > _other.TimeStamp)
                return true;
            else
                return false;
        }

    }

    // Day 4 Guard Class
    public class Guard : IComparable<Guard>
    {
        public Guard(int x)
        {
            minutesAsleep = 0;
            gaurdID = x;
        }

        public int minutesAsleep;
        public int gaurdID;

        public int CompareTo(Guard _other)
        {
            if (this > _other)
                return 1;
            else if (this < _other)
                return -1;
            else
                return 0;
        }

        public override bool Equals(object obj)
        {
            var guard = obj as Guard;
            return guard != null &&
                   gaurdID == guard.gaurdID;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator <(Guard _this, Guard _other)
        {
            if (_this.minutesAsleep < _other.minutesAsleep)
                return true;
            else
                return false;
        }

        public static bool operator >(Guard _this, Guard _other)
        {
            if (_this.minutesAsleep > _other.minutesAsleep)
                return true;
            else
                return false;
        }

        public static bool operator ==(Guard _this, Guard _other)
        {
            if (_this.gaurdID == _other.gaurdID)
                return true;
            else
                return false;
        }

        public static bool operator !=(Guard _this, Guard _other)
        {
            if (_this.gaurdID != _other.gaurdID)
                return true;
            else
                return false;
        }
    }




    class Day4
    {
        public void Execute()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~         Day 4        -");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("");

            // Load text file
            string fileContent = File.ReadAllText("Input\\input.txt");
            // Format input to remove white space and any '+' characters
            fileContent = fileContent.Replace("+", "");
            // Split string into an array
            string[] fileContentSplit =
                fileContent.Split(new char[] { '\t', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            List<string> lines = new List<string>();
            lines = fileContentSplit.ToList();

            // Print answers
            Console.WriteLine("Finding worst gaurd...");
            Console.WriteLine("Part 1 Answer:\t" + PartOne(lines).ToString());
            Console.WriteLine("Finding common letters...");
            Console.WriteLine("Part 2 Answer:\t" + PartTwo(lines).ToString());
        }


        private string PartOne(List<string> _input)
        {
            // Place seperate rows into list of entrys
            var Entries = new List<Day4Entry>();
            foreach (var line in _input)
            {
                // Get time details out of line first
                string[] parse =
                    line.Split(new char[] { '[', ']', '-', ' ', ':' },
                    StringSplitOptions.RemoveEmptyEntries);

                var year = int.Parse(parse[0]);
                var month = int.Parse(parse[1]);
                var day = int.Parse(parse[2]);
                var hour = int.Parse(parse[3]);
                var minute = int.Parse(parse[4]);

                // Get message details out of line second
                parse = line.Split(new char[] { '[', ']' },
                    StringSplitOptions.RemoveEmptyEntries);
                var message = parse[1];

                Day4Entry temp = new Day4Entry(year, month, day, hour, minute, message);

                Entries.Add(temp);
            }

            Console.WriteLine("BEFORE SORT");
            for(int i = 0; i < 5; i++)
            {
                Console.WriteLine(Entries[i].TimeStamp);
            }
            // Sort entries into chronological order
            Entries.Sort();
            Console.WriteLine("AFTER SORT");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(Entries[i].TimeStamp);
            }

            int CurrentGaurdID = 0;
            int SleepTime = 0;
            int WakeTime = 0;

            // List of gaurds
            var DictionaryGuards = new Dictionary<int, Guard>();
            foreach (var Ent in Entries)
            {
                // Split up message in entry
                string[] parse =
                    Ent.Message.Split(new char[] { ' ', '#' },
                    StringSplitOptions.RemoveEmptyEntries);

                // Check what the first word is in the message to determine
                //  what to process
                if (parse[0] == "Guard")
                {
                    CurrentGaurdID = int.Parse(parse[1]);

                    if (!DictionaryGuards.ContainsKey(CurrentGaurdID))
                    {
                        //Guard temp = ;
                        DictionaryGuards.Add(CurrentGaurdID, new Guard(CurrentGaurdID));
                    }

                }
                else if (parse[0] == "falls")
                {
                    SleepTime = Ent.TimeStamp.Minute;
                }
                else if (parse[0] == "wakes")
                {
                    WakeTime = Ent.TimeStamp.Minute;
                    int TimeAsleep = WakeTime - SleepTime;
                    DictionaryGuards[CurrentGaurdID].minutesAsleep += TimeAsleep;

                }
            }

            var ListGuards = DictionaryGuards.Values.ToList();
            var LaziestGuard = ListGuards.Max();



            // Go though valid entries and check
            var MinutesCount = new List<int>();
            for (int i = 0; i < 60; i++)
            {
                MinutesCount.Add(0);
            }

            bool CheckingRightGuard = false;
            foreach (var Ent in Entries)
            {
                // Split up message in entry
                string[] parse =
                    Ent.Message.Split(new char[] { ' ', '#' },
                    StringSplitOptions.RemoveEmptyEntries);

                // Check what the first word is in the message to determine
                //  what to process
                if (parse[0] == "Guard")
                {
                    CheckingRightGuard = false;
                    CurrentGaurdID = int.Parse(parse[1]);

                    if (CurrentGaurdID == LaziestGuard.gaurdID)
                    {
                        // start checking minuters
                        CheckingRightGuard = true;
                    }

                }
                else if (parse[0] == "falls" && CheckingRightGuard)
                {
                    SleepTime = Ent.TimeStamp.Minute;
                }
                else if (parse[0] == "wakes" && CheckingRightGuard)
                {
                    WakeTime = Ent.TimeStamp.Minute;

                    for (int i = SleepTime; i < WakeTime; i++)
                    {
                        MinutesCount[i]++;

                    }


                }
            }

            int TotalWorstMinutes = MinutesCount.Max();
            int WorstActualMinute = MinutesCount.IndexOf(TotalWorstMinutes);

            Console.WriteLine($"Laziest Guard: #{LaziestGuard.gaurdID}. Slept for {LaziestGuard.minutesAsleep} minutes!");
            Console.WriteLine($"{WorstActualMinute} was his worst minute!");
            Console.WriteLine($"{LaziestGuard.gaurdID} X {WorstActualMinute} = {LaziestGuard.gaurdID * WorstActualMinute}");

            return $"{LaziestGuard.gaurdID} * {WorstActualMinute} = {LaziestGuard.gaurdID * WorstActualMinute}";
        }

        private string PartTwo(List<string> _input)
        {
            // Place seperate rows into list of entrys
            var Entries = new List<Day4Entry>();
            foreach (var line in _input)
            {
                // Get time details out of line first
                string[] parse =
                    line.Split(new char[] { '[', ']', '-', ' ', ':' },
                    StringSplitOptions.RemoveEmptyEntries);

                var year = int.Parse(parse[0]);
                var month = int.Parse(parse[1]);
                var day = int.Parse(parse[2]);
                var hour = int.Parse(parse[3]);
                var minute = int.Parse(parse[4]);

                // Get message details out of line second
                parse = line.Split(new char[] { '[', ']' },
                    StringSplitOptions.RemoveEmptyEntries);
                var message = parse[1];

                Day4Entry temp = new Day4Entry(year, month, day, hour, minute, message);

                Entries.Add(temp);
            }

            // Sort entries into chronological order
            Entries.Sort();

            int CurrentGaurdID = 0;
            int SleepTime = 0;
            int WakeTime = 0;

            var Guards = new Dictionary<int, Dictionary<int, int>>();


            foreach (var Ent in Entries)
            {
                // Split up message in entry
                string[] parse =
                    Ent.Message.Split(new char[] { ' ', '#' },
                    StringSplitOptions.RemoveEmptyEntries);

                // Check what the first word is in the message to determine
                //  what to process
                if (parse[0] == "Guard")
                {
                    CurrentGaurdID = int.Parse(parse[1]);

                    if(!Guards.ContainsKey(CurrentGaurdID))
                    {
                        Guards.Add(CurrentGaurdID, new Dictionary<int, int>());
                    }


                }
                else if (parse[0] == "falls" )
                {
                    SleepTime = Ent.TimeStamp.Minute;
                }
                else if (parse[0] == "wakes" )
                {
                    WakeTime = Ent.TimeStamp.Minute;

                    for (int i = SleepTime; i < WakeTime; i++)
                    {
                        if(!Guards[CurrentGaurdID].ContainsKey(i))
                        {
                            Guards[CurrentGaurdID].Add(i, 1);
                        }
                        else
                        {
                            Guards[CurrentGaurdID][i]++;
                        }
                    }
                }
            }

            // Now go through every guard and find most frequent minute
            int GuardID = 0;
            int Minute = 0;
            int MinuteIndex = 0;
            foreach (var g in Guards)
            {
                if(g.Value.Values.Count > 0)
                {
                    int max = g.Value.Values.Max();
                    if (max > Minute)
                    {
                        Minute = g.Value.Values.Max();
                        MinuteIndex = g.Value.FirstOrDefault(x => x.Value == Minute).Key;
                        GuardID = g.Key;
                    }
                }

            }

            Console.WriteLine($"Laziest Guard: #{GuardID}!");
            Console.WriteLine($"{MinuteIndex} was his worst minute!");
            //Console.WriteLine($"{LaziestGuard} X {Minute} = {LaziestGuard * Minute}");

            return $"{GuardID} * {MinuteIndex} = {GuardID * MinuteIndex}";

        }
    }
}


