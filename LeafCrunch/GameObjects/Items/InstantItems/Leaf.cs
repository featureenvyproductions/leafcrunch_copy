using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.GameObjects.Items.ItemOperations;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items.InstantItems
{
    public abstract class Leaf : InstantItem
    {
        protected int _pointIncrement = 10;

        //hacky but eh
        virtual public int PointIncrement
        {
            get { return _pointIncrement; }
            set { _pointIncrement = value; }
        }

        public Leaf(Control control) : base(control)
        {
        }

        public Leaf(Control control, Operation operation)
            : base(control, operation)
        {
            ActivationKey = Keys.Enter;
            Operation.ToExecute = Apply;
            Operation.Params = new Dictionary<string, object> {
                { "RainbowPoints", PointIncrement }
            };
        }

        virtual protected Result Apply(GenericGameObject genericGameObject, object paramList)
        {
            var victim = genericGameObject as IItemUser;
            if (victim != null) victim.ApplyItem(paramList);
            return new Result() { Value = true };
        }
    }
}
