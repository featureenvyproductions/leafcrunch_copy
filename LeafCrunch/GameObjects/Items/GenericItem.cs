using LeafCrunch.GameObjects.Items.ItemOperations;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items
{
    public class GenericItem : InteractiveGameObject
    {
        private Keys _activationKey = Keys.None; //if we keep this as none, it means just being on the same tile will activate it
        private bool _isSuspended = false; //different from active...this is just for pausing

        public Operation Operation { get; set; }
        public bool Active { get; set; }
        public bool MarkedForDeletion { get; set; }

        public bool IsSuspended
        { 
            get { return _isSuspended; } 
            set { _isSuspended = value; } 
        }

        public Keys ActivationKey
        {
            get { return _activationKey; }
            set { _activationKey = value; }
        }

        public GenericItem(Control control) : base(control)
        {
            //idk man I got myself into constructor hell IDK if we want this to be a case or not
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

        public void Cleanup()
        {
            var parent = Control.Parent;
            parent.Controls.Remove(Control); //this is dumb I should figure out a better way to do this
        }

        new virtual public void Update()
        {
            if (_isSuspended || !Active || Operation == null) return;

            //tbr....maybe we can do something with the result but I think for now we'll just have this generic handle thing
            HandleResult(Operation.Execute());
        }

        virtual protected void HandleResult(Result result) { }
    }
}
