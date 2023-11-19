using LeafCrunch.GameObjects.Items.ItemOperations;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items.InstantItems
{
    //ok so the way this works is that the rainbow bar reflects the player's rainbow points
    //and the leaf operates on the player to produce that result
    //when the operation is complete the leaf can be removed from the list of room items
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
            Operation.ToExecute = PointChange;
            Operation.Params = null;
        }

        //what we'll ultimately use as the operation
        protected Result PointChange(GenericGameObject genericGameObject, object paramList)
        {
            var player = genericGameObject as Player;
            if (player == null) return new Result() { Value = false }; //who knows what happened...we should only be operating on the player.

            player.RainbowPoints += PointIncrement;
            return new Result() { Value = true };
        }
    }
}
