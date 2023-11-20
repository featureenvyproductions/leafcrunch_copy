using LeafCrunch.GameObjects.Items.ItemOperations;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items.InstantItems
{
    public class GreenLeaf : Leaf
    {
        new protected int _pointIncrement = 1;

        override public int PointIncrement
        {
            get { return _pointIncrement; }
            set { _pointIncrement = value; }
        }

        //god it's annoying that have to have this constructor everywhere why did i do this
        public GreenLeaf(Control control) : base(control)
        {

        }

        public GreenLeaf(Control control, Operation operation)
            : base(control, operation)
        {
        }
    }
}
