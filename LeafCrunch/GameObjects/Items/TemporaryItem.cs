using LeafCrunch.GameObjects.Items.ItemOperations;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items
{
    //this would be like when i have cooler stuff like pine cones and they give you a point multiplier or something
    //for a limited time
    public class TemporaryItem : GenericItem
    {
        private int _ticks = 100;
        virtual public int Ticks
        {
            get { return _ticks; }
            set { _ticks = value; }
        }

        private bool _isApplied = false; //true once we've applied the item/turned on its effect so we don't do it additively
        virtual public bool IsApplied
        {
            get { return _isApplied; }
            set { _isApplied = value; }
        }

        public TemporaryItem(Control control) : base(control)
        {
        }

        public TemporaryItem(Control control, Operation operation) : base(control, operation)
        {
        }

        protected override void HandleResult(Result result)
        {
            Ticks--;
            //we don't mark for deletion until we've gone through the ticks.
            if (Ticks == 0)
            {
                Active = false;
                MarkedForDeletion = true;
            }
        }

        public override void Update()
        {
            //oh when will we set it to active though
            if (IsSuspended || !Active || Ticks <= 0 || Operation == null) return;

            //first let's see if this is a multi target operation and handle accordingly
            var multitarget = Operation as MultiTargetOperation;
            if (multitarget != null) HandleResult(multitarget.Execute());
            else HandleResult(Operation.Execute());

            IsApplied = true; //one we set this we leave it alone
        }

        virtual public void ShowAsStat() { }
    }
}
