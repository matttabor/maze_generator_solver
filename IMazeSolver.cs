using System.Collections.Generic;

namespace maze_generator
{
    public interface IMazeSolver
    {
         SortedDictionary<int, int> SolveMaze(Maze maze);
    }
}