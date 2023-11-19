using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items.Obstacles
{
    public class MovingObstacle : Obstacle, IReboundable, ICollidable
    {
        //simple and dumb
        //we have a speed and we go that speed until we hit a thing
        //then we go the opposite way at the same speed until we hit a thing, etc

        public MovingObstacle(Control control) : base(control)
        {
            //doesn't have to be these this is for testing
            Speed = new Speed()
            {
                vx = 5,
                vy = 5
            };
        }

        public MovingObstacle(Control control, int speedx, int speedy) : base(control)
        {
            Speed = new Speed()
            {
                vx = speedx,
                vy = speedy
            };
        }

        public void Rebound(ICollidable collidable)
        {
            //can collide with other obstacles or the player
            //with obstacles (like walls) we actually will bounce it off of those
            var obstacle = collidable as Obstacle;
            if (obstacle != null)
            {
                Rebound(obstacle);
                return;
            }

            var player = collidable as Player;
            if (player != null) Rebound(player);
        }

        //could be modified to move in response to the player colliding with it but I think for now this'll just be a placeholder
        private void Rebound(Player player)
        {
            //come back and deal with this later
        }

        private void Rebound (Obstacle obstacle)
        {
            //bounce off a wall or a rock or whatever and go the other direction
        }

        private bool _isSuspended = false;
        override public void Update()
        {
            if (_isSuspended) return;
            UpdateLocation();
        }

        protected void UpdateLocation()
        {
            if (Control == null) return;

            Control.Left += Speed.vx;
            if (Control.Left <= 0)
            {
                while (Control.Left <= 0) Control.Left++;
                Speed.vx *= -1;
            }

            Control.Top += Speed.vy;
            if (Control.Top <= 0)
            {
                while (Control.Top <= 0) Control.Top++;
                Speed.vy *= -1;
            }

            if ((Control.Left + Control.Width) >= GlobalVars.RoomWidth)
            {
                while ((Control.Left + Control.Width) >= GlobalVars.RoomWidth) Control.Left--;
                //bounce
                Speed.vx *= -1;
            }
            if ((Control.Top + Control.Height) >= GlobalVars.RoomHeight)
            {
                while ((Control.Top + Control.Height) >= GlobalVars.RoomHeight) Control.Top--;
                //bounce
                Speed.vy *= -1;
            }
        }
    }
}
