using System.Collections.Generic;

namespace LeafCrunch.GameObjects.Items.ItemOperations
{
    //does a thing to a list of targets
    public class MultiTargetOperation : Operation
    {
        private List<GenericGameObject> _targets = new List<GenericGameObject>();
        public List<GenericGameObject> Targets
        {
            get { return _targets; }
            set { _targets = value; }
        }
        new public Result Execute()
        {
            if (ToExecute == null) return null;

            List<Result> results = new List<Result>();
            foreach (GenericGameObject target in Targets)
            {
                if (target != null)
                {
                    results.Add(new Result()
                    {
                        Value = ToExecute(target, Params)
                    });
                }
            }
            return new Result()
            {
                Value = results
            };
        }
    }
}
