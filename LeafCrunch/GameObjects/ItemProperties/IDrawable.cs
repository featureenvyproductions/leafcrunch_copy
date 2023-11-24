using System.Drawing;

namespace LeafCrunch.GameObjects.ItemProperties
{
    public interface IDrawable
    {
        Image CurrentImage { get; }
        int X { get; set; }
        int Y { get; set; }
    }
}
