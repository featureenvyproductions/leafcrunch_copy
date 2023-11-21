using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.GameObjects.Items.ItemOperations;
using LeafCrunch.Utilities;
using LeafCrunch.Utilities.Entities;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items.InstantItems
{
    //you know once we do the dynamic loading for items, we probaby won't have to have different classes anymore
    //maybe?
    //just if we use this we should make sure the point multiplier still works....
    public class Leaf : InstantItem
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

        //tbr....we actually will load the operation data as part of the control configs
        //and pass an operation data structure to this when initializing
        //but for now we'll parse this here to start.

        //oh we could also use the registry to get the room instead of passing a parent control but idk
        public Leaf(Control control, string operationName) : base(control)
        {
            if (!OperationMethodRegistry.TargetOperations.ContainsKey("Items.InstantItems.Leaf.Apply"))
                OperationMethodRegistry.TargetOperations.Add("Items.InstantItems.Leaf.Apply", Apply);

            ActivationKey = Keys.Enter;

            InitializeOperationFromRegistry(operationName);
        }

        //create from item data loaded from file
        public Leaf (ItemData itemData) :base()
        {
            if (!OperationMethodRegistry.TargetOperations.ContainsKey("Items.InstantItems.Leaf.Apply"))
                OperationMethodRegistry.TargetOperations.Add("Items.InstantItems.Leaf.Apply", Apply);

            ActivationKey = Keys.Enter;

            var img = UtilityMethods.ImageFromPath(itemData.SingleImage);
            Control = new PictureBox()
            {
                Left = itemData.X,
                Top = itemData.Y,
                Image = img,
                Width = img.Width,
                Height = img.Height
            };
            _pointIncrement = itemData.PointIncrement;
            InitializeOperationFromRegistry(itemData.Operation);

            //only consider it initialized if we came this way for now
            IsInitialized = true;
        }

        virtual protected Result Apply(GenericGameObject genericGameObject, object paramList)
        {
            var victim = genericGameObject as IItemUser;
            if (victim != null) victim.ApplyItem(paramList);
            return new Result() { Value = true };
        }
    }
}
