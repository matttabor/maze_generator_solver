namespace Maze.Models
{
    /// <summary>
    /// A cell is essentially a room made up of 4 doors (one to the north, south, east and west).
    /// If a door is open then you can walk through that direction.
    /// </summary>
    public class Cell
    {
        public Cell()
        {
            North = new Door();
            South = new Door();
            East = new Door();
            West = new Door();
        }
        
        public Door North { get; set;}
        public Door South { get; set;}
        public Door East { get; set;}
        public Door West { get; set;}
    }
}
