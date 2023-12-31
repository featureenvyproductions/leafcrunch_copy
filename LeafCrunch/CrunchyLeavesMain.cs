﻿using LeafCrunch.GameObjects;
using LeafCrunch.GameObjects.Stats;
using LeafCrunch.Menus;
using LeafCrunch.Utilities;
using LeafCrunch.Utilities.Animation;
using LeafCrunch.Utilities.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static LeafCrunch.Utilities.GlobalVars;

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

        //note: when we refactor the interrupt controller
        //we'll also need to fix the draw code.
        //also because I need a way to change the text bc it'll need to be slightly different for stuff like
        //the nightmare levels
        //especially when the nightmares show up in the real world. 
        //also rather than wiping out the whole room control I can probably just make a thing called "animated transition"
        //and it's a sequence of images instead of just one
        private InterruptController InterruptController { get; set; }

        private int roomIndex = -1;
        private List<string> OrderedRooms = new List<string>()
        {
            "intro",
            "intro2",
            "intro3",
            "intro4",
            "intro5",
            "instructions",
            "level1",
            //note to self: we need to have a bunch of EXCRUCIATINGLY normal levels
            //then the character starts to age
            //then we have a nightmare level with nothing
            //then as they get older we have more nightmare levels
            //but as long as you can get to the leaf you'll be ok
            //then there is a series of only nightmare levels
            //and then the nightmares show up in the real world
            //and then everything goes black
            //and you have to try to find the leaf in the darkness and step on it
            //I think the help text will remain ominously the same the first time you encounter hazards
            //and as you encounter more there will be more tips, but the help menu will be visibly worn down
            //the last level will be after you find the leaf
            //you're a guy now and there's leaves everywhere
            //maybe there's nightmares too but either they'll be on the outside of where you are bouncing around
            //or the leaves will form a boundary or something
            //idk what the right thing is
            "transition1",
            "level2",
            "outro"
        };

        private void InitializeRoom(bool reload)
        {
            if (!reload)
            {
                roomIndex++;
                if (roomIndex >= OrderedRooms.Count)
                {
                    //you win the game....we'll display a final screen here but I'll figure that part out later
                    Application.Exit();
                    Initialized = false;
                    return;
                }
            }
            if (RoomController?.Control != null)
            {
                //remove the control no matter what.
                this.Controls.Remove(RoomController.Control);
            }
            RoomController = new RoomController(this, OrderedRooms[roomIndex]);
            //note to self: I think for level transitions we'll just have a "room" sort of thing but there's nothing in it
            //or maybe in the json I can have a special transition type
            //and this can inherit from the same thing as roomcontroller maybe
            //only the win condition will just be Ticks = however long I want to display it for.
        }

        private bool Initialized = false;
        public CrunchyLeavesMain()
        {
            Initialized = false;
            InitializeComponent();

            GlobalVars.CalculateFrameRate(timer1.Interval);

            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoSize = true;

            InitializeRoom(false); //I think this'll work to start

            //oh shit i forgot to dynamically initialize activation keys we'll come back to that
            //and we'll add some code to make sure there's only one thing that can happen in this game per key
            //like instead of just SETTING an activation key explicitly, we'll call "request activation key"
            //from the utilities 
            //and it'll query a list of used keys to make sure it's not in there or whatever
            //if relevant anyway
            //we can just set the key without requesting if we don't care
            //like items can all have the same one
            //but menus shouldn't
            //oh ok maybe we'll just do that for menus.
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
            Initialized = true;
        }

        private void CrunchyLeavesMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Initialized) return;
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

        private void HandleWinCondition(WinCondition win)
        {
            switch (win)
            {
                case WinCondition.Lose:
                    //reinitialize the current room
                    InitializeRoom(true);
                    break;
                case WinCondition.Win:
                    InitializeRoom(false); //proceed to the next room
                    break;
                default:
                    break;//keep trucking
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Initialized) return;
            //is controller active
            switch (InterruptController.Update())
            {
                case ControllerState.ACTIVE: //it is
                    break;
                default:
                    RoomController.Update();
                    HandleWinCondition(RoomController.WinCondition);
                    break;
            }
        }

        private void CrunchyLeavesMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Initialized) return;
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
