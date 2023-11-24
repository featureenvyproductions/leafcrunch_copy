using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.GameObjects.Items.ItemOperations;
using LeafCrunch.Utilities.Entities;

namespace LeafCrunch.GameObjects.Items.Obstacles
{
    //moves but can also inflict damage - so like reverse of items

    //i want to make a version of this that can only collide with the form edges and the player so i can like
    //trap the player in a maze with hazards
    public class HazardousMovingObstacle : MovingObstacle, IHazard
    {
        public HazardousMovingObstacle(ObstacleData obstacleData) : base(obstacleData)
        {
            IsInitialized = false;
            if (!OperationMethodRegistry.TargetOperations.ContainsKey("Items.Obstacles.HazardousMovingObstacle.InflictDamage"))
                OperationMethodRegistry.TargetOperations.Add("Items.Obstacles.HazardousMovingObstacle.InflictDamage", InflictDamage);

            InitializeOperationFromRegistry(obstacleData.OperationName);
            IsInitialized = true;
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
