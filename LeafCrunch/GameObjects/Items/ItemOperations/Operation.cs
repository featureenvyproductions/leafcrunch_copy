using System.Collections.Generic;

namespace LeafCrunch.GameObjects.Items.ItemOperations
{
    public class OperationMethodRegistry
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

    //let's actually plan on loading up the operations and referencing them as we go
    //could result in some fuckery if we don't reference the methods right since they're not necessarily static but
    //OOPS I DONT CARE
    //that's a problem for future ej
    public class OperationRegistry
    {
        private static Dictionary<string, Operation> _operations;
        public static Dictionary<string, Operation> Operations
        {
            get
            {
                if (_operations == null)
                    _operations = new Dictionary<string, Operation>();
                return _operations;
            }
            set
            {
                _operations = value;
            }
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

        public string ToExecuteName { get; set; }
        public string TargetName { get; set; }

        public Result Execute()
        {
            if (Target == null)
            {
                if (!string.IsNullOrEmpty(TargetName)
                    && GenericGameObjectRegistry.RegisteredObjects.ContainsKey(TargetName))
                {
                    Target = GenericGameObjectRegistry.RegisteredObjects[TargetName];
                }
                if (Target == null) return null;
            }

            if (ToExecute == null)
            {
                //see if we're trying to get something from the registry
                //if we have it this way then we can load up all the operations and registry names at once
                //bc we won't care about any of the methods until all the objects are initialized
                //and the objects can just reference operation names
                if (!string.IsNullOrEmpty(ToExecuteName)
                    && OperationMethodRegistry.TargetOperations.ContainsKey(ToExecuteName))
                {
                    ToExecute = OperationMethodRegistry.TargetOperations[ToExecuteName];
                }
                if (ToExecute == null) return null;
            }
            return ToExecute(Target, Params);
        }
    }
}
