using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.Utilities;
using LeafCrunch.Utilities.Entities;
using System.Drawing;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items.Obstacles
{
    //tile system doesn't work great but it's fast for seeing if we're like on top of the control
    //this can do more precise checking
    //for a future improvement we can also check a small group of tiles or like a bigger tile/region
    public class Obstacle : GenericItem, ICollidable, IDrawable
    {
        public Obstacle(Control control) : base(control)
        {
        }
        public Obstacle(ObstacleData obstacleData) : base()
        {
            _currentImage = UtilityMethods.ImageFromPath(obstacleData.SingleImage);

            X = obstacleData.X;
            Y = obstacleData.Y;
            W = CurrentImage.Width;
            H = CurrentImage.Height;
           
            IsInitialized = true;
        }

        private Image _currentImage;
        public Image CurrentImage { get
            { return _currentImage; }
            }
        //checks if x is within the bounds of the control
        public bool CollisionX(int x)
        {
            return (x >= X && x <= X + W);
        }

        public bool CollisionY(int y)
        {
            return (y <= Y && y >= Y + H);
        }

        public int X { get; set; }
        public int Y { get; set; }

        //should probably put these in a higher level
        public int H { get; set; }
        public int W { get; set; }

        //base class needs to be modified
        override public int TileIndex
        {
            get
            {
                int row = Y / GlobalVars.RoomTileSizeH;
                int tileIndex = X / GlobalVars.RoomTileSizeW; //close enough it doesn't have to be exact
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
