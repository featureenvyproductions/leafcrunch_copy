using System.Drawing;
using System.IO;
using System.Reflection;

namespace LeafCrunch.Utilities
{
    public class UtilityMethods
    {
        public static Image ImageFromPath(string relPath)
        {
            return Image.FromFile(Path.Combine(
                            Path.GetDirectoryName(
                                Assembly.GetExecutingAssembly().Location)
                            , relPath));
        }
    }
}
