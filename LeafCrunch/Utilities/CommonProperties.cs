namespace LeafCrunch.Utilities
{
    public class GlobalVars
    {
        public static int RoomWidth { get; set; }
        public static int RoomHeight { get; set; }

        public static int RoomTileSizeW { get; set; }
        public static int RoomTileSizeH { get; set; }
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
