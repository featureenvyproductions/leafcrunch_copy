using System.Windows.Forms;

namespace LeafCrunch.Menus
{
    //mostly for menus but could be for pausing I guess
    //make sure we don't have an issue with accidentally bringing up a menu when we've already got one up
    public abstract class GenericInterrupt
    {
        public bool IsActive { get; set; }
        public Keys ActivationKey { get; set; }
        public Control Control { get; set; } //parent control for menu items or message popup...like you show and hide this

        virtual public void Deactivate() { }
        virtual public void Activate() { }
        virtual public void OnKeyPress(KeyEventArgs e) { }
        virtual public void OnKeyUp(KeyEventArgs e) { }
        virtual public void Update() { } //to fill in...probably loop through items and call their update functions
        virtual public void Hide() { }
        virtual public void Show() { }
    }

    //exists to stop stuff from happening but nothing else
    public class Pause: GenericInterrupt
    {
        public Pause() : base() 
        {
            ActivationKey = Keys.Space;
        }

        override public void Deactivate()
        {
            IsActive = false;
        }

        override public void Activate()
        {
            IsActive = true;
        }
    }
}
