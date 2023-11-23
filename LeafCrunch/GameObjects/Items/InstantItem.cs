using LeafCrunch.GameObjects.Items.ItemOperations;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items
{
    public class InstantItem : GenericItem
    {
        public InstantItem () : base()
        {
            Control = null; //setting this to help me find problems
        }

        protected override void HandleResult(Result result)
        {
            //regardless of the outcome, the item is used so we delete it
            MarkedForDeletion = true;
            Active = false;
        }

        public override void Update()
        {
            //oh when will we set it to active though
            if (IsSuspended || !Active || Operation == null) return;

            HandleResult(Operation.Execute());
        }
    }
}
