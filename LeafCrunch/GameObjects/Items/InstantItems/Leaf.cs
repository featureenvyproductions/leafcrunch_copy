using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.GameObjects.Items.ItemOperations;
using LeafCrunch.Utilities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static LeafCrunch.GameObjects.Items.ItemOperations.Operation;

namespace LeafCrunch.GameObjects.Items.InstantItems
{
    public abstract class Leaf : InstantItem
    {
        protected int _pointIncrement = 10;

        //hacky but eh
        virtual public int PointIncrement
        {
            get { return _pointIncrement; }
            set { _pointIncrement = value; }
        }

        public Leaf(Control control) : base(control)
        {
            Parent = control.Parent;
        }

        public Leaf(Control control, Operation operation)
            : base(control, operation)
        {
            Parent = control.Parent;
            ActivationKey = Keys.Enter;
            Operation.ToExecute = Apply;
            Operation.Params = new Dictionary<string, object> {
                { "RainbowPoints", PointIncrement }
            };
        }

        //tbr....we actually will load the operation data as part of the control configs
        //and pass an operation data structure to this when initializing
        //but for now we'll parse this here to start.

        //oh we could also use the registry to get the room instead of passing a parent control but idk
        public Leaf(Control control, string jsonFileString) : base(control)
        {
            if (!OperationMethodRegistry.TargetOperations.ContainsKey("Items.InstantItems.Leaf.Apply"))
                OperationMethodRegistry.TargetOperations.Add("Items.InstantItems.Leaf.Apply", Apply);

            ActivationKey = Keys.Enter;
            Parent = control.Parent;
            //load the string from the file first
            ConfigFile = jsonFileString;
            var jsonString = Load();
            var loader = new JsonLoader();
            var operationData = loader.LoadFromJson<OperationData>(jsonString);

            Operation = new Operation();
            Operation.Params = ConvertParamList(operationData.ParameterList);
            //what do we do if this object isn't in the dictionary though....should probably handle that.
            Operation.Target = null;
            Operation.TargetName = operationData.TargetName;// GenericGameObjectRegistry.RegisteredObjects[operationData.TargetName];
            Operation.ToExecute = null;
            Operation.ToExecuteName = operationData.MethodToExecute;//ConvertMethodReference(operationData.MethodToExecute);
        }

        virtual protected TargetOperation ConvertMethodReference(string methodName)
        {
            return OperationMethodRegistry.TargetOperations[methodName];
        }

        public Control Parent { get; set; }

        virtual protected object GetPropertyValue(string propertyName)
        {
            var t = GetType();
            var p = t.GetProperty(propertyName);
            return p?.GetValue(this);
        }

        private Dictionary<string, object> ConvertParamList(List<ParameterData> paramData)
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
            /*
            return new Dictionary<string, object> {
                { "RainbowPoints", PointIncrement }
            };*/
        }

        virtual protected Result Apply(GenericGameObject genericGameObject, object paramList)
        {
            var victim = genericGameObject as IItemUser;
            if (victim != null) victim.ApplyItem(paramList);
            return new Result() { Value = true };
        }
    }
}
