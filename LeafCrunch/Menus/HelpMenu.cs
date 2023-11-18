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

        virtual public void Deactivate() { }
        virtual public void Activate() { }
        virtual public void OnKeyPress(KeyEventArgs e) { }
        virtual public void OnKeyUp(KeyEventArgs e) { }
        virtual public void Update() { } //to fill in...probably loop through items and call their update functions
        virtual public void Hide() { }
        virtual public void Show() { }
    }

    public class GenericMenu : GenericInterrupt
    {
        public List<MenuItem> Items { get; set; }

        override public void Deactivate()
        {
            IsActive = false;
            Hide();
        }

        public override void Hide()
        {
            Control.Visible = false;
            Control.SendToBack();
        }

        public override void Activate()
        {
            IsActive = true;
            Show();
        }

        public override void Show()
        {
            Control.Visible = true;
            Control.BringToFront();
        }

        public override void Update()
        {
            if (!IsActive) return;
        }
    }

    //this isn't really interactive
    //it just displays some helpful tips on how to play the game
    public class HelpMenu : GenericMenu
    {
        public HelpMenu() : base()
        {
            ActivationKey = Keys.H;
        }

    }
}
