using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items.Obstacles
{
    //an object where if the player tile = the obstacle tile
    //we won't allow the player to move in the direction of the obstacle.
    //i'm just gonna make this an item to make coding the collision easier
    public class Obstacle : GenericItem
    {
        public Obstacle(Control control) : base(control)
        {
        }

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
