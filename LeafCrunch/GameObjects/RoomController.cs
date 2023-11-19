using LeafCrunch.Utilities;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using LeafCrunch.GameObjects.Items;
using LeafCrunch.GameObjects.Items.InstantItems;
using LeafCrunch.GameObjects.Items.ItemOperations;
using LeafCrunch.GameObjects.Items.TemporaryItems;
using LeafCrunch.GameObjects.Stats;

namespace LeafCrunch.GameObjects
{
    public class RoomController : GenericGameObject
    {
        public bool ActiveRoom = true; //always true right now, idk if we want more rooms in the future

        //I should have a thing where we show +10 or whatever every time we get some points.

        public StatsDisplay StatsDisplay { get; set; }

        //eventually we're going to load control names from a file I think so I won't need this fucking list
        //or we're gonna initialize the controls on the fly with a location and a type
        //like i'll have a prototype and initialize from the prototype
        public RoomController(Control control, Control playerControl, Control statsControl, Control countDownControl, List<Control> itemControls) : base(control) //tbd we need a list of items maybe?
        {
            GlobalVars.RoomWidth = control.Width;
            GlobalVars.RoomHeight = control.Height;
            GlobalVars.RoomTileSizeW = 32; //for now
            GlobalVars.RoomTileSizeH = 32;

            //eventually this will also be where we load custom rooms from some file
            //and there will be an arg here telling us what room file we want
            Load(playerControl, statsControl, countDownControl, itemControls);
        }

        //for now this just loads the test level
        protected void Load(Control playerControl, Control statsControl, Control countDownControl, List<Control> itemControls)
        {
            Player = new Player(playerControl);
            StatsDisplay = new StatsDisplay(statsControl, Player);

            //dumb intermittent hard coded solution till we finish the rest
            Items = new List<GenericItem>()
            {
                new GreenLeaf(itemControls.ElementAt(0), new Operation()
                {
                    Target = Player
                }),
                new YellowLeaf(itemControls.ElementAt(1), new Operation()
                {
                    Target = Player,
                }),
                new OrangeLeaf(itemControls.ElementAt(2), new Operation()
                {
                    Target = Player
                }),
                new RedLeaf(itemControls.ElementAt(3), new Operation()
                {
                    Target = Player
                })
            };

            Items.Add(new PineCone(itemControls.ElementAt(4), new MultiTargetOperation()
            {
                Targets = new List<GenericGameObject>(Items.Cast<GenericItem>())
            }, countDownControl));

            RegisterTemporaryItems();
        }

        //i guess we should know about the player
        public Player Player { get; set; }

        private bool _isSuspended = false; //says whether or not the room is active (as opposed to a menu)

        //call child suspend methods, remove all active keys.
        //oh yeah you know we want to have that still at the room level i think
        //because I don't want to ignore them completely.
        public void Suspend()
        {
            _isSuspended = true;
            ActiveKeys.Clear();
            Player.Suspend();
            foreach (var item in Items)
            {
                item.IsSuspended = true;
            }
        }

        public void Resume()
        {
            _isSuspended = false;
            Player.Resume();
            foreach (var item in Items)
            {
                item.IsSuspended = false;
            }
        }

        public override void Update()
        {
            if (!_isSuspended)
            {
                Player.Update();
                StatsDisplay.Update();
                PerformItemOperations();
                //should we check afterwards to make sure we didn't miss anything marked for deletion?
                //or do it as we cycle through?
                //idk if any solution is really perfect here
                CleanUpItems();
            }
        }

        public List<GenericItem> Items = new List<GenericItem>();
        protected List<TemporaryItem> TemporaryItems = new List<TemporaryItem>(); //items that can be applied

        public void RegisterTemporaryItems()
        {
            if (Items == null) return;
            foreach (var item in Items)
            {
                var tempItem = item as TemporaryItem;
                if (tempItem != null) TemporaryItems.Add(tempItem);
            }
        }

        protected List<TemporaryItem> ActiveItems = new List<TemporaryItem>();
        public void UpdateTemporaryItems()
        {
            //take any that are active and put them into the active items
            ActiveItems = TemporaryItems.Where(x => x != null && x.IsApplied).ToList();
            
            //make sure they're drawing in the right spot
            foreach (var item in ActiveItems)
            {
                item.ShowAsStat();
            }
        }

        protected bool ItemActiveKeyPressed(GenericItem i) => i.ActivationKey == Keys.None || ActiveKeys.Contains(i.ActivationKey);

        //same idea can apply eventually to wall collisions.
        protected bool ItemTileActive(GenericItem i) => i.TileIndex == Player.TileIndex;

        protected bool IsItemActive(GenericItem i)
        {
            return i.Active || (ItemTileActive(i) && ItemActiveKeyPressed(i));
        }

        protected void CleanUpItems()
        {
            var items = Items.Where(i => i.MarkedForDeletion);
            foreach (var item in items)
            {
                item.Active = false;
                item.Cleanup();
            }
            Items.RemoveAll(i => i.MarkedForDeletion);
        }

        //the room SHOULD know about the player right
        public void PerformItemOperations()
        {
            //look at the items in the room
            //note, we may want to rewrite this to stop as soon as we find an active item
            //we we want to make use of the fact that an obstacle and an item won't be in the same tile,
            //so if we find an item, we can short circuit and not check for a wall/obstacle.
            var activeItems = Items.Where(i => i != null && IsItemActive(i));
            //then execute the operation

            foreach (var i in activeItems)
            {
                //in case the item should be active but isn't yet.
                i.Active = true;
                i.Update();
            }

            UpdateTemporaryItems();
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
            Player.OnKeyPress(e);
            StatsDisplay.OnKeyPress(e);
            if (!ActiveKeys.Contains(e.KeyCode))
                ActiveKeys.Add(e.KeyCode);
        }

        public override void OnKeyUp(KeyEventArgs e)
        {
            if (_isSuspended) return;
            Player.OnKeyUp(e);
            StatsDisplay.OnKeyUp(e);
            ActiveKeys.Remove(e.KeyCode);
        }

    }
}
