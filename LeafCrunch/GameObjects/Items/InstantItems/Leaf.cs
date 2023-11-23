using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.GameObjects.Items.ItemOperations;
using LeafCrunch.Utilities;
using LeafCrunch.Utilities.Entities;
using System.Drawing;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items.InstantItems
{
    public class Leaf : InstantItem, IDrawable
    {
        protected int _pointIncrement = 10;
        
        virtual public int PointIncrement
        {
            get { return _pointIncrement; }
            set { _pointIncrement = value; }
        }

        private Image _image;
        override public Image CurrentImage { get { return _image; } }

        //create from item data loaded from file
        public Leaf (ItemData itemData) :base()
        {
            if (!OperationMethodRegistry.TargetOperations.ContainsKey("Items.InstantItems.Leaf.Apply"))
                OperationMethodRegistry.TargetOperations.Add("Items.InstantItems.Leaf.Apply", Apply);

            ActivationKey = Keys.Enter;

            //this could probably go in the generic item code but whatever
            _image = UtilityMethods.ImageFromPath(itemData.SingleImage);
            X = itemData.X;
            Y = itemData.Y;
            W = _image.Width;
            H = _image.Height;
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
