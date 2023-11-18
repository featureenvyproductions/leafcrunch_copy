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
                    //Control = ???
                }
            };

            timer1.Start();
        }

        private void CrunchyLeavesMain_KeyDown(object sender, KeyEventArgs e)
        {
            var relevantInterrupts = Interrupts.Where(i => i != null && !i.IsActive /*we don't need already active ones*/
                                                            && i.ActivationKey == e.KeyCode);
            foreach (var i in relevantInterrupts)
            {
                i.IsActive = true;
            }

            //don't waste time looping through objects if we're in a menu or paused
            //note: we'll have to do more here later on as we flesh out the menus of course
            //in case menus have key shortcuts
            //hm i wonder if escape should close them actually
            if (relevantInterrupts.Any()) return;
            
            //go through list of objects and fire off keypress events
            //you know I also wonder if we want to just store the key in general and not just for the player
            foreach (var obj in Objects)
            {
                obj.OnKeyPress(e);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var activeInterrupts = Interrupts.Where(i => i != null && i.IsActive);
            foreach (var i in activeInterrupts)
            {
                var menu = i as GenericMenu;
                if (menu != null) menu.Update();
            }

            if (!activeInterrupts.Any()) return; //don't do gameplay objects during interrupt.
            foreach (var obj in Objects)
            {
                obj.Update();
            }
        }

        //i wonder if i should have "random mode" and just it makes random leave controls everywhere

        private void CrunchyLeavesMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (Interrupts.Where(i => i != null && i.IsActive).Any()) return; //short circuit if a menu is up
            //note: could cause an issue later though if we have a keydown and the person is still holding it when we do the menu
            //maybe we should force a key up if that happens....somehow.
            
            //or we could store the active keys actually and then check if they're still pressed when we leave the menu.
            //yeah that could work
            //and then if not fire off the key up handler for the ones that aren't down anymore.

            //go through list of objects and fire off key up events
            foreach (var obj in Objects)
            {
                obj.OnKeyUp(e);
            }
        }
    }
}
