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
    public class Obstacle : GenericItem, ICollidable
    {
        public Obstacle(Control control) : base(control)
        {
        }
        public Obstacle(ObstacleData obstacleData) : base()
        {
            /*var img =*/
            CurrentImage = UtilityMethods.ImageFromPath(obstacleData.SingleImage);
            Control = new PictureBox()
            {
                Left = obstacleData.X,
                Top = obstacleData.Y,
              //  Image = img,
                Width = CurrentImage.Width,
                Height = CurrentImage.Height,
                BackColor = System.Drawing.Color.Transparent
            };
            IsInitialized = true;
        }
        public Image CurrentImage { get; set; }
        //checks if x is within the bounds of the control
        public bool CollisionX(int x)
        {
            return (x >= Control.Left && x <= Control.Right);
        }

        public bool CollisionY(int y)
        {
            return (y <= Control.Top && y >= Control.Bottom);
        }
    }
}
