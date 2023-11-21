using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.Utilities;
using LeafCrunch.Utilities.Entities;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items.Obstacles
{
    public class MovingObstacle : Obstacle, IReboundable, ICollidable
    {
        private bool _isSuspended = false; //I don't think I need this because the room handles it but eh

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

        public MovingObstacle(ObstacleData obstacleData) : base(obstacleData)
        {
            IsInitialized = false;
            Speed = new Speed()
            {
                vx = obstacleData.InitialSpeedX,
                vy = obstacleData.InitialSpeedY
            };

            IsInitialized = true;
        }

        virtual public void Rebound(ICollidable collidable)
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

        override public void Update()
        {
            if (_isSuspended) return;
            UpdateLocation();
        }

        //revisit
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

        private void ResolveCollision(Control control)
        {
            if (Speed.vx != 0) //if we aren't moving in this direction, there's nothing to resolve
            {
                while (CollisionX(control.Left))
                {
                    Control.Left += Speed.vx;
                }
            }
            if (Speed.vy != 0)
            {
                while (CollisionY(control.Top))
                {
                    Control.Top += Speed.vy;
                }
            }
        }

        private void Rebound(Player player)
        {
            //if the player isn't moving in a direction, just bounce off them
            Speed.vy = player.Speed.vy > 0 ? player.Speed.vy : Speed.vy * -1;
            Speed.vx = player.Speed.vx > 0 ? player.Speed.vx : Speed.vx * -1;

            ResolveCollision(player.Control);
        }

        private void Rebound (Obstacle obstacle)
        {
            Speed.vy *= -1;
            Speed.vx *= -1;

            ResolveCollision(obstacle.Control);
        }
    }
}
