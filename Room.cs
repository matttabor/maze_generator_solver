namespace maze_generator
{
    public class Room
    {
        public Room()
        {
            Visited = false;
            Door0 = Door1 = Door2 = Door3 = true; // all closed to start
        }

        public bool Visited { get; set; }
        // North
        public bool Door0 { get; set;}
        // South
        public bool Door1 { get; set;}
        // East
        public bool Door2 { get; set;}
        // West
        public bool Door3 { get; set;}
    }
}
