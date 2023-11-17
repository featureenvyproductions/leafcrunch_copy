using LeafCrunch.GameObjects;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LeafCrunch
{
    public partial class CrunchyLeavesMain : Form
    {
        private List<GenericGameObject> Objects { get; set; }
        public CrunchyLeavesMain()
        {
            InitializeComponent();
            var Player = new Player(pbPlayer);
            Objects = new List<GenericGameObject>()
            {
                Player,
                new Room(pbLevel1), //eventually make it so it comes with leaves in a list from ext configuration maybe.
                //random leaves
            };
            timer1.Start();
        }

        private void CrunchyLeavesMain_KeyDown(object sender, KeyEventArgs e)
        {
            //go through list of objects and fire off keypress events
            //you know I also wonder if we want to just store the key in general and not just for the player
            foreach (var obj in Objects)
            {
                obj.OnKeyPress(e);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var obj in Objects)
            {
                obj.Update();
            }
        }

        private void CrunchyLeavesMain_KeyUp(object sender, KeyEventArgs e)
        {
            //go through list of objects and fire off key up events
            foreach (var obj in Objects)
            {
                obj.OnKeyUp(e);
            }
        }
    }
}
