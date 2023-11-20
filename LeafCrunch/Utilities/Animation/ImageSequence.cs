using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LeafCrunch.Utilities.Animation
{
    public class ImageSequence
    {
        private int _currentIndex = 0;
        private int _frameCount = 0; //so we don't have to count the animation frames every single time

        //treat as static if we only have one image in list...this is more for consolidating code
        public List<Image> Images = new List<Image>();

        //the image that's currently visible
        public Image CurrentImage { get; set; }

        public ImageSequence(List<Image> images)
        {
            Images = images;
            CurrentImage = Images.FirstOrDefault();
            _frameCount = Images.Count;
        }

        public void ResetAnimation()
        {
            _currentIndex = 0;
            CurrentImage = Images.ElementAt(0);
        }

        public void UpdateFrame()
        {
            if (_frameCount > 1)
            {
                if (_currentIndex + 1 == _frameCount) _currentIndex = 0;
                else ++_currentIndex;
                CurrentImage = Images.ElementAt(_currentIndex);
            }
            //if it is static we don't do anything bc the displayed image never changes
        }
    }
}
