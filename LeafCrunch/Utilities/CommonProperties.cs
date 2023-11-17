using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafCrunch.Utilities
{
    public class GlobalVars
    {
        public static int RoomWidth { get; set; }
        public static int RoomHeight { get; set; }

        public static int RoomTileSizeW { get; set; }
        public static int RoomTileSizeH { get; set; }
    }

    public class Speed
    {
        public int vx = 0;
        public int vy = 0;
    }
}
