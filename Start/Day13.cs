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

    class Day13
    {
        public static float SIZE = 7.0f;
        //public const float SIZE = 50;

        public class Track
        {
            public enum TrackType { EMPTY, HORIZONTAL, VERTICAL, CROSS, CORNERNE, CORNERSE };
            public TrackType Type;
            public int X;
            public int Y;

            public Shape Shape;

            public Track(int x, int y, TrackType type)
            {
                X = x;
                Y = y;
                Type = type;

                Shape = new RectangleShape(new Vector2f(1.0f, 1.0f) * SIZE);

                if (type == TrackType.HORIZONTAL || type == TrackType.VERTICAL)
                {
                    Shape.FillColor = new Color(204, 81, 39);

                    if(type == TrackType.HORIZONTAL)
                    {
                        Shape.Scale = new Vector2f(1.0f, 0.2f);                   
                        Shape.Position = new Vector2f(X, (Y + 0.4f)) * SIZE;

                    }
                    else
                    {
                        Shape.Scale = new Vector2f(0.2f, 1.0f);
                        Shape.Position = new Vector2f((X + 0.4f) * SIZE, Y * SIZE);
                    }

                }
                else if(type == TrackType.CROSS)
                {
                    //rect = new RectangleShape(new Vector2f(1.0f * SIZE, 1.0f * SIZE));
                    Shape.Scale = new Vector2f(0.2f, 0.2f);
                    Shape.Position = new Vector2f(X + 0.4f, Y + 0.4f) * SIZE;
                    Shape.FillColor = new Color(255, 0, 0);
                    //rect.Scale = new Vector2f(0.3f, 0.3f);
                }
                else if(type == TrackType.CORNERNE)
                {
                    ConvexShape corner = new ConvexShape(4);
                    corner.SetPoint(0, new Vector2f(0.9f, 0.8f) * SIZE);
                    corner.SetPoint(1, new Vector2f(0.8f, 0.9f) * SIZE);
                    corner.SetPoint(2, new Vector2f(0.1f, 0.2f) * SIZE);
                    corner.SetPoint(3, new Vector2f(0.2f, 0.1f) * SIZE);

                    Shape = corner;
                    Shape.FillColor = new Color(255, 0, 0);
                    Shape.Position = new Vector2f(X, Y) * SIZE;
                }
                else if (type == TrackType.CORNERSE)
                {
                    ConvexShape corner = new ConvexShape(4);
                    corner.SetPoint(0, new Vector2f(0.1f, 0.8f) * SIZE);
                    corner.SetPoint(1, new Vector2f(0.8f, 0.1f) * SIZE);
                    corner.SetPoint(2, new Vector2f(0.9f, 0.2f) * SIZE);
                    corner.SetPoint(3, new Vector2f(0.2f, 0.9f) * SIZE);

                    Shape = corner;
                    Shape.FillColor = new Color(255, 0, 0);
                    Shape.Position = new Vector2f(X, Y) * SIZE;
                }
                else
                {
                    Shape.FillColor = new Color(0, 0, 0, 50);
                    Shape.Position = new Vector2f(X * SIZE, Y * SIZE);
                }


            }

        }

        public class Kart : IComparable
        {
            public enum Direction { LEFT, DOWN, RIGHT, UP };
            public Direction KartDirection;
            public int X;
            public int Y;
            public CircleShape Shape;
            public int JunctionTurns;
            public bool Collided;

            public Kart(int x, int y, Direction direction)
            {
                X = x;
                Y = y;
                KartDirection = direction;
                JunctionTurns = 0;
                Collided = false;

                Shape = new CircleShape(0.2f * SIZE);
                Shape.Position = new Vector2f(X + 0.3f, Y + 0.3f) * SIZE;
                Shape.FillColor = new Color(255, 255, 255);
            }

            public void UpdateRadius(float radius)
            {
                float leftover = (1 - radius) / 2;

                Shape.Radius = (radius * SIZE);
                Shape.Position = new Vector2f(X + leftover, Y + leftover) * SIZE;
            }

            private void UpdateLocation()
            {
                if (KartDirection == Direction.UP)
                    Y--;
                if (KartDirection == Direction.DOWN)
                    Y++;
                if (KartDirection == Direction.LEFT)
                    X--;
                if (KartDirection == Direction.RIGHT)
                    X++;


                Shape.Position = new Vector2f(X + 0.3f, Y + 0.3f) * SIZE;
            }

            public void Update(Track[,] map)
            {
                if (!Collided)
                {
                    if (map[X, Y].Type == Track.TrackType.HORIZONTAL ||
                map[X, Y].Type == Track.TrackType.VERTICAL)
                    {
                        // Do Nothing
                    }
                    else if (map[X, Y].Type == Track.TrackType.CORNERNE)
                    {
                        if (KartDirection == Direction.UP)
                        {
                            KartDirection = Direction.LEFT;

                        }
                        else if (KartDirection == Direction.DOWN)
                        {
                            KartDirection = Direction.RIGHT;

                        }
                        else if (KartDirection == Direction.LEFT)
                        {
                            KartDirection = Direction.UP;

                        }
                        else if (KartDirection == Direction.RIGHT)
                        {
                            KartDirection = Direction.DOWN;

                        }
                    }
                    else if (map[X, Y].Type == Track.TrackType.CORNERSE)
                    {
                        if (KartDirection == Direction.UP)
                        {
                            KartDirection = Direction.RIGHT;

                        }
                        else if (KartDirection == Direction.DOWN)
                        {
                            KartDirection = Direction.LEFT;

                        }
                        else if (KartDirection == Direction.LEFT)
                        {
                            KartDirection = Direction.DOWN;

                        }
                        else if (KartDirection == Direction.RIGHT)
                        {
                            KartDirection = Direction.UP;

                        }
                    }
                    else
                    {
                        int temp = (int)KartDirection;
                        if (JunctionTurns % 3 == 0)
                        {
                            temp += 1;
                        }
                        else if (JunctionTurns % 3 == 1)
                        {
                            temp += 0;
                        }
                        else if (JunctionTurns % 3 == 2)
                        {
                            temp += 3;
                        }

                        KartDirection = (Direction)(temp % 4);



                        JunctionTurns++;
                    }




                    UpdateLocation(); 
                }

            }

            public int CompareTo(object obj)
            {
                Kart other = (Kart)obj;
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


        public void Execute()
        {
            // Generate random

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~         Day 13        -");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("");

            // Load text file
            string fileContent = File.ReadAllText("Input\\Day13Input.txt");
            // Split string into an array
            string[] fileContentSplit =
                fileContent.Split(new char[] {'\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            List<string> lines = new List<string>();
            lines = fileContentSplit.ToList();

            // Print answers
            Console.WriteLine("Finding cart crashes...");
            FindCrash(lines);
            //Console.WriteLine("Finding energy with mixed size...");
            //PartTwo();
        }


        private void FindCrash(List<string> tracks)
        {
            // First find width and height of map
            int WIDTH = tracks.Max(a => a.Length);
            int HEIGHT = tracks.Count();

            SIZE = 1050 / WIDTH;

            Console.WriteLine($"Dimensions: {WIDTH}x{HEIGHT}");

            List<Kart> Karts = new List<Kart>();
            Track[,] Map = new Track[WIDTH, HEIGHT];
            // Initialise map
            for(int x = 0; x < WIDTH; x++)
            {
                for(int y = 0; y < HEIGHT; y++)
                {
                    Map[x, y] = new Track(x, y, Track.TrackType.EMPTY);
                }
            }

            // Fill map
            int index = 0;
            foreach(var track in tracks)
            {
                char[] temp = track.ToCharArray();
                for(int x = 0; x < track.Count(); x++)
                {
                    // Check if vertical
                    if(temp[x] == '|' || temp[x] == 'v' || temp[x] == '^')
                    {
                        Map[x, index] = new Track(x, index, Track.TrackType.VERTICAL);

                        if (temp[x] == 'v')
                            Karts.Add(new Kart(x, index, Kart.Direction.DOWN));
                        else if (temp[x] == '^')
                            Karts.Add(new Kart(x, index, Kart.Direction.UP));
                    }
                    else if (temp[x] == '-' || temp[x] == '<' || temp[x] == '>')
                    {
                        Map[x, index] = new Track(x, index, Track.TrackType.HORIZONTAL);

                        if (temp[x] == '<')
                            Karts.Add(new Kart(x, index, Kart.Direction.LEFT));
                        else if (temp[x] == '>')
                            Karts.Add(new Kart(x, index, Kart.Direction.RIGHT));
                    }
                    else if (temp[x] == '+')
                    {
                        Map[x, index] = new Track(x, index, Track.TrackType.CROSS);
                    }
                    else if(temp[x] == '/')
                    {
                        Map[x, index] = new Track(x, index, Track.TrackType.CORNERSE);
                    }
                    else if (temp[x] == '\\')
                    {
                        Map[x, index] = new Track(x, index, Track.TrackType.CORNERNE);
                    }
                }
                index++;
            }


            //RenderWindow window = new RenderWindow(
            //    new SFML.Window.VideoMode((uint)(WIDTH * SIZE),(uint)(HEIGHT * SIZE)),
            //    "Day 13");


            RenderWindow window = new RenderWindow(
                new SFML.Window.VideoMode(1050, 1050),
                "Day 13");

            window.SetActive();

            Application.EnableVisualStyles();


            window.Clear();
            window.DispatchEvents();
            for(int x = 0; x < WIDTH; x++)
            {
                for(int y = 0; y < HEIGHT; y++)
                {
                      window.Draw(Map[x,y].Shape);
                }
            }
            Karts.ForEach(a => window.Draw(a.Shape));

            window.Display();

            Thread.Sleep(3000);

            int TotalKarts = Karts.Count();
            int TotalCollisions = 0;

            //for(int i = 0; i < 30; i++)
            while(true)
            {
                Karts.Sort();
                for (int k = 0; k < TotalKarts; k++)
                {
                    Karts[k].Update(Map);
                    // Check for collision
                    for(int i = 0; i < Karts.Count(); i++)
                    {
                        for(int j = i + 1; j < Karts.Count(); j++)
                        {
                            if(Karts[i].X == Karts[j].X &&
                                Karts[i].Y== Karts[j].Y &&
                                !Karts[i].Collided &&
                                !Karts[j].Collided)
                            {
                                Karts[i].Collided = true;
                                Karts[j].Collided = true;
                                Karts[i].Shape.FillColor = new Color(0, 0, 255, 100);
                                Karts[j].Shape.FillColor = new Color(0, 0, 255, 100);
                                Karts[i].UpdateRadius(1.0f);
                                Karts[j].UpdateRadius(1.0f);

                                Console.WriteLine($"Collision at {Karts[i].X},{Karts[i].Y}");
                                TotalCollisions += 2;
                            }
                        }
                    }
                }
                ////Karts.ForEach(a => a.Update(Map));




                if (false)
                {
                    window.Clear();
                    window.DispatchEvents();
                    for (int x = 0; x < WIDTH; x++)
                    {
                        for (int y = 0; y < HEIGHT; y++)
                        {
                            window.Draw(Map[x, y].Shape);
                        }
                    }
                    Karts.ForEach(a => window.Draw(a.Shape));

                    window.Display(); 
                }

                // Stop Code PART ONE
                if (TotalCollisions == TotalKarts)
                    break;

                // Stop Code PART TWO
                if (TotalKarts - TotalCollisions == 1)
                {
                    Kart Last = Karts.Find(a => a.Collided == false);
                    Console.WriteLine($"The last kart's position is: {Last.X},{Last.Y}");
                    break;
                    
                }
                //Thread.Sleep(500);  
            }

            Console.WriteLine("Everything has collided!");
            Thread.Sleep(3000);

        }

    }
}
