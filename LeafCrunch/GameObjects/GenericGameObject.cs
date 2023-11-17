using System.Windows.Forms;

namespace LeafCrunch.GameObjects
{
    public abstract class GenericGameObject
    {
        public Control Control { get; set; } //the associated forms control

        public GenericGameObject Parent { get; set; }

        public GenericGameObject(Control control)
        {
            Control = control;
        }

        public GenericGameObject(Control control, GenericGameObject parent)
        {
            Control = control;
            Parent = parent;
        }

        public virtual void Update() { }
        public virtual void OnKeyPress(KeyEventArgs e) { }
        public virtual void OnKeyUp(KeyEventArgs e) { }
    }

    //show the player's stats i guess....updates the status display controls based on the player's info
    public class StatsDisplay : GenericGameObject
    {
        private Player _player { get; set; }

        public StatsDisplay(Control control) : base(control)
        {
        }

        //may add other controls as needed besides just points display
        public StatsDisplay(Control pointsDisplay, Player player) : base(pointsDisplay)
        {
            _player = player;
        }

        public override void Update()
        {
            var label = Control as Label;
            if (label != null)
            {
                label.Text = _player.RainbowPoints.ToString();
                label.Refresh(); //do i need this though
            }
        }
    }
}
