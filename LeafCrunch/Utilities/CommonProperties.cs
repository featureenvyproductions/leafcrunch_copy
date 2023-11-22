namespace LeafCrunch.Utilities
{
    public class GlobalVars
    {
        public static int RoomWidth { get; set; }
        public static int RoomHeight { get; set; }

        public static int RoomTileSizeW { get; set; }
        public static int RoomTileSizeH { get; set; }

        //could also hypothetically use this to schedule events besides animation updates
        //how many ticks we want to pass before updating an animation
        public static int FrameTickCount { get; set; }
        //how many frames per second we want the animation to be
        public static int TargetFrameRate { get; set; }

        public static void CalculateFrameRate(int tickInterval)
        {
            TargetFrameRate = 10;
            var ticksPerSecond = 1000 / tickInterval;
            FrameTickCount = ticksPerSecond / TargetFrameRate;
        }
    }

    public enum Direction
    {
        None,
        North,
        South,
        West,
        East
    }

    public enum Axis
    {
        Horizontal,
        Vertical
    }

    public class Speed
    {
        public int vx = 0;
        public int vy = 0;
    }

    public enum ControllerState
    {
        ACTIVE = 0,
        SUSPEND = 1,
        UNCHANGED = 2
    }
}
