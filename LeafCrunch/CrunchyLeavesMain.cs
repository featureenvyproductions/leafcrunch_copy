using LeafCrunch.GameObjects;
using LeafCrunch.Menus;
using LeafCrunch.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LeafCrunch
{
    //still need a main timer
    //maybe a help screen
    //and transitions between levels
    //and barriers
    //sounds also
    //do we want some kind of mystery box thing? 
    public partial class CrunchyLeavesMain : Form
    {
        private Room Room { get; set; }

        private InterruptController InterruptController { get; set; }

        public CrunchyLeavesMain()
        {
            InitializeComponent();
            Room = new Room(pbLevel1, pbPlayer, lblRainbowPoints, lblCountDown, new List<Control>() {
                pbGreenLeaf01,
                pbYellowLeaf01,
                pbOrangeLeaf01,
                pbRedLeaf01,
                pbPineCone01
            });

            InterruptController = new InterruptController(new List<Control>() { pnHelpMenu });
            
            timer1.Start();
        }

        private void CrunchyLeavesMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (InterruptController.OnKeyDown(e))
            {
                case ControllerState.SUSPEND:
                    Room.Suspend();
                    break;
                case ControllerState.RESUME:
                    Room.Resume();
                    break;
                default:
                    Room.OnKeyPress(e);
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (InterruptController.Update())
            {
                case ControllerState.SUSPEND:
                    break;
                default:
                    Room.Update();
                    break;
            }
        }

        private void CrunchyLeavesMain_KeyUp(object sender, KeyEventArgs e)
        {
            switch (InterruptController.OnKeyUp(e))
            {
                case ControllerState.SUSPEND:
                    break;
                default:
                    Room.OnKeyUp(e);
                    break;
            }
        }
    }
}
