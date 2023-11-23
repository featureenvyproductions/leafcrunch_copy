using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Stats
{
    //show the player's stats i guess....updates the status display controls based on the player's info
    //right now this is just a label, but eventually we will probably want to have it
    //implement IDrawable and have a method that populates the current image and draws a bunch of
    //stats related things to it
    //that's a problem for future ej
    public class StatsDisplay : GenericGameObject
    {
        private Player _player { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get { return 150; } }
        public int H { get { return 20; } }
        public string Text { get; set; }

        public StatsDisplay(Player player): base()
        {
            _player = player;
            X = 50;
            Y = 30;
            Text = "";
        }

        public StatsDisplay(Control control) : base(control)
        {
        }

        public StatsDisplay(Control pointsDisplay, Player player) : base(pointsDisplay)
        {
            _player = player;
        }

        public override void Update()
        {
            Text = _player.RainbowPoints.ToString();
        }
    }
}
