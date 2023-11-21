using System.Collections.Generic;

namespace LeafCrunch.GameObjects.Items.ItemOperations
{
    public class OperationRegistry
    {
        //because creating delegates on the fly is a pain in the fucking ass
        //maybe one day.
        private static Dictionary<string, TargetOperation> _targetOperations;
        public static Dictionary<string, TargetOperation> TargetOperations
        {
            get
            {
                if (_targetOperations == null)
                    _targetOperations = new Dictionary<string, TargetOperation>();
                return _targetOperations;
            }
            set { _targetOperations = value; }
        }
    }

    public class Result
    {
        //can be whatever result we need it to be
        public object Value;
    }


    public delegate Result TargetOperation(GenericGameObject target, object paramList);

    //does a thing to a target
    public class Operation : IOperation
    {
        public GenericGameObject Target { get; set; }
        public object Params { get; set; } //must be passed but can be null
        public TargetOperation ToExecute { get; set; }

        public Result Execute()
        {
            if (Target != null && ToExecute != null)
            {
                return ToExecute(Target, Params);
            }
            return null;
        }
    }
}
