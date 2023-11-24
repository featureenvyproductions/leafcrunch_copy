using LeafCrunch.Utilities;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using LeafCrunch.GameObjects.Items;
using LeafCrunch.GameObjects.Items.InstantItems;
using LeafCrunch.GameObjects.Items.ItemOperations;
using LeafCrunch.GameObjects.Items.TemporaryItems;
using LeafCrunch.GameObjects.Stats;
using LeafCrunch.GameObjects.Items.Obstacles;
using LeafCrunch.Utilities.Entities;
using System.IO;
using static LeafCrunch.Utilities.GlobalVars;
using System;
using System.Drawing;
using System.Drawing.Imaging;

//when we fill up the rainbow bar, fix the text to show points as percentages of 100 points rather than
//explicitly showing points
//also for some exceptionally hard levels, you win if you get any points at all.

//ohhhhh i need to account for things drawing over the protgonist like tree trunks
//like in a better way than i did

//also i need to change the size of the young sprite

//you know one thing i should do is when i actually implement the dynamic loading of the game board
//i should make it so that i only place things exactly in tiles
//because right now dropping them on the board in the form designer
//i'm not like getting them exact, so it makes the collisions a little funny
//if i had a virtual grid and loaded things into it, the collision code would be a lot less fucking stupid
//i also didn't consider how i want to handle things that are like more than one tile big but tbh, that's
//not REALLY going to be a thing. I think what I would probably do is just have an overlay and bring it to the front after
//initializing all the underlying obstacle tiles. that's fine probably idk.
//i probably don't need to get that complex for this....most of the obstacles are going to be rocks or fences or like
//cats

//note to self....I actually just wanted to calculate the tile of the destination origin pixel and use that
//for a comparison
//that should work pretty well we can come back to that. 

//also collect factory code into its own folder so the loading/initializing stuff isn't a fucking mess.

//also also why isn't the generic game object class the thing that registers shit in the fucking registry
//what is my problem

//hoo boy do i need a level builder
//and/or i could make it so when you only include an x or y coordinate it means just repeat that obstacle along that whole axis


//oh i need to draw a T-wall oops

namespace LeafCrunch.GameObjects
{
    public class RoomController : GenericGameObject
    {
        private bool _interactive = true; //eh there's probably a better way to do this but
        public bool ActiveRoom = true; //always true right now, idk if we want more rooms in the future
        private bool _isInitialized = false;

        #region Win Condition Properties

        public WinCondition WinCondition {
            get
            {
                //wait till we're loaded and running
                if (!_isInitialized || _isSuspended) return WinCondition.None;
                foreach (var condition in _loseConditions)
                {
                    //one and done
                    if (condition.CheckCondition(this) == WinCondition.Lose)
                        return WinCondition.Lose;
                }
                foreach (var condition in _winConditions)
                {
                    if (condition.CheckCondition(this) == WinCondition.None)
                        return WinCondition.None;
                }
                return WinCondition.Win;
            }
        }

        private int _totalTicks = 0;
        public int TotalTicks
        { 
            get { return _totalTicks; }
            set { _totalTicks = value; }
        }

        //i mean I suppose I could give the player a list of win conditions to check but
        //i'm tired and i'm not doing that
        public int TotalPoints
        {
            get
            {
                if (Player != null) return Player.RainbowPoints;
                return 0;
            }
        }

        //i'm only doing win conditions right now.
        //i'll have a separate list of lose conditions
        //but we'll come back to that

        private List<Condition> _winConditions = new List<Condition>();
        private List<Condition> _loseConditions = new List<Condition>();

        #endregion

        #region Pause Handling
        private bool _isSuspended = false; //says whether or not the room is active (as opposed to a menu)
        #endregion

        #region Room Objects
        private string _roomName = string.Empty;
        public string RoomName
        {
            get { return _roomName; }
            set { _roomName = value; }
        }
        private List<Obstacle> _obstacles = new List<Obstacle>();
        private List<GenericItem> _items = new List<GenericItem>();
        private List<TemporaryItem> _temporaryItems = new List<TemporaryItem>();
        private List<TemporaryItem> _activeItems = new List<TemporaryItem>();
        private List<MovingObstacle> _movingObstacles = new List<MovingObstacle>();
        #endregion

        #region Player Properties
        //i guess we should know about the player
        protected Player Player { get; set; }
        #endregion

