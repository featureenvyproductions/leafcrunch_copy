using LeafCrunch.GameObjects;
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
        private List<GenericGameObject> Objects { get; set; }
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
