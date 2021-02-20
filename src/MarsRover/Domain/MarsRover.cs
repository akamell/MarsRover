namespace MarsRoverTracking.MarsRover.Domain
{
    public class MarsRover
    {
        public string RoverId { get; set; }
        public string Message { get; set; }
        public int LastIndexDirection { get; set; }
        public int CurrentPositionY { get; set; }
        public int CurrentPositionX { get; set; }
    }
}