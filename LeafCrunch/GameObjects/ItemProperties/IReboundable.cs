using LeafCrunch.GameObjects.Items.Obstacles;

namespace LeafCrunch.GameObjects.ItemProperties
{

    public interface IReboundable
    {
        void Rebound(ICollidable obstacle);
    }

}
