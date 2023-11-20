using System.Collections.Generic;
using System.Drawing;

namespace LeafCrunch.Utilities.Animation
{
    public class AnimatedSprite
    {
        public Dictionary<Direction, ImageSequence> StaticImage { get; set; }
        public Dictionary<Direction, ImageSequence> Animation { get; set; }

        private ImageSequence _currentAnimation = null;

        public Image CurrentImage
        {
            get
            {
                return _currentAnimation.CurrentImage;
            }
        }

        public AnimatedSprite(Dictionary<Direction, ImageSequence> staticImages,
            Dictionary<Direction, ImageSequence> animations)
        {
            StaticImage = staticImages;
            Animation = animations;
            //we could check these to make sure they're valid and whatever
            //but like if you're implementing the animation interface it's bc you have an image
            //so why wouldn't you have at least the absolute bare minimum default image
            _currentAnimation = StaticImage[Direction.None];
        }

        //right now i'll just mostly have walking/standing animations but i could make special ones eventually

        //whoever calls this should have calculated which animation we need to display
        //and whether it's static/we're moving
        //this should just check whether things changed and reset the frame count and index if so.
        public void UpdateSequence(Direction direction, bool staticImage)
        {
            ImageSequence targetAnimation;
            if (staticImage)
            {
                targetAnimation = StaticImage[direction];
            }
            else
            {
                targetAnimation = Animation[direction];
            }

            if (_currentAnimation.Equals(targetAnimation))
            {
                //no changes are needed
                //update the frame
                _currentAnimation.UpdateFrame();
            }
            else
            {
                //replace the current animation
                _currentAnimation = targetAnimation;
                _currentAnimation.ResetAnimation();
            }
        }
    }
}
