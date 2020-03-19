using System;
using System.Collections.Generic;
using Maze.DisjointSets;

namespace Maze.Models
{
    public class MazeModel
    {
        private int _size;
        public int MaxIndex => NumOfRooms - 1;
        /// <summary>
        /// Number of rows and colums
        /// </summary>
        public int Size => _size;
        public int NumOfRooms => _size * _size;
        public Cell[] Rooms { get; set; }

        public void GenerateMaze()
        {
            var set = new DisjointSet(_size * _size);
            int maxIndex = _size * _size - 1;
            int room1, room2, doorNum;
            var rand = new Random();
            var visitedRooms = new HashSet<int>();

            // while start and end are not in the same set AND while everyroom has not been visited yet
            while ((set.Find(0) != set.Find(maxIndex)) && visitedRooms.Count < NumOfRooms)
            {
                // pick a random room
                room1 = rand.Next(maxIndex);

                // pick a random room adjacent to room1
                // several restrictions apply to what cell can
                // be chosen based upon where room1 is located
                if (room1 < _size) // room1 is in first row
                {
                    if (room1 % _size == 0) //room1 is in first row first col
                    {
                        doorNum = rand.Next(1);   // only two doors availible - south and east
                        if (doorNum == 0)
                            doorNum = 2;
                        else
                            doorNum = 1;
                    }
                    else if ((room1 + 1) % _size == 0)
                    {   // room1 is in first row last col
                        doorNum = rand.Next(1); // only two doors availible - south and west
                        if (doorNum == 0)
                            doorNum = 1;
                        else
                            doorNum = 3;
                    }
                    else // door is somewhere in row 1
                    {                      
                        doorNum = rand.Next(3) + 1; // only three doors availible - south, east and west
                    }
                }
                else if (room1 > maxIndex - _size)
                {   // room1 is in last row
                    if (room1 % _size == 0)
                    {           //room1 is in last row first col
                        doorNum = rand.Next(1);           // only two doors availible - north and east
                        if (doorNum == 1)
                            doorNum = 2;
                    }
                    else if ((room1 + 1) % _size == 0) // room1 is in last row last col
                    {
                        doorNum = rand.Next(1); // only two doors availible - North and west
                        if (doorNum == 1)
                            doorNum = 3;
                    }
                    else  // door is somewhere in last row
                    { 
                        doorNum = rand.Next(3);   // only three doors availible - North, East and West
                        if (doorNum == 1)
                            doorNum = 3;
                    }
                }
                else if (room1 % _size == 0)
                { // room1 is in first column
                    doorNum = rand.Next(3);       // only three doors availible - North and South and east
                    if (doorNum == 3)
                        doorNum = 2;
                }
                else if ((room1 + 1) % _size == 0)
                { // room1 is in last column
                    doorNum = rand.Next(2); // only three doors availible - North and South
                }
                else // no restrictions on random adjacent apply
                    doorNum = rand.Next(3);

                // get room2 number based on door number
                if (doorNum == 0)
                {  // room2 is to the north
                    room2 = room1 - _size;
                }
                else if (doorNum == 1)
                {
                    room2 = room1 + _size;   // room2 is to the south
                }
                else if (doorNum == 2)
                {
                    room2 = room1 + 1;  // room2 is to the east
                }
                else
                    room2 = room1 - 1;

                //mark room1 and room2 as visited
                visitedRooms.Add(room1);
                visitedRooms.Add(room2);

                if (set.Find(room1) != set.Find(room2))
                {
                    // open doors connecting room1 and room2
                    if (room1 - room2 == 1)  //room2 is to the left of room1
                    {
                        Rooms[room1].West.OpenDoor();
                        Rooms[room2].East.OpenDoor();
                    }
                    else if (room1 - room2 == -1)
                    {           // room2 is to the right of room1
                        Rooms[room1].East.OpenDoor();
                        Rooms[room2].West.OpenDoor();
                    }
                    else if (room1 - room2 == _size)
                    {   // room2 is to the north of room1
                        Rooms[room1].North.OpenDoor();
                        Rooms[room2].South.OpenDoor();
                    }
                    else
                    {                                   // room2 is to the south of room1
                        Rooms[room1].South.OpenDoor();
                        Rooms[room2].North.OpenDoor();
                    }

                    set.union(set.Find(room1), set.Find(room2));
                }
            }
        }
        
        public MazeModel(int size)
        {
            _size = size;
            InitializeRooms();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A sorted dictionary representing the solution in reverse order (starting at the exit and working backwards towards the entrance)</returns>

        public Cell GetCellAtIndex(int index)
        {
            return Rooms[index];
        }

        private void InitializeRooms()
        {
            Rooms = new Cell[_size * _size];
            for (int i = 0; i < _size * _size; i++)
            {
                Rooms[i] = new Cell();
            }

            // Open the north door for an entrance to the maze
            Rooms[0].North.OpenDoor();;
            // Open the bottom right corner south door as an exit for the maze
            Rooms[MaxIndex].South.OpenDoor();;
        }
    }
}
