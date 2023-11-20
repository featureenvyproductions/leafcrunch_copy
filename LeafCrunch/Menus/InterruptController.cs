using LeafCrunch.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

//i'm sure I can probably refactor this....to do
namespace LeafCrunch.Menus
{
    //handles pauses and menus and other stuff that's not interactive or interactive but in a different way than the gameplay
    public class InterruptController
    {
        private List<GenericInterrupt> _interrupts { get; set; }

        private bool _interruptActive = false;
        public bool InterruptActive
        {
            get { return _interruptActive; }
            set { _interruptActive = value; }
        }

        //like the room we'll eventually have a real loading system and not this hacked together ordered control list
        public InterruptController(List<Control> menuControls)
        {
            Load(menuControls);

            //before we start, hide all the menus
            foreach (var interrupt in _interrupts)
            {
                interrupt.Hide();
            }
        }

        protected void Load(List<Control> menuControls)
        {
            _interrupts = new List<GenericInterrupt>()
                {
                    new HelpMenu()
                    {
                        IsActive = false,
                        Control = menuControls.ElementAt(0) // un-hard-code this when we implement the real loading code
                    },
                    new Pause()
                    {
                        IsActive = false
                    }
                };
        }

        public ControllerState OnKeyDown(KeyEventArgs e)
        {
            //the first thing we do is check to see if we're leaving a menu
            //this returns true as long as the menu is still active
            if (_interruptActive)
            {
                //are we leaving a specific menu?
                var activeInterrupt = _interrupts.Where(i => i != null && i.IsActive && i.ActivationKey == e.KeyCode).FirstOrDefault();
                if (activeInterrupt != null)
                {
                    //just leave this menu
                    activeInterrupt.Deactivate();
                    //was that the last one?
                    var remaining = _interrupts.Where(i => i != null && i.IsActive);
                    if (remaining == null || remaining.Count() == 0)
                    {
                        _interruptActive = false;
                        return ControllerState.SUSPEND;
                    }
                }
                //brute force close of all menus
                else if (e.KeyCode == Keys.Escape)
                {
                    var activeInterrupts = _interrupts.Where(i => i != null && i.IsActive);
                    foreach (var i in activeInterrupts)
                    {
                        i.Deactivate();
                    }
                    _interruptActive = false;
                    return ControllerState.SUSPEND;
                }
            }

            //if we're not then see if we're entering one
            var relevantInterrupts = _interrupts.Where(i => i != null && i.ActivationKey == e.KeyCode);

            //don't waste time looping through objects if we're in a menu or paused
            //note: we'll have to do more here later on as we flesh out the menus of course
            //in case menus have key shortcuts
            //eventually we'll have to add something that checks on load to make sure menus arent configured with the same shortcuts
            //or reserved keys like left/right/up/down/enter
            if (relevantInterrupts.Any())
            {
                _interruptActive = true;
                foreach (var i in relevantInterrupts)
                {
                    i.Activate();
                    i.OnKeyPress(e);
                }
                return ControllerState.ACTIVE;
            }

            //idk what to do about this, we should come back to it
            return ControllerState.UNCHANGED;
        }

        public ControllerState OnKeyUp(KeyEventArgs e)
        {
            //see if we have any menus active
            if (_interruptActive)
            {
                //fire off the menu key up events instead.
                var activeInterrupts = _interrupts.Where(i => i != null && i.IsActive);
                foreach (var i in activeInterrupts)
                {
                    i.OnKeyUp(e);
                }
                return ControllerState.ACTIVE;
            }

            return ControllerState.UNCHANGED;
        }

        public ControllerState Update()
        {
            if (_interruptActive)
            {
                var activeInterrupts = _interrupts.Where(i => i != null && i.IsActive);
                foreach (var i in activeInterrupts)
                {
                    i.Update();
                }
                return ControllerState.ACTIVE;
            }
            return ControllerState.UNCHANGED;
        }
    }
}
