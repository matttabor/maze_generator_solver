using System;
using System.Collections.Generic;

namespace maze_generator
{
    public class Maze
    {
        private int _size;
        public int MaxIndex => NumOfRooms - 1;
        public int Size => _size;
        public int NumOfRooms => _size * _size;
        private static Room[] _rooms;

        public void GenerateMaze()
        {
            var set = new DisjointSet(_size * _size);
            int maxIndex = _size * _size - 1;
            int room1, room2, doorNum;
            var rand = new Random();

            // while start and end are not in the same set AND while everyroom has not been visited yet
            while ((set.find(0) != set.find(maxIndex)) && GetNumberOfRoomsVisited() < _size * _size)
            {
                // pick a random room
                room1 = rand.Next(maxIndex);

                // pick a random room adjacent to room1
                // several restrictions apply to what cell can
                // be chosen based upon where room1 is located
                if (room1 < _size) // room1 is in first row
                { 
                    if (room1 % _size == 0)
                    { //room1 is in first row first col
                        doorNum = rand.Next() % 2;   // only two doors availible - south and east
                        if (doorNum == 0)
                            doorNum = 2;
                        else
                            doorNum = 1;
                    }
                    else if ((room1 + 1) % _size == 0)
                    {   // room1 is in first row last col
                        doorNum = rand.Next() % 2;                   // only two doors availible - south and west
                        if (doorNum == 0)
                            doorNum = 1;
                        else
                            doorNum = 3;
                    }
                    else
                    {                           // door is somewhere in row 1
                        doorNum = rand.Next() % 3 + 1; // only three doors availible - south, east and west

                    }
                }
                else if (room1 > maxIndex - _size)
                {   // room1 is in last row
                    if (room1 % _size == 0)
                    {           //room1 is in last row first col
                        doorNum = rand.Next() % 2;           // only two doors availible - north and east
                        if (doorNum == 1)
                            doorNum = 2;
                    }
                    else if ((room1 + 1) % _size == 0)
                    {   // room1 is in last row last col
                        doorNum = rand.Next() % 2;                   // only two doors availible - North and west
                        if (doorNum == 1)
                            doorNum = 3;
                    }
                    else
                    {                       // door is somewhere in last row 
                        doorNum = rand.Next(3);   // only three doors availible - North, East and West
                        if (doorNum == 1)
                            doorNum = 3;
                    }
                }
                else if (room1 % _size == 0)
                { // room1 is in first column
                    doorNum = rand.Next() % 3;       // only three doors availible - North and South and east
                    if (doorNum == 3)
                        doorNum = 2;
                }
                else if ((room1 + 1) % _size == 0)
                { // room1 is in last column
                    doorNum = rand.Next() % 2;               // only three doors availible - North and South
                }
                else // no restrictions on random adjacent apply
                    doorNum = rand.Next() % 4;

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
                _rooms[room1].Visited = true;
                _rooms[room2].Visited = true;

                if (set.find(room1) != set.find(room2))
                {
                    // open doors connecting room1 and room2
                    if (room1 - room2 == 1)  //room2 is to the left of room1
                    {              
                        _rooms[room1].Door3 = false;
                        _rooms[room2].Door2 = false;
                    }
                    else if (room1 - room2 == -1)
                    {           // room2 is to the right of room1
                        _rooms[room1].Door2 = false;
                        _rooms[room2].Door3 = false;
                    }
                    else if (room1 - room2 == _size)
                    {   // room2 is to the north of room1
                        _rooms[room1].Door0 = false;
                        _rooms[room2].Door1 = false;
                    }
                    else
                    {                                   // room2 is to the south of room1
                        _rooms[room1].Door1 = false;
                        _rooms[room2].Door0 = false;
                    }

                    set.union(set.find(room1), set.find(room2));
                }
            }
        }
        public Maze(int size)
        {
            _size = size;
            InitializeRooms();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A sorted dictionary representing the solution in reverse order (starting at the exit and working backwards towards the entrance)</returns>
        
        public Room GetRoomAtIndex(int index)
        {
            return _rooms[index];
        }

        public void MarkAllRoomsAsUnvisited()
        {
            for (int i = 0; i < NumOfRooms; i++)
            {
                _rooms[i].Visited = false;
            }
        }


        private void InitializeRooms()
        {
            _rooms = new Room[_size * _size];
            for (int i = 0; i < _size * _size; i++)
            {
                _rooms[i] = new Room();
            }

            _rooms[0].Door0 = false;
            _rooms[MaxIndex].Door1 = false;
        }

        private int GetNumberOfRoomsVisited()
        {
            var roomsCount = 0;
            for (int i = 0; i < _size * _size; i++)
            {
                roomsCount += _rooms[i].Visited ? 1 : 0;
            }

            return roomsCount;
        }
    }
}