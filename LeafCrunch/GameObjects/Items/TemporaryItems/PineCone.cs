using LeafCrunch.GameObjects.Items.InstantItems;
using LeafCrunch.GameObjects.Items.ItemOperations;
using LeafCrunch.Utilities;
using LeafCrunch.Utilities.Entities;
using System.Drawing;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items.TemporaryItems
{
    //basically exists to apply a multiplier to point increases
    //effectively multiplies PointIncrement of all the leaf targets
    public class PineCone : TemporaryItem
    {
        private int _multiplier = 2;
        private bool _displayingAsStat = false;

        public PineCone(ItemData itemData) : base()
        {
            IsApplied = false;
            Active = false;

            if (!OperationMethodRegistry.TargetOperations.ContainsKey("Items.TemporaryItems.PineCode.ApplyPointMultiplier"))
                OperationMethodRegistry.TargetOperations.Add("Items.TemporaryItems.PineCode.ApplyPointMultiplier", ApplyPointMultiplier);

            CountdownDisplayX = itemData.DisplayControl.X;
            CountdownDisplayY = itemData.DisplayControl.Y;
            CountdownDisplayWidth = 150;
            CountdownDisplayHeight = 50;
            
            CurrentImage = UtilityMethods.ImageFromPath(itemData.SingleImage);
            
            X = itemData.X;
            Y = itemData.Y;
            W = CurrentImage.Width;
            H = CurrentImage.Height;

            _multiplier = itemData.PointMultiplier;

            InitializeMultiOperationFromRegistry(itemData.Operation);

            //only consider it initialized if we came this way for now
            IsInitialized = true;
            
        }

        //I feel like this doesn't belong here but eh we'll come back to it
        public override void ShowAsStat()
        {
            if (!_displayingAsStat)
            {
                _displayingAsStat = true;
            }
        }

        public bool DisplayingAsStat
        {
            get { return _displayingAsStat; }
        }
        public int CountdownDisplayX
        {
            get; set;
        }
        public int CountdownDisplayY
        { get; set; }
        public int CountdownDisplayWidth
        { get; set; }
        public int CountdownDisplayHeight
        { get; set; }
        public string CountdownDisplayText
        {
            get
            {
                if (Ticks <= 0) return string.Empty;
                return Ticks.ToString();
            }
        }

        public override void Update()
        {
            if (IsSuspended) return;
            base.Update();
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
