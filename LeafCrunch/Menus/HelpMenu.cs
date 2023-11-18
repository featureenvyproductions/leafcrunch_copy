using System.Windows.Forms;

namespace LeafCrunch.Menus
{
    //this isn't really interactive
    //it just displays some helpful tips on how to play the game
    public class HelpMenu : GenericMenu
    {
        public HelpMenu() : base()
        {
            ActivationKey = Keys.F1;
        }

    }
}
