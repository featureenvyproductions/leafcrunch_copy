using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeafCrunch
{
    public partial class CrunchyLeavesMain : Form
    {
        private List<GenericGameObject> Objects { get; set; }
        public CrunchyLeavesMain()
        {
            InitializeComponent();
            var Player = new Player(pbPlayer);
            Objects = new List<GenericGameObject>()
            {
                Player,
                new Room(pbBackground),
                //random leaves
            };
            timer1.Start();
        }


        public class Player : InteractiveGameObject
        {
            public Player(Control control) : base(control)
            {
            }

            public override void Update()
            {
                UpdateLocation();
            }

            public override void OnKeyPress(KeyEventArgs e)
            {
                ChangeSpeed(e);
            }

            protected void UpdateLocation()
            {
                if (Control == null) return;

                //we probably don't need to care about speed y actually in this particular iteration
                Control.Left += Speed.vx;
                if (Control.Left <= 0)
                {
                    while (Control.Left <= 0) Control.Left++;
                }
                //this should probably be moved to collision checking code but right now let's just do it like this.
                if ((Control.Left + Control.Width) >= GlobalVars.RoomWidth)
                {
                    while ((Control.Left + Control.Width) >= GlobalVars.RoomWidth) Control.Left--;
                }
            }

            protected void ChangeSpeed(KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Left)
                {
                    if (Speed.vx > 0) Speed.vx = 0; //change direction
                    Speed.vx -= 10;
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (Speed.vx < 0) Speed.vx = 0; //change direction
                    Speed.vx += 10;
                }
                else
                {
                    Speed.vx = 0;
                }
                if (e.KeyCode == Keys.Up)
                {
                    if (Speed.vy < 75)
                        Speed.vy += 5;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (Speed.vy > 0)
                        Speed.vy -= 5;
                }
            }
        }

        public class InteractiveGameObject : GenericGameObject
        {
            public Speed Speed { get; set; }

            public InteractiveGameObject(Control control) : base(control)
            {
                Speed = new Speed() { vx = 0, vy = 0 };
            }
        }

        public class GlobalVars
        {
            public static int RoomWidth { get; set; }
            public static int RoomHeight { get; set; }
        }

        public class Room : GenericGameObject
        {
            public Room(Control control) : base(control)
            {
                GlobalVars.RoomWidth = control.Width;
                GlobalVars.RoomHeight = control.Height;
            }

            public override void Update()
            {
            }
        }

        public abstract class GenericGameObject
        {
            public Control Control { get; set; } //the associated forms control

            public GenericGameObject Parent { get; set; }

            public GenericGameObject(Control control)
            {
                Control = control;
            }

            public GenericGameObject(Control control, GenericGameObject parent)
            {
                Control = control;
                Parent = parent;
            }

            public virtual void Update() { }
            public virtual void OnKeyPress(KeyEventArgs e) { }
        }

        public class Speed
        {
            public int vx = 0;
            public int vy = 0;
        }

        private void CrunchyLeavesMain_KeyDown(object sender, KeyEventArgs e)
        {
            //go through list of objects and fire off keypress events
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
    }
}
