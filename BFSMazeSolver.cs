using System.Collections.Generic;
using System;

namespace maze_generator
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
        public SortedDictionary<int, int> SolveMaze(Maze maze)
        {
            var queue = new Queue<int>();
            maze.MarkAllRoomsAsUnvisited();
            var solution = new SortedDictionary<int, int>();

            var goalIndex = maze.MaxIndex;
            var startIndex = 0;
            int row, col, nextRow, nextCol, nextCell;

            queue.Enqueue(startIndex);
            maze.GetRoomAtIndex(startIndex).Visited = true;

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
                    if (!maze.GetRoomAtIndex(cell).Door0)
                    {
                        nextCell = nextRow * maze.Size + nextCol;
                        if (maze.GetRoomAtIndex(nextCell).Visited == false)
                        {
                            queue.Enqueue(nextCell);
                            maze.GetRoomAtIndex(nextCell).Visited = true;
                            solution.Add(nextCell, cell);
                        }
                    }
                }

                // Check South
                nextRow = row + 1;
                nextCol = col;
                if (nextRow < maze.Size)
                {
                    if (!maze.GetRoomAtIndex(cell).Door1)
                    {
                        nextCell = nextRow * maze.Size + nextCol;
                        if (!maze.GetRoomAtIndex(nextCell).Visited)
                        {
                            queue.Enqueue(nextCell);
                            maze.GetRoomAtIndex(nextCell).Visited = true;
                            solution.Add(nextCell, cell);
                        }
                    }
                }

                //check east
                nextRow = row;
                nextCol = col + 1;
                if( nextCol < maze.Size ){
                    if(!maze.GetRoomAtIndex(cell).Door2){
                        nextCell = nextRow * maze.Size + nextCol;
                        if(!maze.GetRoomAtIndex(nextCell).Visited) {
                            queue.Enqueue( nextCell );
                            maze.GetRoomAtIndex(nextCell).Visited = true;
                            solution.Add(nextCell, cell);
                        }
                    }
                }

                // check west
                nextRow = row;
                nextCol = col - 1;
                if(nextCol >= 0 ){
                    if(!maze.GetRoomAtIndex(cell).Door3){
                        nextCell = nextRow * maze.Size + nextCol;
                        if(!maze.GetRoomAtIndex(nextCell).Visited) {
                            queue.Enqueue( nextCell );
                            maze.GetRoomAtIndex(nextCell).Visited = true;
                            solution.Add(nextCell, cell);
                        }
                    }
                }
            }

            return solution;
        }
    }
}