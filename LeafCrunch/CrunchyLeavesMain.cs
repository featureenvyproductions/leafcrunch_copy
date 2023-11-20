using LeafCrunch.GameObjects;
using LeafCrunch.Menus;
using LeafCrunch.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
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

        public Image ImageFromPath(string relPath)
        {
            return Image.FromFile(Path.Combine(
                            Path.GetDirectoryName(
                                Assembly.GetExecutingAssembly().Location)
                            , relPath ));
        }
        public CrunchyLeavesMain()
        {
            InitializeComponent();

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
            new Dictionary<Direction, Player.ImageSequence>()
            {
                //static images
                //the only quirk is it immediately switches back to the none image....like
                //it's fine it's just weird
                {
                    Direction.None, new Player.ImageSequence(new List<Image>()
                    {
                        ImageFromPath("Images/Player/player_static_south.png")
                    })
                },
                {
                    Direction.South, new Player.ImageSequence(new List<Image>()
                    {
                        ImageFromPath("Images/Player/player_static_south.png")
                    })
                },
                {
                    Direction.East, new Player.ImageSequence(new List<Image>()
                    {
                        ImageFromPath("Images/Player/player_static_east.png")
                    })
                },
                {
                    Direction.West, new Player.ImageSequence(new List<Image>()
                    {
                        ImageFromPath("Images/Player/player_static_west.png")
                    })
                },
                {
                    Direction.North, new Player.ImageSequence(new List<Image>()
                    {
                        ImageFromPath("Images/Player/player_static_north.png")
                    })
                }
            },
            new Dictionary<Direction, Player.ImageSequence>()
            {
                //animation sequences
                {
                    Direction.None, new Player.ImageSequence(new List<Image>()
                    {
                        ImageFromPath("Images/Player/player_static.png")
                    })
                },
                {
                    Direction.South, new Player.ImageSequence(new List<Image>()
                    {
                        ImageFromPath("Images/Player/Animation/player_south_00.png"),
                        ImageFromPath("Images/Player/Animation/player_south_01.png")
                    })
                },
                {
                    Direction.East, new Player.ImageSequence(new List<Image>()
                    {
                        ImageFromPath("Images/Player/Animation/player_east_00.png"),
                        ImageFromPath("Images/Player/Animation/player_east_01.png")
                    })
                },
                {
                    Direction.West, new Player.ImageSequence(new List<Image>()
                    {
                        ImageFromPath("Images/Player/Animation/player_west_00.png"),
                        ImageFromPath("Images/Player/Animation/player_west_01.png")
                    })
                },
                {
                    Direction.North, new Player.ImageSequence(new List<Image>()
                    {
                        ImageFromPath("Images/Player/Animation/player_north_00.png"),
                        ImageFromPath("Images/Player/Animation/player_north_01.png")
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
