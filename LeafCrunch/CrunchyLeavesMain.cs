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
                new Room(pbLevel1), //eventually make it so it comes with leaves in a list from ext configuration maybe.
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
                UpdateSpeed();
                UpdateLocation();
            }

            private List<Keys> _activeKeys = new List<Keys>();
            List<Keys> ActiveKeys 
            {
                get
                {
                   return _activeKeys;
                }

                set
                { 
                    _activeKeys = value;
                } 
            }
            
            public override void OnKeyPress(KeyEventArgs e)
            {
                //ChangeSpeed(e);
                if (!ActiveKeys.Contains(e.KeyCode))
                    ActiveKeys.Add(e.KeyCode);
            }

            public override void OnKeyUp(KeyEventArgs e)
            {
                //ChangeSpeed(e, true);
                ActiveKeys.Remove(e.KeyCode);
            }

            protected void UpdateLocation()
            {
                if (Control == null) return;

                Control.Left += Speed.vx;
                if (Control.Left <= 0)
                {
                    while (Control.Left <= 0) Control.Left++;
                }

                Control.Top += Speed.vy;
                if (Control.Top <= 0)
                {
                    while (Control.Top <= 0) Control.Top++;
                }

                //this should probably be moved to collision checking code but right now let's just do it like this.
                if ((Control.Left + Control.Width) >= GlobalVars.RoomWidth)
                {
                    while ((Control.Left + Control.Width) >= GlobalVars.RoomWidth) Control.Left--;
                }
                if ((Control.Top + Control.Height) >= GlobalVars.RoomHeight)
                {
                    while ((Control.Top + Control.Height) >= GlobalVars.RoomHeight) Control.Top--;
                }
            }

            protected void UpdateVx()
            {
                if (ActiveKeys.Contains(Keys.Left))
                {
                    if (Speed.vx > 0) Speed.vx = 0; //change direction
                    Speed.vx -= 10;
                }
                else if (ActiveKeys.Contains(Keys.Right))
                {
                    if (Speed.vx < 0) Speed.vx = 0; //change direction
                    Speed.vx += 10;
                }
                else
                {
                    Speed.vx = 0;
                }
            }

            protected void UpdateVy()
            {
                if (ActiveKeys.Contains(Keys.Up))
                {
                    if (Speed.vy > 0) Speed.vy = 0; //change direction
                    Speed.vy -= 10;
                }
                else if (ActiveKeys.Contains(Keys.Down))
                {
                    if (Speed.vy < 0) Speed.vy = 0; //change direction
                    Speed.vy += 10;
                }
                else
                {
                    Speed.vy = 0;
                }
            }
            
            protected void UpdateSpeed()
            {
                UpdateVx();
                UpdateVy();
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
            public virtual void OnKeyUp(KeyEventArgs e) { }
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
