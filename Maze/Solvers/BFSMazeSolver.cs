using System.Collections.Generic;
using System;
using Maze.Models;

namespace Maze.Solvers
{
    /// <summary>
    /// Maze solver that uses the breadth first search algorthm to find a solution
    /// </summary>
    public class BFSMazeSolver : IMazeSolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maze"></param>
        /// <returns></returns>
        public SortedDictionary<int, int> SolveMaze(MazeModel maze)
        {
            var queue = new Queue<int>();
            var vistedRooms = new HashSet<int>();
            var solution = new SortedDictionary<int, int>();

            var goalIndex = maze.MaxIndex;
            var startIndex = 0;
            int row, col, nextRow, nextCol, nextCell;

            queue.Enqueue(startIndex);
            vistedRooms.Add(startIndex);

            while (queue.Count >= 0)
            {
                var cell = queue.Dequeue();
                if (cell == goalIndex)
                {
                    break;
                }

                row = cell / maze.Size;
                col = cell % maze.Size;

                // Check North
                nextRow = row - 1;
                nextCol = col;
                if (nextRow >= 0)
                {
                    if (maze.GetCellAtIndex(cell).North.IsOpen)
                    {
                        nextCell = nextRow * maze.Size + nextCol;
                        if(!vistedRooms.Contains(nextCell))
                        {
                            queue.Enqueue(nextCell);
                            vistedRooms.Add(nextCell);
                            solution.Add(nextCell, cell);
                        }
                    }
                }

                // Check South
                nextRow = row + 1;
                nextCol = col;
                if (nextRow < maze.Size)
                {
                    if (maze.GetCellAtIndex(cell).South.IsOpen)
                    {
                        nextCell = nextRow * maze.Size + nextCol;
                        if(!vistedRooms.Contains(nextCell))
                        {
                            queue.Enqueue(nextCell);
                            vistedRooms.Add(nextCell);
                            solution.Add(nextCell, cell);
                        }
                    }
                }

                //check east
                nextRow = row;
                nextCol = col + 1;
                if( nextCol < maze.Size ){
                    if(maze.GetCellAtIndex(cell).East.IsOpen){
                        nextCell = nextRow * maze.Size + nextCol;
                        if(!vistedRooms.Contains(nextCell)) 
                        {
                            queue.Enqueue( nextCell );
                            vistedRooms.Add(nextCell);
                            solution.Add(nextCell, cell);
                        }
                    }
                }

                // check west
                nextRow = row;
                nextCol = col - 1;
                if(nextCol >= 0 ){
                    if(maze.GetCellAtIndex(cell).West.IsOpen){
                        nextCell = nextRow * maze.Size + nextCol;
                        if(!vistedRooms.Contains(nextCell))
                        {
                            queue.Enqueue( nextCell );
                            vistedRooms.Add(nextCell);
                            solution.Add(nextCell, cell);
                        }
                    }
                }
            }

            return solution;
        }
    }
}