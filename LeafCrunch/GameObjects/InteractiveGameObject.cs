using LeafCrunch.Utilities;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects
{
    public class InteractiveGameObject : GenericGameObject
    {
        public Speed Speed { get; set; }

        public InteractiveGameObject(): base()
        {
            Speed = new Speed() { vx = 0, vy = 0 };
        }

        public InteractiveGameObject(Control control) : base(control)
        {
            Speed = new Speed() { vx = 0, vy = 0 };
        }

        //i don't love this because it's so imprecise but it does speed things up
        //to improve
        //to be done...index everything during loading
        //add references to an obstacle index hash table
        //calculate tile of destination pixel
        //look up destination tile in obstacle hash table
        //revisit moving things because they can be tricky...
        //have them do their own collision checking with stationary objects and the player
        //after calculating the index of their own destination pixels
        public int TileIndex
        {
            get
            {
                //determine the general location based on the tile size and where our origin is
                //tiles work like previous row counts + column count
                //like
                //1 2 3 4 5
                //6 7 8 x 10
                //player x is on tile 9
                int row = Control.Top / GlobalVars.RoomTileSizeH;
                int tileIndex = Control.Left / GlobalVars.RoomTileSizeW; //close enough it doesn't have to be exact
                                                                         //if we're past the first row we need to do some addition
                if (row > 0)
                {
                    //get the max row length
                    int maxCols = GlobalVars.RoomWidth / GlobalVars.RoomTileSizeW; //this is fine for our purposes even if decimals get truncated
                    tileIndex += maxCols * row;
                }
                return tileIndex;
            }
        }
    }
}
