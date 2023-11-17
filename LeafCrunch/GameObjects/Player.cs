using LeafCrunch.Utilities;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects
{
    public class Player : InteractiveGameObject
    {
        private int _maxRainbowPoints = 100;
        private int _rainbowPoints = 0;
        public int RainbowPoints
        {
            get { return _rainbowPoints; }
            set {
                if (value > _maxRainbowPoints) _rainbowPoints = _maxRainbowPoints;
                else _rainbowPoints = value; 
            }
        }

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
}
