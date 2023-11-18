namespace LeafCrunch.GameObjects.Items.ItemOperations
{
    public class Result
    {
        //can be whatever result we need it to be
        //but for now it'll probably just be a bool to say whether or not we're marked for deletion
        public object Value;
    }

    //does a thing to a target
    public class Operation : IOperation
    {
        public delegate Result TargetOperation(GenericGameObject target, object paramList);

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
