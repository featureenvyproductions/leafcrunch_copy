using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.Utilities;
using LeafCrunch.Utilities.Entities;

namespace LeafCrunch.GameObjects.Items.Obstacles
{
    public class MovingObstacle : Obstacle, IReboundable, ICollidable
    {
        private bool _isSuspended = false; //I don't think I need this because the room handles it but eh

        //simple and dumb
        //we have a speed and we go that speed until we hit a thing
        //then we go the opposite way at the same speed until we hit a thing, etc

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
            X += Speed.vx;
            if (X <= 0)
            {
                while (X <= 0) X++;
                Speed.vx *= -1;
            }

            Y += Speed.vy;
            if (Y <= GlobalVars.RoomTopMargin)
            {
                while (Y <= GlobalVars.RoomTopMargin) Y++;
                Speed.vy *= -1;
            }

            if ((X + W) >= GlobalVars.RoomWidth)
            {
                while ((X + W) >= GlobalVars.RoomWidth) X--;
                //bounce
                Speed.vx *= -1;
            }
            if ((Y + H) >= GlobalVars.RoomHeight)
            {
                while ((Y + H) >= GlobalVars.RoomHeight) Y--;
                //bounce
                Speed.vy *= -1;
            }
        }

        private void ResolveCollisionWithPlayer(Player player)
        {
            if (Speed.vx != 0) //if we aren't moving in this direction, there's nothing to resolve
            {
                while (CollisionX(player.X))
                {
                    X += Speed.vx;
                }
            }
            if (Speed.vy != 0)
            {
                while (CollisionY(player.Y))
                {
                    Y += Speed.vy;
                }
            }
        }

        private void ResolveCollisionWithObstacle(Obstacle obstacle)
        {
            if (Speed.vx != 0) //if we aren't moving in this direction, there's nothing to resolve
            {
                while (CollisionX(obstacle.X))
                {
                    X += Speed.vx;
                }
            }
            if (Speed.vy != 0)
            {
                while (CollisionY(obstacle.Y))
                {
                    Y += Speed.vy;
                }
            }
        }

        //this will be less messy when all the x y stuff is in the base class
        private void ResolveCollision(InteractiveGameObject obj)
        {
            var player = obj as Player;
            if (player != null)
            { 
                ResolveCollisionWithPlayer(player);
                return;
            }
            var obstacle = obj as Obstacle;
            if (obstacle != null) ResolveCollisionWithObstacle(obstacle);
        }

        private void Rebound(Player player)
        {
            //if the player isn't moving in a direction, just bounce off them
            Speed.vy = player.Speed.vy > 0 ? player.Speed.vy : Speed.vy * -1;
            Speed.vx = player.Speed.vx > 0 ? player.Speed.vx : Speed.vx * -1;

            //could probably simplify
            ResolveCollision(player);
        }

        private void Rebound (Obstacle obstacle)
        {
            Speed.vy *= -1;
            Speed.vx *= -1;

            ResolveCollision(obstacle);
        }
    }
}
