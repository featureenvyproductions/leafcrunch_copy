using LeafCrunch.Utilities;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using LeafCrunch.GameObjects.Items;

namespace LeafCrunch.GameObjects
{
    public class Room : GenericGameObject
    {
        public Room(Control control) : base(control) //tbd we need a list of items maybe?
        {
            GlobalVars.RoomWidth = control.Width;
            GlobalVars.RoomHeight = control.Height;
            GlobalVars.RoomTileSizeW = 32; //for now
            GlobalVars.RoomTileSizeH = 32;
        }

        //i guess we should know about the player
        public Player Player { get; set; }

        public override void Update()
        {
            PerformItemOperations();
            //should we check afterwards to make sure we didn't miss anything marked for deletion?
            //or do it as we cycle through?
            //idk if any solution is really perfect here
            CleanUpItems();
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
            if (!ActiveKeys.Contains(e.KeyCode))
                ActiveKeys.Add(e.KeyCode);
        }

        public override void OnKeyUp(KeyEventArgs e)
        {
            ActiveKeys.Remove(e.KeyCode);
        }

    }
}
