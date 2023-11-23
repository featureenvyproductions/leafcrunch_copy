using LeafCrunch.Utilities;
using System.Collections.Generic;
using System.Windows.Forms;
using LeafCrunch.GameObjects.Items.Obstacles;
using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.Utilities.Animation;
using LeafCrunch.Utilities.Entities;

namespace LeafCrunch.GameObjects
{
    public class Player : InteractiveGameObject, IReboundable, ICollidable, IDamageReceptor, IItemUser, IAnimated
    {
        private string _objectName = "Player";

        #region Pause Properties
        private bool _isSuspended = false;
        #endregion 

        #region Points Properties
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
        #endregion

        #region Key Press Properties
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
        #endregion

        #region Animation Related Properties
        private int _animationTicks = 0;
        public int AnimationTicks
        { 
            get { return _animationTicks;}
            set { _animationTicks = value; }
        }

        private Speed _previousSpeed = new Speed() { vx = 0, vy = 0 };
        public Speed PreviousSpeed
        {
            get { return _previousSpeed; }
            set { _previousSpeed = value; }
        }

        private Direction _previousDirection = Direction.None;
        public Direction PreviousDirection
        {
            get { return _previousDirection; }
            set { _previousDirection = value; }
        }

        public Direction Direction
        {
            get
            {
                return GetCurrentDirection();
            }
        }

        public AnimatedSprite Sprite { get; set; }
        #endregion

        #region Loading and Initialization
        //how should we do the data
        //i guess let's have a file for each entity maybe
        //so like player, objects, items, etc

        //i should probably set some kind of boolean to make sure initialization finishes and then
        //check it on update
        //maybe put it in the generic class and check it for every object.

        private bool _isInitialized = false;
        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }

        private const string _configFile = "player.json";
        override public string ConfigFile
        {
            get { return _configFile; }
        }

        public override void Initialize()
        {
            //get the file data
            var jsonString = Load();
            if (string.IsNullOrEmpty(jsonString)) return; //maybe even throw an exception here

            //load up all the stuff
            var loader = new PlayerLoader();
            var playerData = loader.Load(jsonString);

            if (playerData == null || playerData.Sprite == null || playerData.Stats == null) return;

            _maxRainbowPoints = playerData.Stats.MaxPoints;
            Speed = new Speed()
            {
                vx = playerData.Stats.InitialSpeedX,
                vy = playerData.Stats.InitialSpeedY
            };
            Sprite = playerData.Sprite; //of course things could have gone wrong when initializing this but i don't have
            //emotional energy to check all that.
            //I'll make some friggin validator code later or some shit but right now
            //the game should just stop working anyway if the stuff isn't there that needs to be there

            //it looks like everything is initializing to show an image right away BUT I'M JUST CHECKING
            Sprite.UpdateSequence(Direction.South, true);

            //double check that adding this to the child controls of the room will set the parent of this to the room
            Control = new PictureBox()
            {
                Name = _objectName,
               // Image = Sprite.CurrentImage,
                Width = Sprite.CurrentImage.Width,
                Height = Sprite.CurrentImage.Height,
                Top = playerData.Stats.InitialY,
                Left = playerData.Stats.InitialX,
                BackColor = System.Drawing.Color.Transparent
            };

            //eventually we'll probably need to have special sprites as well but we'll come back to that
            //like the stomp animation
            IsInitialized = true;
            GenericGameObjectRegistry.RegisteredObjects.Add(_objectName, this);
        }
        #endregion

        #region Constructors
        public Player() : base()
        {
            Initialize();
        }
        #endregion

        #region Event Handling
        public override void Update()
        {
            if (_isSuspended || !IsInitialized) return;

            UpdateSpeed();
            UpdateLocation();
            UpdateAnimation();
            CleanUpPointVisualizers();
        }

        public override void OnKeyPress(KeyEventArgs e)
        {
            if (_isSuspended || !IsInitialized) return;
            if (!ActiveKeys.Contains(e.KeyCode))
                ActiveKeys.Add(e.KeyCode);
        }

        public override void OnKeyUp(KeyEventArgs e)
        {
            if (_isSuspended || !IsInitialized) return;

            //stupid as hell but this isn't saving correctly in the update method
            Direction makeMeAFuckingCopy;
            Direction.TryParse(Direction.ToString(), out makeMeAFuckingCopy); //is it just referencing the fucking direction
            PreviousDirection = makeMeAFuckingCopy;
            //maybe if we're already stationary don't set this
            if (PreviousSpeed == null) PreviousSpeed = new Speed();
            PreviousSpeed.vx = Speed.vx; //for whatever reason this doesn't fucking work
            PreviousSpeed.vy = Speed.vy;

            ActiveKeys.Remove(e.KeyCode);
        }
        #endregion

        #region Pause Handling
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
        #endregion

        #region Motion
        protected void UpdateLocation()
        {
            if (Control == null) return;

            Control.Left += Speed.vx;
            if (Control.Left <= 0)
            {
                while (Control.Left <= 0) Control.Left++;
            }

            Control.Top += Speed.vy;
            if (Control.Top <= GlobalVars.RoomTopMargin)
            {
                while (Control.Top <= GlobalVars.RoomTopMargin) Control.Top++;
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
            UpdateVx();
            UpdateVy();
        }
        #endregion

        #region Collision Handling
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
            //need to save this stuff before a rebound for the animation to work 
            //still stupid but better
            Direction makeMeAFuckingCopy;
            Direction.TryParse(Direction.ToString(), out makeMeAFuckingCopy); //is it just referencing the fucking direction
            PreviousDirection = makeMeAFuckingCopy;
            //maybe if we're already stationary don't set this
            if (PreviousSpeed == null) PreviousSpeed = new Speed();
            PreviousSpeed.vx = Speed.vx; //for whatever reason this doesn't fucking work
            PreviousSpeed.vy = Speed.vy;


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
        #endregion

        #region Operation Handling
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

        #endregion

        #region Point Visualization Handling
        private void CleanUpPointVisualizers()
        {
            _pointVisualizer.RemoveAll(x => !x.Active);
        }
        #endregion

        #region Animation Handling
        public Direction GetCurrentDirection()
        {
            if (Speed.vx < 0) return Direction.West;
            if (Speed.vx > 0) return Direction.East;
            if (Speed.vy < 0) return Direction.North;
            if (Speed.vy > 0) return Direction.South;
            return Direction.None;
        }

        public void UpdateAnimation()
        {
            if (!IsInitialized) return;
            //basically if enough ticks have gone by
            //we update the animation to be the next frame in the current one
            //and the frame we show depends on the direction
            //and if we're stopped, the previous direction.

            //check the tick count
            if (AnimationTicks == GlobalVars.FrameTickCount)
            {
                //time to update
                //reset the count
                AnimationTicks = 0;

                //are we moving
                //if not, get the previous direction and display it
                //for whatever reason this just sometimes straight up decides not to work
                //maybe i need to save the previous values in my key up event
                if (Speed.vx == 0 && Speed.vy == 0)
                {
                    //did we just stop
                    if (PreviousSpeed != null && (Speed.vx != PreviousSpeed.vx || Speed.vy != PreviousSpeed.vy))
                    {
                        //we need to pass the previous direction so the sprite faces the right way
                        Sprite.UpdateSequence(PreviousDirection, true);
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
          //  (Control as PictureBox).Image = Sprite.CurrentImage;
        }
        #endregion
    }
}
