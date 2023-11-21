using LeafCrunch.GameObjects.Items.ItemOperations;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items.InstantItems
{
    public class OrangeLeaf : Leaf
    {
        new protected int _pointIncrement = 10;

        override public int PointIncrement
        {
            get { return _pointIncrement; }
            set { _pointIncrement = value; }
        }

        public OrangeLeaf(Control control) : base(control)
        {
        }

        public OrangeLeaf(Control control, Operation operation)
            : base(control, operation)
        {
        }

        public OrangeLeaf(Control control, string operationName)
            : base(control, operationName)
        {
        }
    }
}
