using System.Collections.Generic;
using Maze.Models;

namespace Maze.Solvers
{
    public interface IMazeSolver
    {
         SortedDictionary<int, int> SolveMaze(MazeModel maze);
    }
}