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


    //note to self: pauses work but my "ignore the keys and do them later" code is still a bit wonky.
    //it seems to register them as pressed even if i press them while the menu is up and that stuff should be ignored.
    //it's fine we can come back and fix it later.
    //but yeah let's just short circuit the key down events

    //i should really just make a global keys list
    //and also just have Suspend() operations for the player and the room. 
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

                        //right now we only have one room but that might not always be true so I've got an active room thing
                        //there's probably a better way to check this
                        foreach(var obj in Objects)
                        {
                            Room room = obj as Room;
                            if (room != null && room.ActiveRoom) room.Resume();
                        }
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
                    //pass event handling back to the room
                    foreach (var obj in Objects)
                    {
                        Room room = obj as Room;
                        if (room != null && room.ActiveRoom) room.Resume();
                    }
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
                foreach (var obj in Objects)
                {
                    Room room = obj as Room;
                    if (room != null && room.ActiveRoom) room.Suspend();
                }
                foreach (var i in relevantInterrupts)
                {
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
