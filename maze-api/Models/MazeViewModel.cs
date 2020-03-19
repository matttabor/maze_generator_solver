using System;
using System.Collections.Generic;
using Maze.Models;

namespace MazeApi.Models
{
    public class MazeViewModel
    {
        public int Size { get; set; }
        public int NumOfRooms => Size * Size;
        public int MaxIndex => NumOfRooms - 1;

        public CellViewModel[] Rooms { get; set; }
    }

    public class CellViewModel
    {
        public DoorViewModel North { get; set;}
        public DoorViewModel South { get; set;}
        public DoorViewModel East { get; set;}
        public DoorViewModel West { get; set;}
    }

    public class DoorViewModel
    {
        public DoorStatus Status { get; set; }

        public DoorViewModel()
        {
        }

        public bool IsOpen => Status == DoorStatus.OPEN;
        public bool IsClosed => Status == DoorStatus.CLOSED;
    }
}