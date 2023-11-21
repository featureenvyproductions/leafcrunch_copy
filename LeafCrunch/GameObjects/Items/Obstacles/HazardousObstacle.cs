using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.GameObjects.Items.ItemOperations;
using LeafCrunch.Utilities.Entities;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items.Obstacles
{
    public class HazardousObstacle : Obstacle, IHazard
    {
        public HazardousObstacle(Control control) : base(control)
        {
        }

        public HazardousObstacle(Control control, Operation operation) : base(control)
        {
            Operation = operation;
            Operation.ToExecute = InflictDamage;
        }

        public HazardousObstacle(Control control, string operationName) : base(control)
        {
            if (!OperationMethodRegistry.TargetOperations.ContainsKey("Items.Obstacles.HazardousObstacle.InflictDamage"))
                OperationMethodRegistry.TargetOperations.Add("Items.Obstacles.HazardousObstacle.InflictDamage", InflictDamage);

     //       ActivationKey = Keys.Enter;

            InitializeOperationFromRegistry(operationName);
        }

        public HazardousObstacle(ObstacleData obstacleData) : base(obstacleData)
        {
            //we're not initialized till this stuff happens
            IsInitialized = false;
            if (!OperationMethodRegistry.TargetOperations.ContainsKey("Items.Obstacles.HazardousObstacle.InflictDamage"))
                OperationMethodRegistry.TargetOperations.Add("Items.Obstacles.HazardousObstacle.InflictDamage", InflictDamage);

       //     ActivationKey = Keys.Enter;//why did i have an activation key here there's no activation for this...

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
