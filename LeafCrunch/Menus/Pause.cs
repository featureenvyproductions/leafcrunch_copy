using System.Windows.Forms;

namespace LeafCrunch.Menus
{
    //exists to stop stuff from happening but nothing else
    public class Pause : GenericInterrupt
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
