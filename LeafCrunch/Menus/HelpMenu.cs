using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeafCrunch.Menus
{
    //an object on the menu with its own update things
    public class MenuItem
    {
        public void Update() { }
    }

    //mostly for menus but could be for pausing I guess
    public abstract class GenericInterrupt
    {
        public bool IsActive { get; set; }
        public Keys ActivationKey { get; set; }
        public Control Control { get; set; } //parent control for menu items or message popup...like you show and hide this
    }

    public class GenericMenu : GenericInterrupt
    {
        public List<MenuItem> Items { get; set; }
        virtual public void Update() { } //to fill in...probably loop through items and call their update functions
    }

    public class HelpMenu : GenericMenu
    {
        public HelpMenu() : base()
        {
            ActivationKey = Keys.H;
        }

    }
}
