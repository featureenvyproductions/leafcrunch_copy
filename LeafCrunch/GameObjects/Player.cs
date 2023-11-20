using LeafCrunch.Utilities;
using System.Collections.Generic;
using System.Windows.Forms;
using LeafCrunch.GameObjects.Items.Obstacles;
using LeafCrunch.GameObjects.ItemProperties;
using System.Drawing;
using System.Linq;

namespace LeafCrunch.GameObjects
{
    public class Player : InteractiveGameObject, IReboundable, ICollidable, IDamageReceptor, IItemUser
    {
        private List<PointVisualizer> _pointVisualizer = new List<PointVisualizer>();
        private bool _isSuspended = false;

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

        public Player(Control control, 
            Dictionary<Direction, ImageSequence> staticImages,
            Dictionary<Direction, ImageSequence> animations) : base(control)
        {
            Sprite = new AnimatedSprite(staticImages, animations);
        }

        public override void Update()
        {
            if (_isSuspended) return;
            UpdateSpeed();
            UpdateLocation();
            UpdateAnimation();
            CleanUpPointVisualizers();
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

        private void ForceStop(Axis axisOfMotion)
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
            PreviousDirection = Direction;
            PreviousSpeed = Speed;
            UpdateVx();
            UpdateVy();
        }

        #region Animation

        //we're only doing the basic 4 directions
        //could expand to more in the future
        
        //so we can face the player the right direction when they're not moving.
        protected Direction PreviousDirection { get; set; }

     //   private Direction _direction = Direction.None;

        private Direction GetCurrentDirection()
        {
            if (Speed.vx < 0) return Direction.West;
            if (Speed.vx > 0) return Direction.East;
            if (Speed.vy < 0) return Direction.North;
            if (Speed.vy > 0) return Direction.South;
            return Direction.None;
        }

        protected Direction Direction
        {
            get
            {
                //PreviousDirection = _direction;
                // _direction = GetCurrentDirection();
                return GetCurrentDirection();// _direction;
            }
        }

        public class ImageSequence
        {
            //treat as static if we only have one image in list...this is more for consolidating code
            public List<Image> Images = new List<Image>();
            private int _currentIndex = 0;
            private int _frameCount = 0; //so we don't have to count the animation frames every single time

            //the image that's currently visible
            public Image CurrentImage { get; set; }

            public ImageSequence(List<Image> images)
            {
                Images = images;
                CurrentImage = Images.FirstOrDefault();
                _frameCount = Images.Count;
            }

            public void ResetAnimation()
            {
                _currentIndex = 0;
                CurrentImage = Images.ElementAt(0);
            }

            public void UpdateFrame()
            {
                if (_frameCount > 1)
                {
                    if (_currentIndex + 1 == _frameCount) _currentIndex = 0;
                    else ++_currentIndex;
                    CurrentImage = Images.ElementAt(_currentIndex);
                }
                //if it is static we don't do anything bc the displayed image never changes
            }
        }

        protected AnimatedSprite Sprite { get; set; }

        public class AnimatedSprite
        {
            public Dictionary<Direction, ImageSequence> StaticImage { get; set; }
            public Dictionary<Direction, ImageSequence> Animation { get; set; }

            private ImageSequence _currentAnimation = null;

            public Image CurrentImage { 
                get
                {
                    return _currentAnimation.CurrentImage;
                }
            }

            public AnimatedSprite(Dictionary<Direction, ImageSequence> staticImages,
                Dictionary<Direction, ImageSequence> animations)
            {
                StaticImage = staticImages;
                Animation = animations;
                //we could check these to make sure they're valid and whatever
                //but like if you're implementing the animation interface it's bc you have an image
                //so why wouldn't you have at least the absolute bare minimum default image
                _currentAnimation = StaticImage[Direction.None];
            }

            //right now i'll just mostly have walking/standing animations but i could make special ones eventually

            //whoever calls this should have calculated which animation we need to display
            //and whether it's static/we're moving
            //this should just check whether things changed and reset the frame count and index if so.
            public void UpdateSequence(Direction direction, bool staticImage)
            {
                ImageSequence targetAnimation;
                if (staticImage)
                {
                    targetAnimation = StaticImage[direction];
                }
                else
                {
                    targetAnimation = Animation[direction];
                }

                if (_currentAnimation.Equals(targetAnimation))
                {
                    //no changes are needed
                    //update the frame
                    _currentAnimation.UpdateFrame();
                }
                else
                {
                    //replace the current animation
                    _currentAnimation = targetAnimation;
                    _currentAnimation.ResetAnimation();
                }
            }
        }

        //how many ticks have gone by since the last frame change
        protected int AnimationTicks = 0;

        //this should really be calculated since it depends on how fast the main timer that controls the game updates ticks
        protected int FrameTickCount = 1;

        //I think I will put this stuff in an interface
        //so other objects can use it
        protected void UpdateAnimation()
        {
            //basically if enough ticks have gone by
            //we update the animation to be the next frame in the current one
            //and the frame we show depends on the direction
            //and if we're stopped, the previous direction.
            
            //check the tick count
            if (AnimationTicks == FrameTickCount)
            {
                //time to update
                //reset the count
                AnimationTicks = 0;

                //are we moving
                //if not, get the previous direction and display it
                if (Speed.vx == 0 && Speed.vy == 0)
                {
                    //did we just stop
                    if (Speed != PreviousSpeed)
                    {
                        //we need to pass the previous direction so the sprite faces the right way
                        Sprite.UpdateSequence(PreviousDirection, true);
                    }
                    else
                    {
                        Sprite.UpdateSequence(Direction, true);
                    }
                }
                else
                {
                    //we are moving
                    //use the right animation for the direction headed
                    Sprite.UpdateSequence(Direction, false);
                }
            }
            else
            {
                //not ready to update the image yet
                AnimationTicks++;
            }

            //whatever happens, make sure we're displaying the right image in the control
            //this had better be a picture box or we have bigger problems
            (Control as PictureBox).Image = Sprite.CurrentImage;
        }

        public Speed PreviousSpeed { get; set; }
        #endregion

        //you know i should probably do something like this for other item effects tbh but whatever
        //to revisit
        public void ApplyDamage(object args)
        {
            var param = args as Dictionary<string, object>;
            if (param != null && param.Count > 0) ApplyPointChanges(param);
            //we might do other things here later
        }

        public void ApplyItem(object args)
        {
            var param = args as Dictionary<string, object>;
            if (param != null && param.Count > 0) ApplyPointChanges(param);
            //might do other things here later
        }

        protected void ApplyPointChanges(Dictionary<string, object> param)
        {
            if (param.ContainsKey("RainbowPoints"))
            {
                int points;
                if (int.TryParse(param["RainbowPoints"].ToString(), out points))
                {
                    RainbowPoints += points;
                }
            }
        }

        private void CleanUpPointVisualizers()
        {
            _pointVisualizer.RemoveAll(x => !x.Active);
        }
    }
}
