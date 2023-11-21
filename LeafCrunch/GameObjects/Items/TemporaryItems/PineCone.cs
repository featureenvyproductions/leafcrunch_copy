using LeafCrunch.GameObjects.Items.InstantItems;
using LeafCrunch.GameObjects.Items.ItemOperations;
using LeafCrunch.Utilities;
using LeafCrunch.Utilities.Entities;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items.TemporaryItems
{
    //basically exists to apply a multiplier to point increases
    //effectively multiplies PointIncrement of all the leaf targets
    public class PineCone : TemporaryItem
    {
        private int _multiplier = 2;
        private bool _displayingAsStat = false;
        
        //eventually we should actually have one object that handles displayed stats but for now we
        //can just give this its own control to display them it's fine it's whatever
        public Control DisplayControl { get; set; }

        public PineCone(Control control) : base(control)
        {
        }

        //this can be an operation to be done on a single leaf
        //or can be a multi target operation that's fed multiple leaves
        public PineCone(Control control, Operation operation, Control displayControl) : base(control, operation)
        {
            Operation.Params = null;
            DisplayControl = displayControl;
            Operation.ToExecute = ApplyPointMultiplier;
        }

        public PineCone(Control control, string operationName, Control displayControl) : base(control)
        {
            DisplayControl = displayControl;
            if (!OperationMethodRegistry.TargetOperations.ContainsKey("Items.TemporaryItems.PineCode.ApplyPointMultiplier"))
                OperationMethodRegistry.TargetOperations.Add("Items.TemporaryItems.PineCode.ApplyPointMultiplier", ApplyPointMultiplier);

            InitializeMultiOperationFromRegistry(operationName);
        }

        public PineCone(ItemData itemData) : base()
        {
            if (!OperationMethodRegistry.TargetOperations.ContainsKey("Items.TemporaryItems.PineCode.ApplyPointMultiplier"))
                OperationMethodRegistry.TargetOperations.Add("Items.TemporaryItems.PineCode.ApplyPointMultiplier", ApplyPointMultiplier);

            DisplayControl = new Label()
            {
                Left = itemData.DisplayControl.X,
                Top = itemData.DisplayControl.Y,
                BackColor = System.Drawing.Color.Transparent
                //will it work without an initial width and height?
            };

            var img = UtilityMethods.ImageFromPath(itemData.SingleImage);
            Control = new PictureBox()
            {
                Left = itemData.X,
                Top = itemData.Y,
                Image = img,
                Width = img.Width,
                Height = img.Height
            };
            _multiplier = itemData.PointMultiplier;

            InitializeMultiOperationFromRegistry(itemData.Operation);

            //only consider it initialized if we came this way for now
            IsInitialized = true;
            
        }

        //I feel like this doesn't belong here but eh we'll come back to it
        public override void ShowAsStat()
        {
            if (DisplayControl == null) return;
            if (!_displayingAsStat)
            {
                //align the top
                Control.Top = DisplayControl.Top;
                //line up the right side of one with the left of the other
                Control.Left = DisplayControl.Left - Control.Width;
                _displayingAsStat = true;
            }
        }

        public override void Update()
        {
            if (IsSuspended) return;
            base.Update(); //do what the base does
            //but we also want to update the count down
            if (DisplayControl == null) return;
            DisplayControl.Text = Ticks.ToString();
            if (Ticks <= 0) DisplayControl.Visible = false;
            DisplayControl.Refresh();
        }

        private Result ApplyPointMultiplier(GenericGameObject genericGameObject, object paramList)
        {
            //don't apply if we already applied it
            if (IsApplied)
            {
                //but do check the ticks to see if it's time to unapply
                if (Ticks <= 1) //we're going to hit 0 when we handle the result
                {
                    //ok we need to unapply the multiplier
                    var target = genericGameObject as Leaf;
                    if (target != null)
                    {
                        target.PointIncrement /= _multiplier;
                        target.Refresh();
                    }
                }
            }
            else
            {
                //ok we can apply it
                var target = genericGameObject as Leaf;
                if (target != null)
                {
                    target.PointIncrement *= _multiplier;
                    target.Refresh();
                }
            }
            return new Result //we don't do anything with the result here right now. 
            {
                Value = null
            };
        }
    }
}
