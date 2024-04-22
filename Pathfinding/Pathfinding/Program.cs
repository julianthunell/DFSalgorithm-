using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace Pathfinding
{
    class Program
    {
        static int width = 25;
        static int height = 25;
        static char[,] maze = new char[width, height];
        const string fileName = "HuntAndKill.bin";

        static Stack<int> prevX = new Stack<int>();
        static Stack<int> prevY = new Stack<int>();

        static int[] testedDirections = new int[4];

        static int mazeNumber = 0;
        static int start = mazeNumber * 625;

        static int x;
        static int y;

        static int xGoal;
        static int yGoal;

        static int stepLength = 2;
        static int totalSteps;
        static long totalTime;

        static string answer;


        static void Main(string[] args)
        {
            Console.WriteLine("1. Maze test");
            Console.WriteLine("2. Time test");
            Console.WriteLine("3. Mase show");
            answer = Console.ReadLine();


            if(answer == "1")
            {
                ReadFile();
                selectGoal(12, 12);
                solveMaze();
            }
            else if(answer =="2")
            {
                totalTime = 0;
                totalSteps = 0;
                int mazeAmount = 300;
                for (int i = 0; i < mazeAmount; i++)
                {
                    Console.WriteLine("Maze: " + i);
                    ReadFile();
                    selectGoal(12, 12);
                    solveMaze();
                    mazeNumber++;
                }
                Console.WriteLine("Everything Completed! | Avg Steps: " + totalSteps / mazeAmount + " | Avg Time: " + totalTime / mazeAmount + "ms");
            }
            else if (answer == "3")
            {
                while (true)
                {
                    ReadFile();
                    printMaze();
                    Console.WriteLine("New?");
                    Console.ReadLine();
                    mazeNumber++;
                }
                
            }
            
        }

        private static void selectGoal(int x, int y)
        {
            xGoal = x;
            yGoal = y;
        }

        public static void solveMaze()
        {

            x = 0;
            y = 0;

            int steps = 0;
            DateTime startTime = DateTime.Now;
            while (x != xGoal | y != yGoal)
            {
                steps++;
                maze[x, y] = 'P';
                move(randomAnalyse(x,y));
                if(answer == "1")
                {
                    Thread.Sleep(100);
                    Console.Clear();
                    printMaze();
                }
            }
            TimeSpan elapsedTime = DateTime.Now - startTime;
            totalSteps += steps;
            totalTime += elapsedTime.Ticks;
            Console.WriteLine("Complete! Steps: "+steps+" Time: "+ elapsedTime.Ticks + "ms");


        }
        private static void move(string direction)
        {
            
            switch (direction)
            {
                case "right":
                    prevX.Push(x);
                    prevY.Push(y);
                    x+=stepLength;
                    break;
                case "left":
                    prevX.Push(x);
                    prevY.Push(y);
                    x-=stepLength;
                    break;
                case "down":
                    prevX.Push(x);
                    prevY.Push(y);
                    y+=stepLength;
                    break;
                case "up":
                    prevX.Push(x);
                    prevY.Push(y);
                    y-=stepLength;
                    break;
                case "noPath":
                    prevX.Pop();
                    prevY.Pop();

                    x = prevX.Peek();
                    y = prevY.Peek();
                    break;
            }
        }

        private static string randomAnalyse(int x, int y)
        {
            bool moved = false;
            int direction = 0;
            int nonMove = 0;
            Boolean startMove = false;

            while (moved == false)
            {
                while (startMove == false)
                {
                    direction = randomNumber(1, 5);
                    if (testedDirections.Contains(direction))
                    {
                    }
                    else
                    {
                        startMove = true;
                    }

                }
                switch (direction)
                {
                    case 1:
                        if (x < width - stepLength && maze[x + stepLength, y] == '0' && maze[x + stepLength-1, y] == '0')
                        {
                            Array.Clear(testedDirections, 0, testedDirections.Length);
                            return "right";
                        }

                        break;
                    case 2:
                        if (x > 0 && maze[x - stepLength, y] == '0' && maze[x - stepLength -1, y] == '0')
                        {
                            Array.Clear(testedDirections, 0, testedDirections.Length);
                            return "left";
                        }

                        break;
                    case 3:
                        if (y < height - 1 && maze[x, y+stepLength] == '0' && maze[x, y + stepLength-1] == '0')
                        {
                            Array.Clear(testedDirections, 0, testedDirections.Length);
                            return "down";

                        }
                        break;
                    case 4:
                        if (y>0 && maze[x, y - stepLength] == '0' && maze[x, y - stepLength-1] == '0')
                        {
                            Array.Clear(testedDirections, 0, testedDirections.Length);
                            return "up";
                        }
                        break;
                }
                testedDirections[nonMove] = direction;
                nonMove++;
                startMove = false;


                if (nonMove > 3)
                {
                    nonMove = 0;
                    Array.Clear(testedDirections, 0, testedDirections.Length);
                    return "noPath";
                }
            }
            Console.WriteLine("no");
            return "no";
        }

        static int randomNumber(int start, int end)
        {
            Random rnd = new Random();
            return rnd.Next(start, end);
        }
        static void ReadFile()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (File.Exists(fileName))
                    {
                        using (var stream = File.Open(fileName, FileMode.Open))
                        {
                            using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                            {
                                reader.BaseStream.Seek(start, SeekOrigin.Begin);
                                char character = reader.ReadChar();

                                start++;

                                maze[j, i] = character;
                            }
                        }

                    }
                }
            }
        }
        static void printMaze()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if(maze[j,i] == '0')
                    {
                        Console.Write("██");
                    }
                    else if (maze[j,i] == 'P')
                    {
                        Console.Write("PP");
                    }
                    else if (maze[j, i] == 'G')
                    {
                        Console.Write("GG");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
