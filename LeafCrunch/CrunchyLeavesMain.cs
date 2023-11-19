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
            //determine whether the interrupt controller is active
            switch (InterruptController.OnKeyDown(e))
            {
                case ControllerState.SUSPEND: //it isn't
                    Room.Resume();
                    break;
                case ControllerState.ACTIVE: //it is
                    Room.Suspend();
                    break;
                default:
                    Room.OnKeyPress(e);
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //is controller active
            switch (InterruptController.Update())
            {
                case ControllerState.ACTIVE: //it is
                    break;
                default:
                    Room.Update();
                    break;
            }
        }

        private void CrunchyLeavesMain_KeyUp(object sender, KeyEventArgs e)
        {
            //is controller active
            switch (InterruptController.OnKeyUp(e))
            {
                case ControllerState.ACTIVE: //it is
                    break;
                default:
                    Room.OnKeyUp(e);
                    break;
            }
        }
    }
}
