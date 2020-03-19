namespace Maze.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Door
    {
        private DoorStatus _status;

        /// <summary>
        /// Creates a new instance of a door. Door is set to closed by default.
        /// </summary>
        public Door(DoorStatus status = DoorStatus.CLOSED)
        {
            _status = status;
        }

        public bool IsOpen => _status == DoorStatus.OPEN;
        public bool IsClosed => _status == DoorStatus.CLOSED;

        public void OpenDoor()
        {
            _status = DoorStatus.OPEN;
        }
    }
}
