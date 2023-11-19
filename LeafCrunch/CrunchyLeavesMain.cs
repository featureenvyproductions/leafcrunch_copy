using LeafCrunch.GameObjects;
using LeafCrunch.Menus;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using LeafCrunch.Utilities;
using LeafCrunch.GameObjects.Items;
using LeafCrunch.GameObjects.Items.InstantItems;
using LeafCrunch.GameObjects.Items.ItemOperations;
using LeafCrunch.GameObjects.Items.TemporaryItems;

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

        private InterruptContainer InterruptHandler { get; set; }

        //handles pauses and menus and other stuff that's not interactive or interactive but in a different way than the gameplay
        public class InterruptContainer
        {
            private bool interruptActive = false;
            public bool InterruptActive
            {
                get { return interruptActive; }
                set { interruptActive = value; }
            }

            //like the room we'll eventually have a real loading system and not this hacked together ordered control list
            public InterruptContainer(List<Control> menuControls)
            {
                Load(menuControls);

                //before we start, hide all the menus
                foreach (var interrupt in Interrupts)
                {
                    interrupt.Hide();
                }
            }

            protected void Load(List<Control> menuControls)
            {
                Interrupts = new List<GenericInterrupt>()
                {
                    new HelpMenu()
                    {
                        IsActive = false,
                        Control = menuControls.ElementAt(0)
                    },
                    new Pause()
                    {
                        IsActive = false
                    }
                };
            }

            private List<GenericInterrupt> Interrupts { get; set; }

            public RoomState OnKeyDown(KeyEventArgs e)
            {
                //the first thing we do is check to see if we're leaving a menu
                //this returns true as long as the menu is still active
                if (interruptActive)
                {
                    //are we leaving a specific menu?
                    var activeInterrupt = Interrupts.Where(i => i != null && i.IsActive && i.ActivationKey == e.KeyCode).FirstOrDefault();
                    if (activeInterrupt != null)
                    {
                        //just leave this menu
                        activeInterrupt.Deactivate();
                        //was that the last one?
                        var remaining = Interrupts.Where(i => i != null && i.IsActive);
                        if (remaining == null || remaining.Count() == 0)
                        {
                            interruptActive = false;
                            //yes so
                            //pass event handling back to the room
                            //i mean really what we need to do is have a state machine with a Room state and a Menu state
                            //Room.Resume();
                            return RoomState.RESUME;
                        }
                        //no, don't pass event handling back to the room
                        //return;
                     //   return RoomState.NOTHING;
                    }
                    //brute force close of all menus
                    else if (e.KeyCode == Keys.Escape)
                    {
                        var activeInterrupts = Interrupts.Where(i => i != null && i.IsActive);
                        foreach (var i in activeInterrupts)
                        {
                            i.Deactivate();
                        }
                        interruptActive = false;
                        //Room.Resume();
                        //return;
                        return RoomState.RESUME;
                    }
                }

                //if we're not then see if we're entering one
                var relevantInterrupts = Interrupts.Where(i => i != null && i.ActivationKey == e.KeyCode);

                //don't waste time looping through objects if we're in a menu or paused
                //note: we'll have to do more here later on as we flesh out the menus of course
                //in case menus have key shortcuts
                //eventually we'll have to add something that checks on load to make sure menus arent configured with the same shortcuts
                //or reserved keys like left/right/up/down/enter
                if (relevantInterrupts.Any())
                {
                    interruptActive = true;
                    //before we do anything else, we need to suspend the room so that the menus take over event handling
                    //Room.Suspend();
                    foreach (var i in relevantInterrupts)
                    {
                        i.Activate();
                        //call their update functions
                        i.OnKeyPress(e);
                    }
                    //return;
                    return RoomState.SUSPEND;
                }
                return RoomState.NOTHING;
            }

            public RoomState OnKeyUp(KeyEventArgs e)
            {
                //see if we have any menus active
                if (interruptActive)
                {
                    //fire off the menu key up events instead.
                    var activeInterrupts = Interrupts.Where(i => i != null && i.IsActive);
                    foreach (var i in activeInterrupts)
                    {
                        i.OnKeyUp(e);
                    }
                    return RoomState.SUSPEND;
                }
                return RoomState.NOTHING;
            }

            public RoomState Update()
            {
                if (interruptActive)
                {
                    var activeInterrupts = Interrupts.Where(i => i != null && i.IsActive);
                    foreach (var i in activeInterrupts)
                    {
                        i.Update();
                    }
                    return RoomState.SUSPEND;
                }
                return RoomState.NOTHING;
            }
        }


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

            InterruptHandler = new InterruptContainer(new List<Control>() { pnHelpMenu });
            
            timer1.Start();
        }

        
         //tells what we need to do with the room (suspend or resume...or nothing)
        public enum RoomState
        {
            RESUME = 0,
            SUSPEND = 1,
            NOTHING = 2
        }

        private void CrunchyLeavesMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (InterruptHandler.OnKeyDown(e))
            {
                case RoomState.SUSPEND:
                    Room.Suspend();
                    break;
                case RoomState.RESUME:
                    Room.Resume();
                    break;
                default:
                    Room.OnKeyPress(e);
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (InterruptHandler.Update())
            {
                case RoomState.SUSPEND:
                    break;
                default:
                    Room.Update();
                    break;
            }
        }

        private void CrunchyLeavesMain_KeyUp(object sender, KeyEventArgs e)
        {
            switch (InterruptHandler.OnKeyUp(e))
            {
                case RoomState.SUSPEND:
                    break;
                default:
                    Room.OnKeyUp(e);
                    break;
            }
        }
    }
}
