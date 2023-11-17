using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects
{
    //stuff the player can use
    //i think there will be instant, temporary and permanent items (maybe not permanent right now - equippable...that can be for another time)
    //don't know if i need different classes for them yet
    //trying to also decide if we want everything to operate on the player
    //or to be able to operate on other things like rooms or leaves or an eventual game object
    //better make it generic I guess
    //also there can be multiple targets
    //and we'll have a class that defines the operation and says what's getting operated on and how
    //maybe we have a handy callback in there somewhere
    //instant - executes the operation on the target as soon as the player interacts with it and the effect is permanent
    //temporary - executes the operation on the target immediately but the effect changes with each tick (or it just lasts for a certain number of ticks)
    public class GenericItem : InteractiveGameObject
    {
        public GenericItem(Control control) : base(control)
        {
            Active = false;
            Operation = null;
            MarkedForDeletion = false;
        }

        public GenericItem(Control control, Operation operation)
            : base(control)
        {
            Active = false;
            Operation = operation;
            MarkedForDeletion = false;
        }

        public bool Active { get; set; }
        public bool MarkedForDeletion { get; set; }

        public Operation Operation { get; set; }

        new virtual public void Update()
        {
            if (!Active || Operation == null) return;

            //tbr....maybe we can do something with the result but I think for now we'll just have this generic handle thing
            HandleResult(Operation.Execute());
        }

        virtual public void HandleResult(Result result)
        {

        }
    }

    public class Result
    {
        //can be whatever result we need it to be
        //but for now it'll probably just be a bool to say whether or not we're marked for deletion
        public object Value;
    }

    public interface IOperation
    {
        Result Execute();
    }

    //does a thing to a target
    public class Operation : IOperation
    {
        public delegate Result TargetOperation(GenericGameObject target);

        public GenericGameObject Target { get; set; }
        public TargetOperation ToExecute { get; set; }

        public Result Execute()
        {
            if (Target != null && ToExecute != null)
            {
                return ToExecute(Target);
            }
            return null;
        }
    }

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
                    results.Add( new Result()
                    {
                        Value = ToExecute(Target)
                    });
                }
            }
            return new Result()
            {
                Value = results
            };
        }
    }

    public class InstantItem : GenericItem
    {
        public InstantItem(Control control) : base(control)
        {
        }

        public override void HandleResult(Result result)
        {
            //regardless of the outcome, the item is used so we delete it
            MarkedForDeletion = true;
            Active = false;
        }

        public override void Update()
        {
            //oh when will we set it to active though
            if (!Active || Operation == null) return;

            HandleResult(Operation.Execute());
        }
    }

    public class TemporaryItem : GenericItem
    {
        private int _ticks = 10000;
        public int Ticks
        {
            get { return _ticks; }
            set { _ticks = value; }
        }

        public TemporaryItem(Control control) : base(control)
        {
        }

        public override void HandleResult(Result result)
        {
            Ticks--;
            //we don't mark for deletion until we've gone through the ticks.
            if (Ticks == 0)
            {
                Active = false;
                MarkedForDeletion = true;
            }
        }

        public override void Update()
        {
            //oh when will we set it to active though
            if (!Active || Ticks <= 0 || Operation == null) return;

            HandleResult(Operation.Execute());
        }
    }
}