        #region Stats Properties
        public StatsDisplay StatsDisplay { get; set; }
        #endregion

        #region Key Handling Properties
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

        protected bool ItemActiveKeyPressed(GenericItem i) => i.ActivationKey == Keys.None || ActiveKeys.Contains(i.ActivationKey);

        //for instruction screen
        //this stuff should probably be its own thing but whatever
        private Keys _continueKey = Keys.None;
        public Keys ContinueKey
        {
            get { return _continueKey; }
            set { _continueKey = value; }
        }

        private bool _continue = false;
        public bool Continue
        {
            get { return _continue; }
            set { _continue = value; }
        }
        #endregion

        #region Collision Handler Related
        protected bool TileObjectPlayerCollision(GenericItem i) => i.TileIndex == Player.TileIndex;
        #endregion

        //got some cleaning up to do but
        public Image StatsImage { get; set; }
        public Image RainbowBarOverlay { get; set; }
        public Image RainbowBar { get; set; }

        #region Constructors
        //eventually we're going to load control names from a file I think so I won't need this fucking list
        //or we're gonna initialize the controls on the fly with a location and a type
        //like i'll have a prototype and initialize from the prototype
        public RoomController(Form parent, string roomName) : base()
        {
            _isInitialized = false;
            //i wonder if these would be better somewhere else
            GenericGameObjectRegistry.RegisteredObjects = new Dictionary<string, GenericGameObject>();
            OperationMethodRegistry.TargetOperations = new Dictionary<string, TargetOperation>();
            OperationRegistry.Operations = new Dictionary<string, Operation>();

            _roomName = roomName;
            var jsonString = File.ReadAllText(UtilityMethods.GetConfigPath($"Rooms/{_roomName}/room.json"));
            var jsonLoader = new JsonLoader();
            var roomData = jsonLoader.LoadFromJson<RoomData>(jsonString);

            //really eventually we'll have a specific stats control and get the margin based on that
            //but we're just doing this for now
            GlobalVars.RoomTopMargin = 40;
            GlobalVars.RoomWidth = roomData.Width;
            //we're gonna leave some padding to avoid stats items i think but idk how I want to do that
            //i may need to have a room top coordinate
            GlobalVars.RoomHeight = roomData.Height;
            GlobalVars.RoomTileSizeW = roomData.TileSizeW;
            GlobalVars.RoomTileSizeH = roomData.TileSizeH;

            //hacky....make it make sense
            //image to help stats show up better
            //like really this needs to be its own object associated with whatever it's displaying
            //and room controller can see its image path and window location
            if (!string.IsNullOrEmpty(roomData.StatsBackgroundImage))
                StatsImage = UtilityMethods.ImageFromPath(roomData.StatsBackgroundImage);

            if (!string.IsNullOrEmpty(roomData.RainbowBarOverlay))
                RainbowBarOverlay = UtilityMethods.ImageFromPath(roomData.RainbowBarOverlay);
            if (!string.IsNullOrEmpty(roomData.RainbowBar))
                RainbowBar = UtilityMethods.ImageFromPath(roomData.RainbowBar);
            //end hacky bit


            var img = UtilityMethods.ImageFromPath(roomData.BackgroundImagePath);
            Control = new PictureBox()
            {
                Top = 0,
                Left = 0,
                Width = GlobalVars.RoomWidth,
                Height = GlobalVars.RoomHeight,
                Image = img
            };

            parent.Controls.Add(Control);
            Control.BringToFront();

            //could i consolidate this
            LoadWinConditions(roomData.WinConditions);

            if (roomData.RoomType == "Transition")
            {
                _isInitialized = true;
                _interactive = false;

                //set continue key if the pause is indefinite
                Enum.TryParse(roomData.ContinueKey ?? "None", out _continueKey);
                return;
            }
            LoadLoseConditions(roomData.LoseConditions);
            LoadRoomObjects();

            if (Player != null)
            {
                if (roomData.InitialPlayerX >= 0) Player.X = roomData.InitialPlayerX;
                if (roomData.InitialPlayerY >= 0) Player.Y = roomData.InitialPlayerY;
            }
            _isInitialized = true;
        }

        protected void LoadWinConditions(List<ConditionData> conditionData)
        {
            foreach (var condition in conditionData)
            {
                _winConditions.Add(new Condition()
                {
                    PropertyName = condition.PropertyName,
                    Value = condition.Value,
                    Comparison = condition.Comparison,
                    ValueType = condition.ValueType,
                    WinCondition = WinCondition.Win
                });
            }
        }

