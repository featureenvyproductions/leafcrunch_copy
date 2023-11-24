using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.Utilities;
using LeafCrunch.Utilities.Entities;
using System.Drawing;

namespace LeafCrunch.GameObjects.Items.Obstacles
{
    //tile system doesn't work great but it's fast for seeing if we're like on top of the control
    //this can do more precise checking
    //for a future improvement we can also check a small group of tiles or like a bigger tile/region
    public class Obstacle : GenericItem, ICollidable
    {
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
        override public Image CurrentImage 
        { 
            get
            { 
                return _currentImage; 
            }
        }

        public bool CollisionX(int x)
        {
            return (x >= X && x <= X + W);
        }

        public bool CollisionY(int y)
        {
            return (y <= Y && y >= Y + H);
        }
    }
}
