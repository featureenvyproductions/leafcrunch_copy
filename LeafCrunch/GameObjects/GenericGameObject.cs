using System.Windows.Forms;

namespace LeafCrunch.GameObjects
{
    //the base thing all the useful things inherit from
    public abstract class GenericGameObject
    {
        public Control Control { get; set; } //the associated forms control

        public GenericGameObject Parent { get; set; }

        public GenericGameObject(Control control)
        {
            Control = control;
        }

        public GenericGameObject(Control control, GenericGameObject parent)
        {
            Control = control;
            Parent = parent;
        }

        public virtual void Initialize() { }
        public virtual void Update() { }
        public virtual void OnKeyPress(KeyEventArgs e) { }
        public virtual void OnKeyUp(KeyEventArgs e) { }
    }
}
