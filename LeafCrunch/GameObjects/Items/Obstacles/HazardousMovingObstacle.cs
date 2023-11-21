﻿using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.GameObjects.Items.ItemOperations;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items.Obstacles
{
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

        public HazardousMovingObstacle(Control control, int speedx, int speedy, string operationName) : base(control, speedx, speedy)
        {
            if (!OperationMethodRegistry.TargetOperations.ContainsKey("Items.Obstacles.HazardousMovingObstacle.InflictDamage"))
                OperationMethodRegistry.TargetOperations.Add("Items.Obstacles.HazardousMovingObstacle.InflictDamage", InflictDamage);

            InitializeOperationFromRegistry(operationName);
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
}
