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
                if (maze.GetCellAtIndex(i).North.IsClosed)
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
                    if (maze.GetCellAtIndex(j).West.IsClosed)
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
                    if (maze.GetCellAtIndex(j).South.IsClosed)
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
                if (maze.GetCellAtIndex(i).North.IsClosed)
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
                    if (maze.GetCellAtIndex(j).West.IsClosed && hash.Contains(actualIndex))
                    {
                        Console.Write("| "); 
                        Console.ForegroundColor = System.ConsoleColor.Red;
                        Console.Write("X  ");
                    }
                    else if (maze.GetCellAtIndex(j).West.IsClosed && !hash.Contains(actualIndex))
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
                    if (maze.GetCellAtIndex(j).South.IsClosed)
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
            var hash = new HashSet<int>();
            hash.Add(start);

            int cell = maze.MaxIndex;
            while (cell != start)
            {
                hash.Add(cell);
                cell = solution[cell];
            }

            // Display the ascii repesentation of the maze
            PrintMaze(maze, hash);
        }
    }
}
