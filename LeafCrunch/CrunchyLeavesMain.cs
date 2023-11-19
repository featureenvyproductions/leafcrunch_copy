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
        private List<GenericInterrupt> Interrupts { get; set; }
        private Room Room { get; set; }

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
            
            Interrupts = new List<GenericInterrupt>()
            {
                new HelpMenu()
                {
                    IsActive = false,
                    Control = pnHelpMenu
                },
                new Pause()
                {
                    IsActive = false
                }
            };

            //before we start, hide all the menus
            foreach (var interrupt in Interrupts)
            {
                interrupt.Hide();
            }

            timer1.Start();
        }

        private bool interruptActive = false; //so we don't have to loop through all the interrupts for 3 different event handlers seeing if something is active

        private void CrunchyLeavesMain_KeyDown(object sender, KeyEventArgs e)
        {
            //the first thing we do is check to see if we're leaving a menu
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
                        //pass event handling back to the room
                        //i mean really what we need to do is have a state machine with a Room state and a Menu state
                        Room.Resume();
                    }
                    return;
                }

                //brute force close of all menus
                if (e.KeyCode == Keys.Escape)
                {
                    var activeInterrupts = Interrupts.Where(i => i != null && i.IsActive);
                    foreach (var i in activeInterrupts)
                    {
                        i.Deactivate();
                    }
                    interruptActive = false;
                    Room.Resume();
                    return;
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
                Room.Suspend();
                foreach (var i in relevantInterrupts)
                {
                    i.Activate();
                    //call their update functions
                    i.OnKeyPress(e);
                }
                return;
            }
            
            Room.OnKeyPress(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (interruptActive)
            {
                var activeInterrupts = Interrupts.Where(i => i != null && i.IsActive);
                foreach (var i in activeInterrupts)
                {
                    i.Update();
                }
                return;
            }

            Room.Update();
        }

        private void CrunchyLeavesMain_KeyUp(object sender, KeyEventArgs e)
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
                return;
            }

            Room.OnKeyUp(e);
        }
    }
}
