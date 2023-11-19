using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafCrunch.GameObjects.ItemProperties
{
    //a thing that can use items
    //mostly just the player
    public interface IItemUser
    {
        void ApplyItem(object args);
    }
}
