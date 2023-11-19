using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.GameObjects.Items.ItemOperations;
using LeafCrunch.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items.Obstacles
{
    //something that can take damage
    public interface IDamageReceptor
    {
        //for the player, damage is point decrements
        //but for anything else if we expand this functionality
        //it might effect speed or maybe destroy something altogether?
        void ApplyDamage(object args);
    }

    //something that can inflict damage
    public interface IHazard
    {
        Result InflictDamage(GenericGameObject genericGameObject, object paramList);
    }

    public class HazardousObstacle: Obstacle, IHazard
    {
        public HazardousObstacle(Control control) : base(control)
        {
        }

        public HazardousObstacle(Control control, Operation operation) : base(control)
        {
            Operation = operation;
            Operation.ToExecute = InflictDamage;
        }

        public override void Update()
        {
            //oh when will we set it to active though
            if (IsSuspended || !Active || Operation == null) return;

            HandleResult(Operation.Execute());
            Active = false;
        }

        virtual protected Result InflictDamage(GenericGameObject genericGameObject, object paramList)
        {
            var victim = genericGameObject as IDamageReceptor;
            if (victim != null) victim.ApplyDamage(paramList);
            return new Result() { Value = true };
        }

        Result IHazard.InflictDamage(GenericGameObject genericGameObject, object paramList)
        {
            return InflictDamage(genericGameObject, paramList);
        }
    }

    //moves but can also inflict damage - so like reverse of items
    public class HazardousMovingObstacle : MovingObstacle, IHazard
    {
        public HazardousMovingObstacle(Control control) : base(control)
        {
        }

        public HazardousMovingObstacle(Control control, int speedx, int speedy, Operation operation) : base(control, speedx, speedy)
        {
            Operation = operation;
            Operation.ToExecute = InflictDamage;
        }

        //rebounds, but also inflicts damage if colliding with target
        override public void Rebound(ICollidable collidable)
        {
            base.Rebound(collidable);

            var obj = collidable as GenericGameObject;
            if (obj != null && Operation.Target.Equals(obj))
            {
                Operation.Execute();
            }
        }

        virtual protected Result InflictDamage(GenericGameObject genericGameObject, object paramList)
        {
            var victim = genericGameObject as IDamageReceptor;
            if (victim != null) victim.ApplyDamage(paramList);
            return new Result() { Value = true };
        }

        Result IHazard.InflictDamage(GenericGameObject genericGameObject, object paramList)
        {
            return InflictDamage(genericGameObject, paramList);
        }
    }

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
