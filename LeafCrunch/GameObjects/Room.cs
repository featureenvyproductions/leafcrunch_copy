using LeafCrunch.Utilities;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects
{
    public class Room : GenericGameObject
    {
        public Room(Control control) : base(control)
        {
            GlobalVars.RoomWidth = control.Width;
            GlobalVars.RoomHeight = control.Height;
        }

        public override void Update()
        {
        }
    }
}
