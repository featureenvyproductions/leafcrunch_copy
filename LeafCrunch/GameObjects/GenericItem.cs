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
           // Operation = null;
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

        public InstantItem(Control control, Operation operation)
            : base(control)
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

    //this would be like when i have cooler stuff like pine cones and they give you a point multiplier or something
    //for a limited time
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


    #region Too many fucking comments
    //some simple items to start
    //leaves
    //if you stand on a leaf and press enter
    //it becomes active
    //although I'm not sure who decides that yet
    //since each object can only occupy a spot, it's probably better to have the player loop through and see if it's colliding
    //with an object. then we'll stop checking all the objects most of the time before going through the lot of them
    //if every object checks for the player, I always have to check every object
    //there's probably a better way to do this but I'm too stupid to know what it is right now
    //OH you know what might be a good way to do this since I'm planning on keeping everything the same size rn
    //divide my board up into virtual tiles and calculate what tile the player is on
    //and calculate when a leaf is initialized what tile it's on (since the board doesn't really change size either)
    //and then I can see if the player and board are on the same tiles.
    //we can get more granular with this later too and make the tiles smaller and have like board regions and then tile regions and then
    //partial tile regions.
    //maybe I'll do like quadrants and tiles idk. tiles for now and then we can do better later
    //we can also use that thing where we just search half the things for a tile index match
    //and then keep halving the list


    #endregion
    //ok so the way this works is that the rainbow bar reflects the player's rainbow points
    //and the leaf operates on the player to produce that result
    //when the operation is complete the leaf can be removed from the list of room items
    public abstract class Leaf : InstantItem
    {
        protected static int _pointIncrement = 10;

        public Leaf(Control control) : base(control)
        {
        }

        public Leaf(Control control, Operation operation)
            : base(control, operation)
        {
        }

        //what we'll ultimately use as the operation
        public static Result PointChange(Player player)
        {
            player.RainbowPoints += _pointIncrement;
            return new Result() { Value = true };
        }
    }

    public class GreenLeaf : Leaf
    {
        //god it's annoying that have to have this constructor everywhere why did i do this
        public GreenLeaf(Control control) : base(control)
        {
            _pointIncrement = 1;
        }

        public GreenLeaf(Control control, Operation operation)
            : base(control, operation)
        {
            _pointIncrement = 1;
        }
    }

    public class YellowLeaf : Leaf
    {
        //god it's annoying that have to have this constructor everywhere why did i do this
        public YellowLeaf(Control control) : base(control)
        {
            _pointIncrement = 5;
        }

        public YellowLeaf(Control control, Operation operation)
            : base(control, operation)
        {
            _pointIncrement = 5;
        }
    }

    public class OrangeLeaf : Leaf
    {
        //god it's annoying that have to have this constructor everywhere why did i do this
        public OrangeLeaf(Control control) : base(control)
        {
            _pointIncrement = 10;
        }

        public OrangeLeaf(Control control, Operation operation)
            : base(control, operation)
        {
            _pointIncrement = 10;
        }
    }

    public class RedLeaf : Leaf
    {
        //god it's annoying that have to have this constructor everywhere why did i do this
        public RedLeaf(Control control) : base(control)
        {
            _pointIncrement = 15;
        }

        public RedLeaf(Control control, Operation operation)
            : base(control, operation)
        {
            _pointIncrement = 15;
        }
    }
}
