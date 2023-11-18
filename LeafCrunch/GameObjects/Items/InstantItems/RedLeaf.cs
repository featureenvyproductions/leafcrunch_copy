using LeafCrunch.GameObjects.Items.ItemOperations;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items.InstantItems
{
    public class RedLeaf : Leaf
    {
        new protected int _pointIncrement = 15;

        override public int PointIncrement
        {
            get { return _pointIncrement; }
            set { _pointIncrement = value; }
        }

        //god it's annoying that have to have this constructor everywhere why did i do this
        public RedLeaf(Control control) : base(control)
        {
        }

        public RedLeaf(Control control, Operation operation)
            : base(control, operation)
        {
        }
    }
}
