using LeafCrunch.GameObjects.Items.ItemOperations;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items
{
    //stuff the player can use
    //i think there will be instant, temporary and permanent items (maybe not permanent right now - equippable...that can be for another time)
    //don't know if i need different classes for them yet
    //trying to also decide if we want everything to operate on the player
    //or to be able to operate on other things like rooms or leaves or an eventual game object
    //better make it generic I guess
    //also there can be multiple targets
    //and we'll have a class that defines the operation and says what's getting operated on and how
    //maybe we have a handy callback in there somewhere
    //instant - executes the operation on the target as soon as the player interacts with it and the effect is permanent
    //temporary - executes the operation on the target immediately but the effect changes with each tick (or it just lasts for a certain number of ticks)
    public class GenericItem : InteractiveGameObject
    {
        private Keys _activationKey = Keys.None; //if we keep this as none, it means just being on the same tile will activate it

        public Keys ActivationKey
        {
            get { return _activationKey; }
            set { _activationKey = value; }
        }

        public GenericItem(Control control) : base(control)
        {
            Active = false;
            // Operation = null;
            MarkedForDeletion = false;
        }

        public GenericItem(Control control, Operation operation)
            : base(control)
        {
            Active = false;
            Operation = operation;
            MarkedForDeletion = false;
        }

        public bool Active { get; set; }
        public bool MarkedForDeletion { get; set; }

        public Operation Operation { get; set; }

        new virtual public void Update()
        {
            if (!Active || Operation == null) return;

            //tbr....maybe we can do something with the result but I think for now we'll just have this generic handle thing
            HandleResult(Operation.Execute());
        }

        virtual public void HandleResult(Result result)
        {

        }

        public void Cleanup()
        {
            var parent = Control.Parent;
            parent.Controls.Remove(Control); //this is dumb I should figure out a better way to do this
        }
    }
}
