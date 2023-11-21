using LeafCrunch.GameObjects.ItemProperties;
using LeafCrunch.GameObjects.Items.ItemOperations;
using System.Collections.Generic;
using System.Windows.Forms;

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
        public Leaf(Control control, string operationName) : base(control)
        {
            if (!OperationMethodRegistry.TargetOperations.ContainsKey("Items.InstantItems.Leaf.Apply"))
                OperationMethodRegistry.TargetOperations.Add("Items.InstantItems.Leaf.Apply", Apply);

            ActivationKey = Keys.Enter;
            Parent = control.Parent;

            InitializeOperationFromRegistry(operationName);
           /* //get the operation from the registry
            //tbd....how do we want to handle this if it's not there
            var operation = OperationRegistry.Operations[operationName];

            //make a copy in case other people want to use it but need to initialize the params differently
            //because i cannot be bothered to make some kind of parameter registry from hell.
            //you know, it occurs to me I can just have a local registry of operation names etc here
            //but omg i don't care whatever i'll sit on it and change it later maybe
            //it's dynamic rn and that's what I care about

            Operation = new Operation()
            {
                //we'll need to initialize the parameters
                //this is kind of dumb...I'll make it better later I guess
                Params = ConvertParamList(operation.ParamData),
                ParamData = operation.ParamData,
                TargetName = operation.TargetName,
                Target = null, //it'll get picked up later
                ToExecute = null,
                ToExecuteName = operation.ToExecuteName //this will get picked up later
            };*/

        }

        public Control Parent { get; set; }

        virtual protected Result Apply(GenericGameObject genericGameObject, object paramList)
        {
            var victim = genericGameObject as IItemUser;
            if (victim != null) victim.ApplyItem(paramList);
            return new Result() { Value = true };
        }
    }
}
