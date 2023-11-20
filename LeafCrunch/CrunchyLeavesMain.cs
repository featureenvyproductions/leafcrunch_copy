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
    ""Static"":
        {
            ""Default"" :{
              ""ImagePaths"": [
                ""Images/Player/player_static_south.png""
              ]
            },
            ""South"" :{
              ""ImagePaths"": [
                ""Images/Player/player_static_south.png""
              ]
            },
            ""North"" :{
              ""ImagePaths"": [
                ""Images/Player/player_static_north.png""
              ]
            },
            ""East"" :{
              ""ImagePaths"": [
                ""Images/Player/player_static_east.png""
              ]
            },
            ""West"":{
              ""ImagePaths"": [
                ""Images/Player/player_static_west.png""
              ]
            }
        },
    ""Moving"" : 
        {
            ""Default"" :{
              ""ImagePaths"": [
                ""Images/Player/player_static.png""
              ]
            },
            ""South"" :{
              ""ImagePaths"": [
                ""Images/Player/Animation/player_south_00.png"",
                ""Images/Player/Animation/player_south_01.png""
              ]
            },
            ""North"" :{
              ""ImagePaths"": [
                ""Images/Player/Animation/player_north_00.png"",
                ""Images/Player/Animation/player_north_01.png""
              ]
            },
            ""East"" :{
              ""ImagePaths"": [
                ""Images/Player/Animation/player_east_00.png"",
                ""Images/Player/Animation/player_east_01.png""
              ]
            },
            ""West"":{
              ""ImagePaths"": [
                ""Images/Player/Animation/player_west_00.png"",
                ""Images/Player/Animation/player_west_01.png""
              ]
            }
        }
}
";
            var spriteloader = new SpriteLoader();
            var spriteData = spriteloader.Load(jsonString);
            var staticsprite = spriteData["Static"];
            var movingsprite = spriteData["Moving"];

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
            staticsprite,
            movingsprite
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
