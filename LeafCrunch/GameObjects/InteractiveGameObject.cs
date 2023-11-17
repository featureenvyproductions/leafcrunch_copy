using LeafCrunch.Utilities;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects
{
    public class InteractiveGameObject : GenericGameObject
    {
        public Speed Speed { get; set; }

        public InteractiveGameObject(Control control) : base(control)
        {
            Speed = new Speed() { vx = 0, vy = 0 };
        }
    }
}
