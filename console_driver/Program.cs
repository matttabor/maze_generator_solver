using System;
using System.Collections.Generic;
using Maze.Models;
using Maze.Solvers;

namespace console_driver
{
    class Program
    {
        static void Main(string[] args)
        {
             // TODO: Read N x N size in as an input
            var size = 15;
            var maze = new MazeModel(size);
            Console.WriteLine();
            Console.WriteLine();
            maze.GenerateMaze();
            PrintMaze(maze);

            var solver = MazeSolverFactory.CreateMazeSolver(MazeSolverType.BFS);

            var solution = solver.SolveMaze(maze);
            PrintSolution(maze, solution);
        }

        static void PrintMaze(MazeModel maze)
        {
            var size = maze.Size;
            // print the north border first
            for (int i = 0; i < size; i++)
            {
                if (maze.GetRoomAtIndex(i).Door0 == true)
                {
                    Console.Write("+----");
                }
                else
                {
                    Console.Write("+    ");
                }
            }
            Console.WriteLine("+");

            // print the interior walls
            for (int i = 0; i < size * size; i += size) // rows
            {
                for (int j = i; j < i + size; ++j) // Prints the west wall
                {
                    if (maze.GetRoomAtIndex(j).Door3)
                    {
                        Console.Write("|    ");
                    }
                    else
                    {
                        Console.Write("     ");
                    }
                }
                Console.WriteLine("|"); // Right most border

                for (int j = i; j < i + size; ++j) // Prints the south wall
                {
                    if (maze.GetRoomAtIndex(j).Door1)
                    {
                        Console.Write("+----");
                    }
                    else
                    {
                        Console.Write("+    ");
                    }
                }
                Console.WriteLine("+"); // Right most border
            }
        }

        static void PrintMaze(MazeModel maze, HashSet<int> hash)
        {
            var size = maze.Size;
            // print the north border first
            Console.WriteLine("\n\n");
            
            for (int i = 0; i < size; i++)
            {
                if (maze.GetRoomAtIndex(i).Door0 == true)
                {
                    Console.Write("+----");
                }
                else
                {
                    Console.Write("+    ");
                }
            }
            Console.WriteLine("+");

            var actualIndex = 0;
            // print the interior walls
            for (int i = 0; i < size * size; i += size) // rows
            {
                for (int j = i; j < i + size; ++j) // Prints the west wall
                {
                    Console.ResetColor();
                    var index = i + j;
                    if (maze.GetRoomAtIndex(j).Door3 && hash.Contains(actualIndex))
                    {
                        Console.Write("| "); 
                        Console.ForegroundColor = System.ConsoleColor.Red;
                        Console.Write("X  ");
                    }
                    else if (maze.GetRoomAtIndex(j).Door3 && !hash.Contains(actualIndex))
                    {
                        Console.Write("|    ");
                    }
                    else
                    {
                        if (hash.Contains(actualIndex))
                        {
                            Console.Write("  ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("X  ");
                        }
                        else
                        {
                            Console.Write("     ");
                        }
                    }

                    actualIndex++;
                }
                Console.ResetColor();
                Console.WriteLine("|"); // Right most border
                
                for (int j = i; j < i + size; ++j) // Prints the south wall
                {
                    if (maze.GetRoomAtIndex(j).Door1)
                    {
                        Console.Write("+----");
                    }
                    else
                    {
                        Console.Write("+    ");
                    }
                }
                Console.WriteLine("+"); // Right most border
            }
        }

        private static void PrintSolution(MazeModel maze, SortedDictionary<int, int> solution)
        {
            int start = 0;
            maze.MarkAllRoomsAsUnvisited();
            var hash = new HashSet<int>();
            hash.Add(start);

            maze.GetRoomAtIndex(0).Visited = true;
            // Console.WriteLine("The path in reverse is: ");
            int cell = maze.MaxIndex;
            while (cell != start)
            {
                // Console.Write($"  {cell}  ");
                hash.Add(cell);
                maze.GetRoomAtIndex(cell).Visited = true;
                cell = solution[cell];
            }
            // Console.WriteLine($"  {cell}  ");
            maze.GetRoomAtIndex(cell).Visited = true;

            // // print ascii solution
            // Console.WriteLine("This is the solution");
            // for (int i = 0; i < maze.NumOfRooms; ++i)
            // {
            //     if (maze.GetRoomAtIndex(i).Visited == true)
            //         Console.Write("x");
            //     else
            //         Console.Write(" ");
            //     if ((i + 1) % maze.Size == 0)
            //         Console.WriteLine();
            // }

            PrintMaze(maze, hash);
        }
    }
}
