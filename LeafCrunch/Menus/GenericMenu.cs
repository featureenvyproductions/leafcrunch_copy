using System.Collections.Generic;

namespace LeafCrunch.Menus
{
    //an object on the menu with its own update things
    public class MenuItem
    {
        public void Update() { }
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
}