        protected void LoadLoseConditions(List<ConditionData> conditionData)
        {
            foreach (var condition in conditionData)
            {
                _loseConditions.Add(new Condition()
                {
                    PropertyName = condition.PropertyName,
                    Value = condition.Value,
                    Comparison = condition.Comparison,
                    ValueType = condition.ValueType,
                    WinCondition = WinCondition.Lose
                });
            }
        }
        #endregion

        #region Loading and Initialization
        protected void LoadRoomObjects()
        {
            LoadPlayer();
            LoadOperations();
            LoadItems();
            LoadObstacles();            
        }

        protected void LoadPlayer()
        {
            Player = new Player();
            if (Player.IsInitialized)
            {
                StatsDisplay = new StatsDisplay(Player);
            }
        }

        protected void LoadOperations()
        {
            OperationRegistry.Load();
        }

        protected void LoadItems()
        {
            var itemFactory = new ItemFactory();
            _items = itemFactory.LoadItems(_roomName);
            RegisterTemporaryItems();
        }

        protected void RegisterTemporaryItems()
        {
            if (_items == null) return;
            foreach (var item in _items)
            {
                var tempItem = item as TemporaryItem;
                if (tempItem != null) _temporaryItems.Add(tempItem);
            }
        }


        //to be updated.....i don't want the obstacles using controls either
        //there shouldn't be a single goddamn control in this game except what i'm using as a canvas
        //unrelated reminder to myself...
        //dont forget to add the player x0y0 reinit code
        public void LoadObstacles()
        {
            var obstacleFactory = new ObstacleFactory();
            _obstacles = new List<Obstacle>();
            _movingObstacles = new List<MovingObstacle>();

            var obstacles = obstacleFactory.LoadObstacles(_roomName);
            foreach (var obstacle in obstacles)
            {
                if (obstacle.IsInitialized)
                {
                    if (obstacle is MovingObstacle) _movingObstacles.Add((MovingObstacle)obstacle);
                    else _obstacles.Add(obstacle);
                }
            }
        }

        #endregion

        #region Pause Handling
        //call child suspend methods, remove all active keys.
        //oh yeah you know we want to have that still at the room level i think
        //because I don't want to ignore them completely.
        public void Suspend()
        {
            //ignore the request on transition rooms
            //i mean this is definitely clunky and why i should have a separate class maybe
            //for transitions
            //but i'll figure that out later 
            if (!_interactive) return;
            _isSuspended = true;
            ActiveKeys.Clear();
            Player.Suspend();
            foreach (var item in _items)
            {
                item.IsSuspended = true;
            }
        }

        public void Resume()
        {
            if (!_interactive) return;
            _isSuspended = false;
            Player.Resume();
            foreach (var item in _items)
            {
                item.IsSuspended = false;
            }
        }
        #endregion

        #region Event Handling
        public override void Update()
        {
            if (!_interactive)
            {
                //transition room
                if (!_isSuspended) //i suppose i should disable menus and pausing on transition screens
                    //but that's a problem for future ej
                    TotalTicks++;
                return;
            }
            if (!_isSuspended)
            {
                Player.Update();
                StatsDisplay.Update();
                PerformItemOperations();
                //should we check afterwards to make sure we didn't miss anything marked for deletion?
                //or do it as we cycle through?
                //idk if any solution is really perfect here
                CleanUpItems();
                UpdateStationaryObstacles();
                UpdateMovingObstacles();
                TotalTicks++; //I only want to update this when the room is actually active
                              //in case we use it for a level timer or something

                Draw();
            }
        }

        Image bgimg = null;

