using LeafCrunch.Utilities;
using LeafCrunch.Utilities.Animation;

namespace LeafCrunch.GameObjects.ItemProperties
{
    //skeleton for things with animations
    //things without this just use whatever image they're loaded with
    public interface IAnimated
    {
        //how many ticks have gone by since the last frame change
        int AnimationTicks { get; set; }

        Speed PreviousSpeed { get; set; }

        Direction PreviousDirection { get; set; }

        Direction Direction { get; }

        AnimatedSprite Sprite { get; set; }

        Direction GetCurrentDirection();

        void UpdateAnimation();
    }
}
