using LeafCrunch.Utilities;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace LeafCrunch.GameObjects
{
    //draws the points somewhere when they change or whatever 
    public class PointVisualizer
    {
        private int _displayTicks = 10;
        private Label _control { get; set; }
        private Control _parent { get; set; }
        private Timer _timer = new Timer();
        private int _currentTicks = 0;

        public bool Active
        {
            get
            {
                return _timer.Enabled;
            }
        }

        public PointVisualizer(Control parent, int points)
        {
            _parent = parent;
            _control = new Label();
            _control.Text = $"+{points}";
            _control.Parent = parent;
            _parent.Controls.Add(_control);
            _control.Visible = true;
            _control.ForeColor = System.Drawing.Color.Black;
            _control.BackColor = System.Drawing.Color.Transparent;

            //uhg this all still makes it disappear when it goes off the person :<
            //i'll have to figure that out....

            _control.BringToFront();
            //Control.Left = parent.Left;
            //Control.Top = parent.Top - 32;
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            if (++_currentTicks == _displayTicks)
            {
                _control.Visible = false;
                _parent.Controls.Remove(_control);
                _timer.Stop();
            }
            else
            {
                //animate it because that's neat
                _control.Top--;
                _control.Left--;
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

        public void ForceStop(bool upDown)
        {
            //stop the player moving a direction and do the equivalent of forcing a key release
            if (upDown)
            {
                ActiveKeys.Remove(Keys.Up);
                ActiveKeys.Remove(Keys.Down);
                Speed.vy = 0;
            }
            else
            {
                ActiveKeys.Remove(Keys.Left);
                ActiveKeys.Remove(Keys.Right);
                Speed.vx = 0;
            }
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
