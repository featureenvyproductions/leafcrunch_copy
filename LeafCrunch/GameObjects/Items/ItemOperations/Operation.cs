using LeafCrunch.Utilities;
using LeafCrunch.Utilities.Entities;
using System.Collections.Generic;
using System.IO;

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

        private const string _configFile = "operations.json";

        private static string LoadJson()
        {
            return File.ReadAllText(UtilityMethods.GetConfigPath(_configFile));
        }

        public static void Load()
        {
            if (_operations == null) _operations = new Dictionary<string, Operation>();

            var jsonString = LoadJson();
            var loader = new JsonLoader();
            var operations = loader.LoadFromJson<OperationDataCollection>(jsonString);
            foreach (var operation in operations.OperationList)
            {
                //clunky but we can tweak it later and handle things like lists of names
                if (!string.IsNullOrEmpty(operation.TargetType))
                {
                    _operations.Add(operation.OperationName, new MultiTargetOperation()
                    {
                        Params = null,
                        ParamData = operation.ParameterList,
                        Target = null,
                        TargetName = null,
                        TargetType = operation.TargetType,
                        ToExecute = null,
                        ToExecuteName = operation.MethodToExecute
                    });
                }
                else
                {
                    _operations.Add(operation.OperationName, new Operation()
                    {
                        Params = null,
                        ParamData = operation.ParameterList,
                        Target = null,
                        TargetName = operation.TargetName,
                        ToExecute = null,
                        ToExecuteName = operation.MethodToExecute
                    });
                }
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
        public string OperationName { get; set; }
        private GenericGameObject _target;
        public GenericGameObject Target 
        { 
            get
            {
                if (_target == null)
                {
                    //find it in the registry
                    if (!string.IsNullOrEmpty(TargetName)
                    && GenericGameObjectRegistry.RegisteredObjects.ContainsKey(TargetName))
                    {
                        _target = GenericGameObjectRegistry.RegisteredObjects[TargetName];
                    }
                }
                //if it's still null whelp we've got bigger problems...
                //again I need to make some validation code
                return _target;
            }
            set { _target = value; }
        }
        public object Params { get; set; } //must be passed but can be null

        public List<ParameterData> ParamData { get; set; } //in case we want to initialize the data later on

        private TargetOperation _toExecute;
        public TargetOperation ToExecute 
        {
            get 
            {
                if (_toExecute == null)
                {
                    //see if we're trying to get something from the registry
                    //if we have it this way then we can load up all the operations and registry names at once
                    //bc we won't care about any of the methods until all the objects are initialized
                    //and the objects can just reference operation names
                    if (!string.IsNullOrEmpty(ToExecuteName)
                        && OperationMethodRegistry.TargetOperations.ContainsKey(ToExecuteName))
                    {
                        _toExecute = OperationMethodRegistry.TargetOperations[ToExecuteName];
                    }
                }
                return _toExecute;
            }
            set
            {
                _toExecute = value;
            }
        }

        public string ToExecuteName { get; set; }
        public string TargetName { get; set; }

        public Result Execute()
        {
            if ((Target == null) || (ToExecute == null)) return null;
            return ToExecute(Target, Params);
        }
    }
}