        public void Draw()
        {
            if (bgimg == null) bgimg = (Image)((Control as PictureBox).Image.Clone());

            Bitmap bg = new Bitmap(bgimg);
            Bitmap playersource = new Bitmap(Player.Sprite.CurrentImage);

            using (Graphics g = Graphics.FromImage(bg))
            {
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

                //hack but we need to revisit and make more flexible
                var itemsAsStats = new List<GenericItem>();
                //we need to clean this up
                foreach (var item in _items)
                {
                    var leaf = (item as Leaf);
                    if (leaf != null)
                    {
                        Bitmap itemsource = new Bitmap(leaf.CurrentImage);
                        g.DrawImage(itemsource, leaf.X, leaf.Y);
                    }
                    var pinecone = (item as PineCone);
                    if (pinecone != null)
                    {
                        //Bitmap itemsource = new Bitmap(pinecone.CurrentImage);
                        if (pinecone.DisplayingAsStat)
                        {
                            //draw later on top of everything else
                            itemsAsStats.Add(pinecone);
                            //Bitmap itemsource = new Bitmap(pinecone.CurrentImage);
                            // g.DrawImage(itemsource, pinecone.CountdownDisplayX - pinecone.W, pinecone.CountdownDisplayY);
                            //g.DrawString(pinecone.CountdownDisplayText, new Font("Tahoma", 8), Brushes.Black, new Rectangle(pinecone.CountdownDisplayX, pinecone.CountdownDisplayY, pinecone.CountdownDisplayWidth, pinecone.CountdownDisplayHeight));
                        }
                        else
                        {
                            Bitmap itemsource = new Bitmap(pinecone.CurrentImage);
                            g.DrawImage(itemsource, pinecone.X, pinecone.Y);
                        }//shouldn't have named this stuff countdown it might not always be a countdown
                    }
                }
               
                //draw the player on top of objects to use and behind obstacles
                g.DrawImage(playersource, Player.X, Player.Y);
                
                foreach (var obstacle in _obstacles)
                {
                    g.DrawImage(new Bitmap(obstacle.CurrentImage), obstacle.X, obstacle.Y);
                }
                foreach (var movingobstacle in _movingObstacles)
                {
                    g.DrawImage(new Bitmap(movingobstacle.CurrentImage), movingobstacle.X, movingobstacle.Y);
                }

                //if (StatsImage != null)
                  //  g.DrawImage(new Bitmap(StatsImage),StatsDisplay.X, StatsDisplay.Y, StatsDisplay.W, StatsDisplay.H);
                //wow this is awful. need to fix these coordinates
                //g.DrawString(StatsDisplay.Text, new Font("Tahoma", 8), Brushes.Black, StatsDisplay.X + StatsDisplay.W/2 - StatsDisplay.MarginX, StatsDisplay.Y + StatsDisplay.H/2 - StatsDisplay.MarginY);

                if (RainbowBar != null && RainbowBarOverlay != null)
                {
                    //fix this to not be hard coded later
                    //may also want to only do the calculation when the points have changed.
                    //and I want to freeze for a sec and show the full rainbow bar maybe
                    //and when we take a hit i want to show the bar breaking
                    //you have to heal the bar before you can store points
                    double percentage = (double)Player.RainbowPoints / 100.0;
                    int widthtodraw = (int)(percentage * (double)RainbowBar.Width);
                    if (widthtodraw > 0)
                    {
                        Bitmap cropImage(Bitmap img)
                        {
                            Rectangle CropArea = new Rectangle(0, 0, widthtodraw, img.Height);
                            Bitmap bmpCrop = img.Clone(CropArea, img.PixelFormat);
                            return bmpCrop;
                        }

                        var cropped = cropImage(new Bitmap(RainbowBar));
                        g.DrawImage(cropped, 5, 10, cropped.Width, RainbowBar.Height / 2);
                    }
                    //g.DrawImage(new Bitmap(RainbowBar), 5, 10, widthtodraw, RainbowBar.Height);
                    g.DrawImage(new Bitmap(RainbowBarOverlay), 5, 10, RainbowBarOverlay.Width, RainbowBarOverlay.Height/2);
                }
                foreach (var v in Player.PointVisualizers)
                {
                    g.DrawString(v.Text, new Font("Tahoma", 8), v.Gain ? Brushes.Green : Brushes.Red, new Rectangle(v.X, v.Y, v.W, v.H));
                }

                //what I'm gathering here is we need to make this part of the overall class for this
                foreach (var stat in itemsAsStats)
                {
                    var pinecone = stat as PineCone;
                    if (pinecone != null)
                    {
                        if (StatsImage != null)
                            g.DrawImage(new Bitmap(StatsImage), pinecone.CountdownDisplayX - pinecone.W - 10, /*i want it lined up with the other thing*/StatsDisplay.Y, StatsDisplay.W, 45);
                        g.DrawImage(new Bitmap(pinecone.CurrentImage), pinecone.CountdownDisplayX - pinecone.W, StatsDisplay.Y + 10/*pinecone.CountdownDisplayY*/);
                        g.DrawString(pinecone.CountdownDisplayText, new Font("Tahoma", 8), Brushes.Black, new Rectangle(pinecone.CountdownDisplayX, StatsDisplay.Y + 10/*pinecone.CountdownDisplayY*/, pinecone.CountdownDisplayWidth, pinecone.CountdownDisplayHeight));
                    }
                }
            }

            //System.IO.MemoryStream surewhynot = new System.IO.MemoryStream();
            //bg.Save(surewhynot, ImageFormat.Png);
            //var test = Image.FromStream(surewhynot);
            (Control as PictureBox).Image = UtilityMethods.ImageFromBitmap(bg);//test;
        }

