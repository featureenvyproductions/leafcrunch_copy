using LeafCrunch.GameObjects;
using LeafCrunch.Menus;
using LeafCrunch.Utilities;
using LeafCrunch.Utilities.Animation;
using LeafCrunch.Utilities.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private RoomController RoomController { get; set; }

        private InterruptController InterruptController { get; set; }

        public CrunchyLeavesMain()
        {
            InitializeComponent();

            GlobalVars.CalculateFrameRate(timer1.Interval);

            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoSize = true;

            RoomController = new RoomController(this, "test");

            InterruptController = new InterruptController(this);

            //how are we going to transition levels though
            //thoughts...
            //let's just start with something really basic
            //in the player object, we'll define a win conditions object
            //this will have a list of properties and the value they need to be
            //(I feel like this is pretty flexible...it can be used for points, or we can even
            //have the player like ask the current room about some items
            //because you can reference anything that matters by looking it up in the generic object registry
            //including the room - haven't added the room to the object registry but I will)
            //when we initialize a room there are a handful of things that apply to the player
            //that we'll initialize from all those Rooms configs because they change with the room
            //that'll be starting position and win conditions.
            //it'll also define the name of the next room, and we can set this globally
            //I realize we can probably also like. initialize this for the room and not the player....
            //we'll see what's easier
            
            //every tick, after we call the update stuff, if we're not on an interrupt
            //we call a "Check win conditions" method of some sort.
            //ohhhhhhh ok. so like wait
            //it'll have one of 3 values.
            //this'll be a member of RoomController
            //it'll be null as long as we change nothing and we're in the same room
            //if it returns the name of the room we're on (which I'll store in globals)
            //we died and need to restart
            //if it returns the name of a different room, that's the next room.
            //load the next room and wipe this one out. 
            //doesn't do anything to preserve persistence between levels but eh we'll figure that out later
            
            timer1.Start();
        }

        private void CrunchyLeavesMain_KeyDown(object sender, KeyEventArgs e)
        {
            //determine whether the interrupt controller is active
            switch (InterruptController.OnKeyDown(e))
            {
                case ControllerState.SUSPEND: //it isn't
                    RoomController.Resume();
                    break;
                case ControllerState.ACTIVE: //it is
                    RoomController.Suspend();
                    break;
                default:
                    RoomController.OnKeyPress(e);
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
                    RoomController.Update();
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
                    RoomController.OnKeyUp(e);
                    break;
            }
        }
    }
}
