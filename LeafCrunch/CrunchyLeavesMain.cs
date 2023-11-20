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
            string jsonString =
@"{
  ""ImagePaths"": [
    ""Images/Player/player_static_south.png""
  ]
}
";
            string jsonStringN =
@"{
  ""ImagePaths"": [
    ""Images/Player/player_static_north.png""
  ]
}
";
            string jsonStringE =
@"{
  ""ImagePaths"": [
    ""Images/Player/player_static_east.png""
  ]
}
";
            string jsonStringW =
@"{
  ""ImagePaths"": [
    ""Images/Player/player_static_west.png""
  ]
}
";
            var imageloader = new ImageSequenceLoader();

            RoomController = new RoomController(pbLevel1, pbPlayer, lblRainbowPoints, lblCountDown, 
            new List<Control>() {
                pbGreenLeaf01,
                pbYellowLeaf01,
                pbOrangeLeaf01,
                pbRedLeaf01,
                pbPineCone01
            },
            new List<Control>()
            {
                pbGenericObstacle,
                pbStationaryHazard
            },
            new List<Control>()
            { 
                pbMovingObstacle,
                pbHazard
            },
            new Dictionary<Direction, ImageSequence>()
            {
                //static images
                //the only quirk is it immediately switches back to the none image....like
                //it's fine it's just weird
                {
                    Direction.None, new ImageSequence(imageloader.Load(jsonString))
                },
                {
                    Direction.South, new ImageSequence(imageloader.Load(jsonString))
                },
                {
                    Direction.East, new ImageSequence(imageloader.Load(jsonStringE))
                },
                {
                    Direction.West, new ImageSequence(imageloader.Load(jsonStringW))
                },
                {
                    Direction.North, new ImageSequence(imageloader.Load(jsonStringN))
                }
            },
            new Dictionary<Direction, ImageSequence>()
            {
                //animation sequences
                {
                    Direction.None, new ImageSequence(new List<Image>()
                    {
                        UtilityMethods.ImageFromPath("Images/Player/player_static.png")
                    })
                },
                {
                    Direction.South, new ImageSequence(new List<Image>()
                    {
                        UtilityMethods.ImageFromPath("Images/Player/Animation/player_south_00.png"),
                        UtilityMethods.ImageFromPath("Images/Player/Animation/player_south_01.png")
                    })
                },
                {
                    Direction.East, new ImageSequence(new List<Image>()
                    {
                        UtilityMethods.ImageFromPath("Images/Player/Animation/player_east_00.png"),
                        UtilityMethods.ImageFromPath("Images/Player/Animation/player_east_01.png")
                    })
                },
                {
                    Direction.West, new ImageSequence(new List<Image>()
                    {
                        UtilityMethods.ImageFromPath("Images/Player/Animation/player_west_00.png"),
                        UtilityMethods.ImageFromPath("Images/Player/Animation/player_west_01.png")
                    })
                },
                {
                    Direction.North, new ImageSequence(new List<Image>()
                    {
                        UtilityMethods.ImageFromPath("Images/Player/Animation/player_north_00.png"),
                        UtilityMethods.ImageFromPath("Images/Player/Animation/player_north_01.png")
                    })
                }
            }
            );

            InterruptController = new InterruptController(new List<Control>() { pnHelpMenu });
            
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