        public override void OnKeyPress(KeyEventArgs e)
        {
            if (_isSuspended) return;
            if (!_interactive)
            {
                //do we have a continue key specified?
                if (ContinueKey != Keys.None
                    && e.KeyCode == ContinueKey)
                {
                    Continue = true;
                }
                return;
            }
            Player.OnKeyPress(e);
            StatsDisplay.OnKeyPress(e);
            if (!ActiveKeys.Contains(e.KeyCode))
                ActiveKeys.Add(e.KeyCode);
        }

        public override void OnKeyUp(KeyEventArgs e)
        {
            if (_isSuspended || !_interactive) return;
            Player.OnKeyUp(e);
            StatsDisplay.OnKeyUp(e);
            ActiveKeys.Remove(e.KeyCode);
        }
        #endregion

        #region Motion And Collision Handling
        protected void UpdateStationaryObstacles()
        {
            CheckObstacleCollisions();
        }

        protected void UpdateMovingObstacles()
        {
            foreach (var obstacle in _movingObstacles)
            {
                obstacle.Update();
                if (obstacle.TileIndex == Player.TileIndex)
                {
                    Player.Rebound(obstacle);
                }
            }
        }

        protected void CheckObstacleCollisions()
        {
            foreach (var obstacle in _obstacles)
            {
                if (TileObjectPlayerCollision(obstacle))
                {
                    ResolvePlayerCollision(obstacle);
                }

                foreach (var movingObstacle in _movingObstacles)
                {
                    if (movingObstacle.TileIndex == obstacle.TileIndex)
                    {
                        ResolveCollision(obstacle, movingObstacle);
                    }
                }

                //this should probably go in the regular update code idk.
                obstacle.Update();
            }
        }

        //probably need a version of this for things that aren't the player but that's a problem for future ej
        //should this go in the obstacle code actually? idk.
        //i probably should make this and the above more generic so you can use them besides with the player. 
        protected void ResolvePlayerCollision(Obstacle obstacle)
        {
            Player.Rebound(obstacle);
            obstacle.Active = true;
        }

        protected void ResolveCollision(Obstacle obstacle, MovingObstacle movingObstacle)
        {
            movingObstacle.Rebound(obstacle);
        }
        #endregion

        #region Item Handling
        protected void UpdateTemporaryItems()
        {
            //take any that are active and put them into the active items
            _activeItems = _temporaryItems.Where(x => x != null && x.IsApplied).ToList();

            //make sure they're drawing in the right spot
            foreach (var item in _activeItems)
            {
                item.ShowAsStat();
            }
        }

        protected bool IsItemActive(GenericItem i)
        {
            return i.Active || (TileObjectPlayerCollision(i) && ItemActiveKeyPressed(i));
        }

        protected void CleanUpItems()
        {
            var items = _items.Where(i => i.MarkedForDeletion);
            foreach (var item in items)
            {
                item.Active = false;
                item.Cleanup();
            }
            _items.RemoveAll(i => i.MarkedForDeletion);
        }

        //the room SHOULD know about the player right
        protected void PerformItemOperations()
        {
            //look at the items in the room
            //note, we may want to rewrite this to stop as soon as we find an active item
            //we we want to make use of the fact that an obstacle and an item won't be in the same tile,
            //so if we find an item, we can short circuit and not check for a wall/obstacle.
            var activeItems = _items.Where(i => i != null && IsItemActive(i));
            //then execute the operation

            foreach (var i in activeItems)
            {
                //in case the item should be active but isn't yet.
                i.Active = true;
                i.Update();
            }

            UpdateTemporaryItems();
        }
        #endregion
    }
}
