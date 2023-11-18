using LeafCrunch.GameObjects;
using LeafCrunch.Menus;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using LeafCrunch.Utilities;

namespace LeafCrunch
{
    //still need a main timer
    //maybe a help screen
    //and transitions between levels
    //and barriers
    //sounds also
    //do we want some kind of mystery box thing?


    //note to self: pauses work but my "ignore the keys and do them later" code is still a bit wonky.
    //it seems to register them as pressed even if i press them while the menu is up and that stuff should be ignored.
    //it's fine we can come back and fix it later.
    public partial class CrunchyLeavesMain : Form
    {
        private List<GenericGameObject> Objects { get; set; }
        private List<GenericInterrupt> Interrupts { get; set; }

        public CrunchyLeavesMain()
        {
            InitializeComponent();
            var Player = new Player(pbPlayer);
            var items = new List<GenericItem>()
            {
                new GreenLeaf(pbGreenLeaf01, new Operation()
                {
                    Target = Player
                }),
                new YellowLeaf(pbYellowLeaf01, new Operation()
                {
                    Target = Player,
                }),
                new OrangeLeaf(pbOrangeLeaf01, new Operation()
                {
                    Target = Player
                }),
                new RedLeaf(pbRedLeaf01, new Operation()
                {
                    Target = Player
                })
            };
            var pinecone = new PineCone(pbPineCone01, new MultiTargetOperation()
            {
                Targets = new List<GenericGameObject>()
            }, lblCountDown);

            //oh i've made this quite stupid actually wow i need to make this considerably less stupid
            var pineconeTargets = (pinecone.Operation as MultiTargetOperation).Targets;
            foreach (GenericGameObject item in items)
            {
                pineconeTargets.Add(item);
            }

            //let's try adding a leaf
            //would be nice if we eventually randomly added leaves to the board and blew some away.
            //like the beginning levels would be easy, we'd just have static leaves.
            //but then as they get harder we can have some blowing in or blowing away
            //we also need an obstacles engine now for mazes.
            var Room = new Room(pbLevel1)
            {
                Player = Player,
                Items = new List<GenericItem>()
            };
            Room.Items.AddRange(items);
            Room.Items.Add(pinecone);
            Room.RegisterTemporaryItems();
            
            //eventually we can add timer and power ups (e.g. pine cone active) and whatever else we want in stats
            
            var stats = new StatsDisplay(lblRainbowPoints, Player);

            Objects = new List<GenericGameObject>()
            {
                Player,
                stats,
                Room
            };

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

        private List<Keys> activeKeys = new List<Keys>();
        private List<Keys> storedActiveKeys = new List<Keys>();
        private List<Keys> delayedDeactivationKeys = new List<Keys>();

        private void CrunchyLeavesMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (!activeKeys.Contains(e.KeyCode)) activeKeys.Add(e.KeyCode);

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
                        //do all of the cleanup and key ups
                        interruptActive = false;

                        //check any keys that were pressed before we opened the menu
                        //see if they're still pressed
                        //and if not, fire any key up events
                        foreach (var key in storedActiveKeys)
                        {
                            if (!activeKeys.Contains(key))
                            {
                                delayedDeactivationKeys.Add(key);
                            }
                        }
                        storedActiveKeys.Clear();
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

                    //check any keys that were pressed before we opened the menu
                    //see if they're still pressed
                    //and if not, fire any key up events
                    foreach (var key in storedActiveKeys)
                    {
                        if (!activeKeys.Contains(key))
                        {
                            delayedDeactivationKeys.Add(key);
                        }
                    }
                    storedActiveKeys.Clear();
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
                //i should do a thing to make sure we can't activate more than one menu
                //unless it's a specific kind of menu
                //or if we press the same key again it'll hide the menu
                interruptActive = true; 
                foreach (var i in relevantInterrupts)
                {
                    //if we're entering a menu for the first time, we need to store the active keys.
                    //not the one for the current menu of course
                    if (!i.IsActive)
                    {
                        storedActiveKeys.AddRange(activeKeys.Where(k => k != e.KeyCode));
                    }
                    i.Activate();
                    //call their update functions
                    i.OnKeyPress(e);
                }
                return;
            }
            
            //go through list of objects and fire off keypress events
            //you know I also wonder if we want to just store the key in general and not just for the player
            foreach (var obj in Objects)
            {
                obj.OnKeyPress(e);
            }
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

            foreach (var obj in Objects)
            {
                obj.Update();
            }
        }

        //i wonder if i should have "random mode" and just it makes random leave controls everywhere

        private void CrunchyLeavesMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (activeKeys.Contains(e.KeyCode)) activeKeys.Remove(e.KeyCode);

            //if there are any delayed key up events (i.e. we went into a menu with a key pressed and released the key before
            //leaving the menu
            //like say we were walking and hit the menu button
            //but stopped walking
            //idk maybe this is overkill
            //but it's whatever
            //i think i'm trying to prevent a thing where we never see the key up event so you leave the menu
            //and like KEEP WALKING WHEN YOURE NOT PUSHING ANYTHING you know
            //this doesn't quite work but eh. we can fix it later

            if (delayedDeactivationKeys != null)
            {
                foreach (var key in delayedDeactivationKeys)
                {
                    foreach (var obj in Objects)
                    {
                        obj.OnKeyUp(e);
                    }
                }
                delayedDeactivationKeys.Clear();
            }

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

            foreach (var obj in Objects)
            {
                obj.OnKeyUp(e);
            }
        }
    }
}
