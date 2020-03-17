using System;

namespace Maze.Solvers
{
    public static class MazeSolverFactory
    {
        public static IMazeSolver CreateMazeSolver(MazeSolverType type)
        {
            switch(type)
            {
                case MazeSolverType.BFS:
                    return new BFSMazeSolver();
                case MazeSolverType.DFS:
                    return new DFSMazeSolver();
                default:
                    throw new Exception("Unsupported type");
            }
        }
    }
}