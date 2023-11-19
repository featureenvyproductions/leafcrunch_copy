using System.Windows.Forms;

namespace LeafCrunch.GameObjects.ItemProperties
{
    //draws the points somewhere when they change or whatever 
    public class PointVisualizer
    {
        private int _displayTicks = 10;
        private Label _control { get; set; }
        private Control _parent { get; set; }
        private Timer _timer = new Timer();
        private int _currentTicks = 0;

        public bool Active
        {
            get
            {
                return _timer.Enabled;
            }
        }

        public PointVisualizer(Control parent, int points)
        {
            _parent = parent;
            _control = new Label();
            var sign = points > 0 ? "+" : string.Empty;
            _control.Text = $"{sign}{points}";
            _control.Parent = parent;
            _parent.Controls.Add(_control);
            _control.Visible = true;
            _control.ForeColor = System.Drawing.Color.Black;
            _control.BackColor = System.Drawing.Color.Transparent;

            //uhg this all still makes it disappear when it goes off the person :<
            //i'll have to figure that out....

            _control.BringToFront();
            //Control.Left = parent.Left;
            //Control.Top = parent.Top - 32;
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            if (++_currentTicks == _displayTicks)
            {
                _control.Visible = false;
                _parent.Controls.Remove(_control);
                _timer.Stop();
            }
            else
            {
                //animate it because that's neat
                _control.Top--;
                _control.Left--;
            }
        }
    }
}
