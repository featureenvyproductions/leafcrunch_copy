using System;
using System.Collections.Generic;
using System.Linq;

namespace LeafCrunch.GameObjects.Items.ItemOperations
{
    //does a thing to a list of targets
    public class MultiTargetOperation : Operation
    {
        private List<GenericGameObject> _targets;
        public List<GenericGameObject> Targets
        {
            get 
            {
                if (_targets == null && !string.IsNullOrEmpty(TargetType))
                {
                    _targets = new List<GenericGameObject>();
                    //maybe we need to get the targets manually
                    //going by type right now but eventually we could do a list of names
                    //(to do)
                    var t = Type.GetType(TargetType);
                    foreach (var obj in GenericGameObjectRegistry.RegisteredObjects)
                    {
                        var val = obj.Value;
                        if (val.GetType() == t || val.GetType().BaseType == t)
                            _targets.Add(val);
                    }
                    //var targets = GenericGameObjectRegistry.RegisteredObjects.Where(x =>
                      //  x.Value.GetType() == t
                    //);

                }
                return _targets;
            }
            set { _targets = value; }
        }

        //we probably want to be able to do targets by type.
        //that would make configs much easier tbh.

        public string TargetType = null;

        new public Result Execute()
        {
            if (ToExecute == null || Targets == null) return null;

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
