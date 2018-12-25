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

    class Day15
    {
        public static int WIDTH = 0;
        public static int HEIGHT = 0;
        public static float SIZE = 0.0f;
        public void Execute()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~         Day 15       -");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("");

            // Load text file
            string fileContent = File.ReadAllText("Input\\Day15Input.txt");
            // Split string into an array
            string[] fileContentSplit =
                fileContent.Split(new char[] {'\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            List<string> lines = new List<string>();
            lines = fileContentSplit.ToList();

            // Print answers
            Console.WriteLine("Fight begins...");
            Game(lines);
   
        }

        public class Coordinate : IEqualityComparer, IComparable
        {
            public int X;
            public int Y;

            public int pX;
            public int pY;

            public Coordinate(int x, int y)
            {
                X = x;
                Y = y;
                pX = 0;
                pY = 0;
            }

            public Coordinate(Coordinate coord)
            {
                X = coord.X;
                Y = coord.Y;
                pX = coord.pX;
                pY = coord.pY;
            }

            public Coordinate(int x, int y, Coordinate previous)
            {
                X = x;
                Y = y;
                pX = previous.X;
                pY = previous.Y;
                
            }

            public new bool Equals(object x, object y)
            {
                Coordinate a = (Coordinate)x;
                Coordinate b = (Coordinate)y;

                if(a.X == b.X &&
                    a.Y == b.Y)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public int GetHashCode(object obj)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return $"({X}, {Y})";
            }

            public int CompareTo(object obj)
            {
                Coordinate other = (Coordinate)obj;
                if (Y < other.Y)
                    return -1;
                else if (Y > other.Y)
                    return 1;
                else
                {
                    if (X < other.X)
                        return -1;
                    else if (X > other.X)
                        return 1;
                    else
                        return 0;
                }
            }
        }

        public class Warrior : IComparable
        {
            public Coordinate Position;
            public float Health;
            public int AttackPower;
            public RectangleShape rect;
            public Text text;
            public string type;


            public Warrior(int x, int y)
            {
                AttackPower = 3;
                Position = new Coordinate(x, y);
                Health = 200.0f;
                rect = new RectangleShape(new Vector2f(1.0f, 1.0f) * SIZE);
                text = new Text(Health.ToString(), new Font("Input\\Venator.ttf"), (uint)SIZE / 2);
                rect.Position = new Vector2f(x, y) * SIZE;
                text.Position = new Vector2f(x, y) * SIZE;
                text.Color = new Color(0, 0, 0);
            }

            public void Damage(int _damage)
            {
                Health -= _damage;
                text.DisplayedString = Health.ToString();
            }

            public Stack<Coordinate> CheckPathExists(List<Warrior> _warriors, Tile[,] _map, Coordinate _start,
                List<Coordinate> _targets)
            {
                Stack<Coordinate> Output = new Stack<Coordinate>();
                Dictionary<Coordinate, int> Targets = new Dictionary<Coordinate, int>();
                int[,] PathExists = new int[WIDTH, HEIGHT];
                // Breadth Search
                // Fill blockages first
                // 1 = start
                // 2 = finish
                // 3 = available
                // 0 = blockage/ checked
                for (int y = 0; y < HEIGHT; y++)
                {
                    for (int x = 0; x < WIDTH; x++)
                    {
                        if (_map[x, y].Type == Tile.Terrain.Tree)
                            PathExists[x, y] = 0;
                        else
                            PathExists[x, y] = 3;
                    }
                }
                // Add start and finish
                int sX, sY;
                sX = _start.X;
                sY = _start.Y;
                PathExists[sX, sY] = 1;

                // Check all targets. If start it on one target, return nothing
                foreach(var target in _targets)
                {
                    if(PathExists[target.X, target.Y] != 0)
                    {
                        PathExists[target.X, target.Y] = 2;
                        if(_start == target)
                            return new Stack<Coordinate>();

                    }
                }
                // Fill enemies for blockages second
                foreach (var warrior in _warriors)
                {
                    PathExists[warrior.Position.X, warrior.Position.Y] = 0;
                }

                //Console.ReadLine();
                // Path to find
                Queue<Coordinate> Path = new Queue<Coordinate>();
                Queue<Coordinate> NodesToCheck = new Queue<Coordinate>();
                Queue<Coordinate> NodesToAdd = new Queue<Coordinate>();

                Path.Enqueue(new Coordinate(sX, sY));
                NodesToCheck.Enqueue(new Coordinate(sX, sY));

                Coordinate node = new Coordinate(0, 0);

                bool closestFound = false;

                int Distance = 0;
                while(!closestFound)
                {
                    Distance++;
                    while(NodesToCheck.Count > 0)
                    {
                        node = new Coordinate(NodesToCheck.Dequeue());
                        int X = node.X;
                        int Y = node.Y;
                        // North
                        if (Y - 1 >= 0 && PathExists[X, Y - 1] != 0 &&
                            !Path.Contains(new Coordinate(X, Y - 1)))
                        {
                            NodesToAdd.Enqueue(new Coordinate(X, Y - 1, new Coordinate(node)));
                            if (PathExists[X, Y - 1] == 2)
                            {
                                Targets.Add(new Coordinate(X, Y - 1, new Coordinate(node)), Distance);
                                closestFound = true;
                                break;
                            }
                                PathExists[X, Y - 1] = 0;
                        }
                        // West
                        if (X - 1 >= 0 && PathExists[X - 1, Y] != 0 &&
                            !Path.Contains(new Coordinate(X - 1, Y)))
                        {
                            NodesToAdd.Enqueue(new Coordinate(X - 1, Y, new Coordinate(node)));
                            if (PathExists[X - 1, Y] == 2)
                            {
                                Targets.Add(new Coordinate(X - 1, Y, new Coordinate(node)), Distance);
                                closestFound = true;
                                break;
                            }
                                PathExists[X - 1, Y] = 0;
                        }
                        // East
                        if (X + 1 < WIDTH && PathExists[X + 1, Y] != 0 &&
                            !Path.Contains(new Coordinate(X + 1, Y)))
                        {
                            NodesToAdd.Enqueue(new Coordinate(X + 1, Y, new Coordinate(node)));
                            if (PathExists[X + 1, Y] == 2)
                            {
                                Targets.Add(new Coordinate(X + 1, Y, new Coordinate(node)), Distance);
                                closestFound = true;
                                break;
                            }
                                PathExists[X + 1, Y] = 0;
                        }
                        // South
                        if (Y + 1 < HEIGHT && PathExists[X, Y + 1] != 0 &&
                            !Path.Contains(new Coordinate(X, Y + 1)))
                        {
                            NodesToAdd.Enqueue(new Coordinate(X, Y + 1, new Coordinate(node)));
                            if (PathExists[X, Y + 1] == 2)
                            {
                                Targets.Add(new Coordinate(X, Y + 1, new Coordinate(node)), Distance);
                                closestFound = true;
                                break;
                            }
                                PathExists[X, Y + 1] = 0;
                        }
                    }

                    

                    //NodesToCheck.Equals(NodesToAdd);
                    NodesToCheck = new Queue<Coordinate>(NodesToAdd);
                    if (NodesToAdd.Count == 0)
                        break;
                    
                    while(NodesToAdd.Count > 0)
                    {
                        Path.Enqueue(NodesToAdd.Dequeue());
                    }
                }

                // Check if path was in fact fount
                if(Targets.Count > 0)
                {
                    // Find smallest distance
                    int minDistance = Targets.Min(a => a.Value);
                    int TotalMin = Targets.Count(a => a.Value == minDistance);

                    List<KeyValuePair<Coordinate, int>> AllBest = Targets.Where(a => a.Value == minDistance).ToList();
                    List<Coordinate> temp = new List<Coordinate>();
                    foreach(var item in AllBest)
                    {
                        temp.Add(item.Key);
                    }
                    temp.Sort();

                    bool EndFound = false;

                    Coordinate previous = new Coordinate(temp[0]);
                    while(!EndFound)
                    {
                        Output.Push(previous);
                        if (previous.pX == 0)
                            EndFound = true;
                        else
                            previous = Path.First(a => a.X == previous.pX && a.Y == previous.pY);
                    }

                
                    Output.Pop();
                    return Output;
                }
                else
                {
                    return Output;
                }
            }

            public void Update(List<Warrior> _warriors, Tile[,] _map)
            {
                if (Health <= 0)
                    return;
                // Movement phase
                // Find locations in range
                List<Coordinate> InRange = new List<Coordinate>();
                List<Warrior> Enemies;
                if (GetType() == typeof(Elf))
                {
                    Enemies = _warriors.Where(a => a.type == "Goblin").ToList();

                    foreach (var enemy in Enemies)
                    {
                        InRange.Add(new Coordinate(enemy.Position.X + 1, enemy.Position.Y));
                        InRange.Add(new Coordinate(enemy.Position.X - 1, enemy.Position.Y));
                        InRange.Add(new Coordinate(enemy.Position.X, enemy.Position.Y + 1));
                        InRange.Add(new Coordinate(enemy.Position.X, enemy.Position.Y - 1));
                    }

                    // Check if warrior is within range
                    if (!InRange.Exists(a => a.X == Position.X && a.Y == Position.Y))
                        MoveToClosest(_map, InRange, _warriors);

                    // Check if in range again after movement and then attack if so
                    if (InRange.Exists(a => a.X == Position.X && a.Y == Position.Y))
                        Attack(Enemies);

                    // Change color for health
                    rect.FillColor = new Color(0, 0, 255, (byte)((Health / 200.0f) * 255));
                }
                else if (GetType() == typeof(Goblin))
                {
                    Enemies = _warriors.Where(a => a.type == "Elf").ToList();

                    foreach (var enemy in Enemies)
                    {
                        InRange.Add(new Coordinate(enemy.Position.X + 1, enemy.Position.Y));
                        InRange.Add(new Coordinate(enemy.Position.X - 1, enemy.Position.Y));
                        InRange.Add(new Coordinate(enemy.Position.X, enemy.Position.Y + 1));
                        InRange.Add(new Coordinate(enemy.Position.X, enemy.Position.Y - 1));
                    }


                    // Check if warrior is within range
                    if (!InRange.Exists(a => a.X == Position.X && a.Y == Position.Y))
                        MoveToClosest(_map, InRange, _warriors);

                    // Check range again 
                    if (InRange.Exists(a => a.X == Position.X && a.Y == Position.Y))
                        Attack(Enemies);



                    // Change color for health
                    rect.FillColor = new Color(255, 0, 0, (byte)((Health / 200.0f) * 255));
                }

                rect.Position = new Vector2f(Position.X, Position.Y) * SIZE;
                text.Position = new Vector2f(Position.X, Position.Y) * SIZE;


                //else
                //    Enemies = _warriors.Where(a => a.type == "Elf").ToList();

                // Find all available positions to move to   

                // Check which coordinates are reachable



            }

            private void Attack(List<Warrior> Enemies)
            {
                // Healths if exists
                Dictionary<char, float> Healths = new Dictionary<char, float>();
                // Coordinates
                Coordinate North = new Coordinate(Position.X, Position.Y - 1);
                Coordinate West = new Coordinate(Position.X - 1, Position.Y);
                Coordinate East = new Coordinate(Position.X + 1, Position.Y);
                Coordinate South = new Coordinate(Position.X, Position.Y + 1);
                // North
                if (Enemies.Exists(a => a.Position.X == North.X &&
                        a.Position.Y == North.Y && a.Health > 0))
                    Healths.Add('N', Enemies.First(a => a.Position.X == North.X &&
                        a.Position.Y == North.Y).Health);
                // West
                if (Enemies.Exists(a => a.Position.X == West.X &&
                        a.Position.Y == West.Y && a.Health > 0))
                    Healths.Add('W', Enemies.First(a => a.Position.X == West.X &&
                        a.Position.Y == West.Y).Health);
                // East
                if (Enemies.Exists(a => a.Position.X == East.X &&
                        a.Position.Y == East.Y && a.Health > 0))
                    Healths.Add('E', Enemies.First(a => a.Position.X == East.X &&
                        a.Position.Y == East.Y).Health);
                // South
                if (Enemies.Exists(a => a.Position.X == South.X &&
                        a.Position.Y == South.Y && a.Health > 0))
                    Healths.Add('S', Enemies.First(a => a.Position.X == South.X &&
                        a.Position.Y == South.Y).Health);
                // Was an enemy found?
                if(Healths.Count() > 0)
                {
                    // Check if more than one enemy has same health first
                    float minHealth = Healths.Aggregate((l, r) => l.Value < r.Value ? l : r).Value;
                    int minHealthCount = Healths.Where(a => a.Value == minHealth).ToList().Count();
                    if (minHealthCount == 1)
                    {
                        // See who to attack
                        char Attack = Healths.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                        switch (Attack)
                        {
                            case 'N':
                                Enemies.First(a => a.Position.X == North.X &&
                        a.Position.Y == North.Y).Damage(AttackPower);
                                break;
                            case 'W':
                                Enemies.First(a => a.Position.X == West.X &&
                        a.Position.Y == West.Y).Damage(AttackPower);
                                break;
                            case 'E':
                                Enemies.First(a => a.Position.X == East.X &&
                        a.Position.Y == East.Y).Damage(AttackPower);
                                break;
                            case 'S':
                                Enemies.First(a => a.Position.X == South.X &&
                        a.Position.Y == South.Y).Damage(AttackPower);
                                break;
                        }
                    }
                    else if (minHealthCount > 1)
                    {
                        var SameHealths = Healths.Where(a => a.Value == minHealth).ToList();
                        if (SameHealths.Exists(a => a.Key == 'N'))
                            Enemies.First(a => a.Position.X == North.X &&
                                a.Position.Y == North.Y).Damage(AttackPower);
                        else if (SameHealths.Exists(a => a.Key == 'W'))
                            Enemies.First(a => a.Position.X == West.X &&
                                a.Position.Y == West.Y).Damage(AttackPower);
                        else if (SameHealths.Exists(a => a.Key == 'E'))
                            Enemies.First(a => a.Position.X == East.X &&
                                a.Position.Y == East.Y).Damage(AttackPower);
                        else if (SameHealths.Exists(a => a.Key == 'S'))
                            Enemies.First(a => a.Position.X == South.X &&
                                a.Position.Y == South.Y).Damage(AttackPower);
                    }

                }
            }

            private void MoveToClosest(Tile[,] _map, List<Coordinate> InRange, List<Warrior> Enemies)
            {
                var path = CheckPathExists(Enemies, _map, Position, InRange);
                //Console.WriteLine("Start: " + Position.ToString());
                //Console.WriteLine("Closest: " + path.First().ToString());// = {CheckPathExists(Enemies, _map, Position, InRange)}");

                if (path.Count > 0)
                {
                    Position = new Coordinate(path.Pop());
                }

            }

            public int CompareTo(object obj)
            {
                Warrior other = (Warrior)obj;
                if (Position.Y < other.Position.Y)
                    return -1;
                else if (Position.Y > other.Position.Y)
                    return 1;
                else
                {
                    if (Position.X < other.Position.X)
                        return -1;
                    else if (Position.X > other.Position.X)
                        return 1;
                    else
                        return 0;
                }
            }
        }

        public class Elf : Warrior
        {
            public Elf(int x, int y) : base(x, y)
            {
                type = "Elf";
                rect.FillColor = new Color(0, 0, 255);
            }
        }

        public class Goblin : Warrior
        {
            public Goblin(int x, int y) : base(x, y)
            {
                type = "Goblin";
                rect.FillColor = new Color(255, 0, 0);
            }
        }


        public class Tile
        {
            public enum Terrain { Tree, Land };
            public Terrain Type;
            public Coordinate Position;
            public RectangleShape rect;
            public Tile(int x, int y, Terrain type)
            {
                Position = new Coordinate(x, y);
                Type = type;

                rect = new RectangleShape(new Vector2f(1.0f, 1.0f) * SIZE);
                rect.Position = new Vector2f(x, y) * SIZE;

                if (type == Terrain.Land)
                    rect.FillColor = new Color(0, 255, 0);
                else
                    rect.FillColor = new Color(0, 120, 0);

            }

           
        }

        public void Game(List<string> _input)
        {
            // Find dimensions of map
            WIDTH = _input[0].Length;
            HEIGHT = _input.Count;
            SIZE = 1000 / WIDTH;

            // SFML
            RenderWindow window = new RenderWindow(
                new SFML.Window.VideoMode(1000, 1000),
                "Day 15");

            window.SetActive();

            Application.EnableVisualStyles();

            // Map
            Tile[,] Map = new Tile[WIDTH, HEIGHT];
            // Warriors
            List<Warrior> Warriors = new List<Warrior>();

            // Fill map
            for(int y = 0; y < HEIGHT; y++)
            {
                char[] temp = _input[y].ToCharArray();
                for(int x = 0; x < WIDTH; x++)
                {
                    if(temp[x] == '#')
                    {
                        Map[x, y] = new Tile(x, y, Tile.Terrain.Tree);
                    }
                    else
                    {
                        Map[x, y] = new Tile(x, y, Tile.Terrain.Land);
                        if(temp[x] == 'G')
                        {
                            Warriors.Add(new Goblin(x, y));
                        }
                        else if(temp[x] == 'E')
                        {
                            Warriors.Add(new Elf(x, y));
                        }
                    }
                }
            }

            int TotalRounds = 0;
            int HitPoints = 0;
            int Score = 0;

            Text RoundCount = new Text($"Round = {TotalRounds}", new Font("Input\\Venator.ttf"), (uint)SIZE / 2);
            RoundCount.Position = new Vector2f(0, 0) * SIZE;
            RoundCount.Color = new Color(0, 0, 0);
            bool GameRunning = true;

            window.Clear();
            window.DispatchEvents();
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    window.Draw(Map[x, y].rect);
                }
            }
            Warriors.ForEach(a => window.Draw(a.rect));
            window.Draw(RoundCount);
            window.Display();

            while (GameRunning)
            {
                Thread.Sleep(100);

                TotalRounds++;

                // First sort the warriors in order of location
                Warriors.Sort();

                // Let every warrior take its turn in moving and then fighting
                for(int i = 0; i < Warriors.Count; i++)
                {
                    Warriors[i].Update(Warriors, Map);
                    
                }
                Warriors.RemoveAll(a => a.Health <= 0);



                window.Clear();
                window.DispatchEvents();
                for (int x = 0; x < WIDTH; x++)
                {
                    for (int y = 0; y < HEIGHT; y++)
                    {
                        window.Draw(Map[x, y].rect);
                    }
                }
                Warriors.ForEach(a => window.Draw(a.rect));
                Warriors.ForEach(a => window.Draw(a.text));
                RoundCount.DisplayedString = $"Round = {TotalRounds}";
                window.Draw(RoundCount);
                
                window.Display();


                // check if winner
                // Check both teams
                if (Warriors.Where(a => a.type == "Elf").ToList().Count() == 0)
                {
                    GameRunning = false;
                    HitPoints = (int)Warriors.Where(a => a.type == "Goblin").ToList().Sum(a => a.Health);
                    Score = HitPoints * TotalRounds;
                    Console.WriteLine($"Combat ends after {TotalRounds} full rounds.");
                    Console.WriteLine($"Goblins win with {HitPoints} total hit points left.");
                    Console.WriteLine($"Outcome: {TotalRounds} * {HitPoints} = {Score}");

                    break;
                }
                else if (Warriors.Where(a => a.type == "Goblin").ToList().Count() == 0)
                {
                    GameRunning = false;
                    HitPoints = (int)Warriors.Where(a => a.type == "Elf").ToList().Sum(a => a.Health);
                    Score = HitPoints * TotalRounds;
                    Console.WriteLine($"Combat ends after {TotalRounds} full rounds.");
                    Console.WriteLine($"Elves win with {HitPoints} total hit points left.");
                    Console.WriteLine($"Outcome: {TotalRounds} * {HitPoints} = {Score}");

                    break;
                }

                Console.ReadLine();

            }

        }
    }
}
