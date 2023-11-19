using LeafCrunch.Utilities;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace LeafCrunch.GameObjects
{
    //draws the points somewhere when they change or whatever 
    public class PointVisualizer
    {
        int displayTicks = 10;
        Label Control { get; set; }
        Control Parent { get; set; }
        Timer Timer = new Timer();
        int currentTicks = 0;

        public bool Active
        {
            get
            {
                return Timer.Enabled;
            }
        }

        public PointVisualizer(Control parent, int points)
        {
            Parent = parent;
            Control = new Label();
            Control.Text = $"+{points}";
            Control.Parent = parent;
            Parent.Controls.Add(Control);
            Control.Visible = true;
            Control.ForeColor = System.Drawing.Color.Black;
            Control.BackColor = System.Drawing.Color.Transparent;

            //uhg this all still makes it disappear when it goes off the person :<
            //i'll have to figure that out....

            Control.BringToFront();
            //Control.Left = parent.Left;
            //Control.Top = parent.Top - 32;
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            if (++currentTicks == displayTicks)
            {
                Control.Visible = false;
                Parent.Controls.Remove(Control);
                Timer.Stop();
            }
            else
            {
                //animate it because that's neat
                Control.Top--;
                Control.Left--;
            }
        }
    }

    public class Player : InteractiveGameObject
    {
        private List<PointVisualizer> _pointVisualizer = new List<PointVisualizer>();
        private int _maxRainbowPoints = 100;
        private int _rainbowPoints = 0;
        public int RainbowPoints
        {
            get { return _rainbowPoints; }
            set {
                //figure out the difference so we can display it just above the player's head
                var current = _rainbowPoints;
                if (value > _maxRainbowPoints) _rainbowPoints = _maxRainbowPoints;
                else _rainbowPoints = value;
                var diff = _rainbowPoints - current;
                _pointVisualizer.Add(new PointVisualizer(Control, diff));
            }
        }

        private void CleanUpPointVisualizers()
        {
            _pointVisualizer.RemoveAll(x => !x.Active);
        }

        private bool _isSuspended = false;

        public void Suspend()
        {
            _isSuspended = true;
            ActiveKeys.Clear();
            Speed.vy = 0;
            Speed.vx = 0;
        }

        public void Resume()
        {
            _isSuspended = false;
            //do we need to do anything else here?
        }

        public Player(Control control) : base(control)
        {
        }

        public override void Update()
        {
            if (_isSuspended) return;
            UpdateSpeed();
            UpdateLocation();
            CleanUpPointVisualizers();
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
            if (_isSuspended) return;
            if (!ActiveKeys.Contains(e.KeyCode))
                ActiveKeys.Add(e.KeyCode);
        }

        public override void OnKeyUp(KeyEventArgs e)
        {
            if (_isSuspended) return;
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
}
