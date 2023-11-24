using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.GameObjects.Items.ItemOperations;
using LeafCrunch.Utilities.Entities;

namespace LeafCrunch.GameObjects.Items.Obstacles
{
    public class HazardousObstacle : Obstacle, IHazard
    {
        public HazardousObstacle(ObstacleData obstacleData) : base(obstacleData)
        {
            //we're not initialized till this stuff happens
            IsInitialized = false;
            if (!OperationMethodRegistry.TargetOperations.ContainsKey("Items.Obstacles.HazardousObstacle.InflictDamage"))
                OperationMethodRegistry.TargetOperations.Add("Items.Obstacles.HazardousObstacle.InflictDamage", InflictDamage);

            InitializeOperationFromRegistry(obstacleData.OperationName);

            IsInitialized = true;
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
}
