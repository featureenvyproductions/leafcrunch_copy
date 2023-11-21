using LeafCrunch.GameObjects.Items.ItemOperations;
using LeafCrunch.Utilities.Entities;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects.Items
{
    public class GenericItem : InteractiveGameObject
    {
        private Keys _activationKey = Keys.None; //if we keep this as none, it means just being on the same tile will activate it
        private bool _isSuspended = false; //different from active...this is just for pausing

        private bool _isInitialized = false;

        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }

        public Operation Operation { get; set; }
        public bool Active { get; set; }
        public bool MarkedForDeletion { get; set; }

        public bool IsSuspended
        { 
            get { return _isSuspended; } 
            set { _isSuspended = value; } 
        }

        public Keys ActivationKey
        {
            get { return _activationKey; }
            set { _activationKey = value; }
        }

        public GenericItem(): base()
        {
            if (!GenericGameObjectRegistry.RegisteredObjects.ContainsValue(this))
                GenericGameObjectRegistry.RegisteredObjects.Add("GenericItem_" + GetHashCode().ToString(), this);
        }

        public GenericItem(Control control) : base(control)
        {
            if (!GenericGameObjectRegistry.RegisteredObjects.ContainsValue(this))
                GenericGameObjectRegistry.RegisteredObjects.Add("GenericItem_" + GetHashCode().ToString(), this);
            //idk man I got myself into constructor hell IDK if we want this to be a case or not
            Active = false;
            // Operation = null;
            MarkedForDeletion = false;
        }

        public GenericItem(Control control, Operation operation)
            : base(control)
        {
            if (!GenericGameObjectRegistry.RegisteredObjects.ContainsValue(this))
                GenericGameObjectRegistry.RegisteredObjects.Add("GenericItem_" + GetHashCode().ToString(), this);
            Active = false;
            Operation = operation;
            MarkedForDeletion = false;
        }

        public void Cleanup()
        {
            var parent = Control.Parent;
            parent.Controls.Remove(Control); //this is dumb I should figure out a better way to do this
        }

        new virtual public void Update()
        {
            if (_isSuspended || !Active || Operation == null) return;

            //tbr....maybe we can do something with the result but I think for now we'll just have this generic handle thing
            HandleResult(Operation.Execute());
        }

        virtual protected void HandleResult(Result result) { }

        //when an operation name is provided
        virtual protected void InitializeOperationFromRegistry(string operationName)
        {
            var operation = OperationRegistry.Operations[operationName];

            Operation = new Operation()
            {
                OperationName = operationName,
                Params = ConvertParamList(operation.ParamData),
                ParamData = operation.ParamData,
                TargetName = operation.TargetName,
                Target = null,
                ToExecute = null,
                ToExecuteName = operation.ToExecuteName
            };
        }

        public virtual void Refresh()
        {
            InitializeOperationFromRegistry();
        }

        //once we set the name we can refresh as needed
        //necessary if anything is a property reference
        virtual protected void InitializeOperationFromRegistry()
        {
            var operation = OperationRegistry.Operations[Operation.OperationName];

            Operation = new Operation()
            {
                OperationName = Operation.OperationName,
                Params = ConvertParamList(operation.ParamData),
                ParamData = operation.ParamData,
                TargetName = operation.TargetName,
                Target = null,
                ToExecute = null,
                ToExecuteName = operation.ToExecuteName
            };
        }

        virtual protected void InitializeMultiOperationFromRegistry(string operationName)
        {
            var operation = OperationRegistry.Operations[operationName] as MultiTargetOperation;

            //can probably consolidate this stuff
            //but i'll do that later.
            //also need to handle if that cast fails...tbd
            var paramList = operation.ParamData != null ? ConvertParamList(operation.ParamData) : null;

            Operation = new MultiTargetOperation()
            {
                OperationName = operationName,
                Params = paramList,
                ParamData = operation.ParamData,
                TargetName = null,
                Target = null,
                TargetType = operation.TargetType,
                ToExecute = null,
                ToExecuteName = operation.ToExecuteName
            };
        }

        virtual protected object GetPropertyValue(string propertyName)
        {
            var t = GetType();
            var p = t.GetProperty(propertyName);
            return p?.GetValue(this);
        }

        virtual protected Dictionary<string, object> ConvertParamList(List<ParameterData> paramData)
        {
            var dict = new Dictionary<string, object>();
            foreach (var param in paramData)
            {
                var val = param.Value;
                var type = param.ValueType;
                var name = param.Name;
                switch (type)
                {
                    case "String":
                        dict.Add(name, val);
                        break;
                    case "Integer":
                        {
                            int i;
                            int.TryParse(val, out i); //check what this is if conversion fails...tbd
                            dict.Add(name, i);
                            break;
                        }
                    case "Property":
                        {
                            dict.Add(name, GetPropertyValue(val));
                        }
                        break;
                }
            }
            return dict;
        }
    }
}
