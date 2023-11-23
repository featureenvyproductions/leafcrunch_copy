using System.Windows.Forms;

namespace LeafCrunch.GameObjects.ItemProperties
{
    //draws the points somewhere when they change or whatever 
    public class PointVisualizer
    {
        private int _displayTicks = 10;
        private Timer _timer = new Timer();
        private int _currentTicks = 0;

        public bool Active
        {
            get
            {
                return _timer.Enabled;
            }
        }

        public bool Gain { get; set; }
        public string Text { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        //shouldn't hardcode this....fix eventually
        public int W { get { return 150; } }
        public int H { get { return 20; } }

        public bool IsVisible { get; set; }

        public PointVisualizer(int points, int x, int y)
        {
            var sign = points > 0 ? "+" : string.Empty;
            Gain = (points > 0);
            Text = $"{sign}{points}";
            X = x;
            Y = y;
            IsVisible = true;
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            if (++_currentTicks == _displayTicks)
            {
                IsVisible = false;
                _timer.Stop();
            }
            else
            {
                //animate it because that's neat
                Y--;
                X--;
            }
        }
    }
}
