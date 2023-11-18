using LeafCrunch.GameObjects.Items.ItemOperations;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items
{
    public class InstantItem : GenericItem
    {
        public InstantItem(Control control) : base(control)
        {
        }

        public InstantItem(Control control, Operation operation)
            : base(control, operation)
        {
        }

        public override void HandleResult(Result result)
        {
            //regardless of the outcome, the item is used so we delete it
            MarkedForDeletion = true;
            Active = false;
        }

        public override void Update()
        {
            //oh when will we set it to active though
            if (!Active || Operation == null) return;

            HandleResult(Operation.Execute());
        }
    }
}
