using LeafCrunch.Utilities;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using LeafCrunch.GameObjects.Items.Obstacles;
using LeafCrunch.GameObjects.ItemProperties;
using System;

namespace LeafCrunch.GameObjects
{
    public class Player : InteractiveGameObject, IReboundable, ICollidable, IDamageReceptor
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

        public void ForceStop(Axis axisOfMotion)
        {
            //stop the player moving a direction and do the equivalent of forcing a key release
            switch (axisOfMotion)
            {
                case Axis.Vertical:
                    ActiveKeys.Remove(Keys.Up);
                    ActiveKeys.Remove(Keys.Down);
                    Speed.vy = 0;
                    break;
                case Axis.Horizontal:
                    ActiveKeys.Remove(Keys.Left);
                    ActiveKeys.Remove(Keys.Right);
                    Speed.vx = 0;
                    break;
            }
        }

        private bool CollisionX(Obstacle obstacle)
        {
            return (obstacle.CollisionX(Control.Left) ||
                    obstacle.CollisionX(Control.Right) ||
                    obstacle.TileIndex == TileIndex);
        }

        private bool CollisionY(Obstacle obstacle)
        {
            return obstacle.CollisionY(Control.Top) ||
                    obstacle.CollisionY(Control.Bottom) ||
                    obstacle.TileIndex == TileIndex;
        }

        public void Rebound(ICollidable collidable)
        {
            var obstacle = collidable as Obstacle;
            if (obstacle != null) Rebound(obstacle);
        }

        //for resolving collisions
        //I could probably also make this generic and have it as an interface I tack on
        //we'll see
        //to make the physics slightly less dumb this should also change depending on whether
        //the obstacle is also moving but we can deal with that later.
        //and i'm only gonna make the dumbest version of it
        private void Rebound(Obstacle obstacle)
        {
            //what direction were we heading in 
            var reboundSpeedx = -Speed.vx;
            var reboundSpeedy = -Speed.vy;

            var reboundable = obstacle as IReboundable;
            if (reboundable != null)
            {
                //this won't conserve momentum in every case, i do not care i am tired
                //placeholder code to fix later
                reboundSpeedx = obstacle.Speed.vx;
                reboundSpeedy = obstacle.Speed.vy;
                reboundable.Rebound(this);
            }

            //go the opposite way until the locations are different
            //was gonna use tile indexes but that might make corners suck
            if (reboundSpeedx != 0) //if we aren't moving in this direction, there's nothing to resolve
            {
                while (CollisionX(obstacle))
                {
                    Control.Left += reboundSpeedx;
                }
                ForceStop(Axis.Horizontal);
            }
            if (reboundSpeedy != 0)
            {
                while (CollisionY(obstacle))
                {
                    Control.Top += reboundSpeedy;
                }
                ForceStop(Axis.Vertical);
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

        //you know i should probably do something like this for other item effects tbh but whatever
        //to revisit
        public void ApplyDamage(object args)
        {
            var param = args as Dictionary<string, object>;
            if (param != null && param.Count > 0)
            {
                //right now we handle point decrements
                //but this could be expanded to handle other stuff.
                if (param.ContainsKey("RainbowPoints"))
                {
                    int points;
                    if (int.TryParse(param["RainbowPoints"].ToString(), out points))
                    {
                        RainbowPoints += points;
                    }
                }
            }
        }
    }
}
