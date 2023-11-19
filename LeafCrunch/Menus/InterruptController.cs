using LeafCrunch.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LeafCrunch.Menus
{
    //handles pauses and menus and other stuff that's not interactive or interactive but in a different way than the gameplay
    public class InterruptController
    {
        private bool interruptActive = false;
        public bool InterruptActive
        {
            get { return interruptActive; }
            set { interruptActive = value; }
        }

        //like the room we'll eventually have a real loading system and not this hacked together ordered control list
        public InterruptController(List<Control> menuControls)
        {
            Load(menuControls);

            //before we start, hide all the menus
            foreach (var interrupt in Interrupts)
            {
                interrupt.Hide();
            }
        }

        protected void Load(List<Control> menuControls)
        {
            Interrupts = new List<GenericInterrupt>()
                {
                    new HelpMenu()
                    {
                        IsActive = false,
                        Control = menuControls.ElementAt(0)
                    },
                    new Pause()
                    {
                        IsActive = false
                    }
                };
        }

        private List<GenericInterrupt> Interrupts { get; set; }

        public ControllerState OnKeyDown(KeyEventArgs e)
        {
            //the first thing we do is check to see if we're leaving a menu
            //this returns true as long as the menu is still active
            if (interruptActive)
            {
                //are we leaving a specific menu?
                var activeInterrupt = Interrupts.Where(i => i != null && i.IsActive && i.ActivationKey == e.KeyCode).FirstOrDefault();
                if (activeInterrupt != null)
                {
                    //just leave this menu
                    activeInterrupt.Deactivate();
                    //was that the last one?
                    var remaining = Interrupts.Where(i => i != null && i.IsActive);
                    if (remaining == null || remaining.Count() == 0)
                    {
                        interruptActive = false;
                        //yes so
                        //pass event handling back to the room
                        //i mean really what we need to do is have a state machine with a Room state and a Menu state
                        //Room.Resume();

                        //change this from resume...we want to SUSPEND the current state
                        return ControllerState.SUSPEND;
                    }
                    //no, don't pass event handling back to the room
                    //return;
                    //   return RoomState.NOTHING;
                }
                //brute force close of all menus
                else if (e.KeyCode == Keys.Escape)
                {
                    var activeInterrupts = Interrupts.Where(i => i != null && i.IsActive);
                    foreach (var i in activeInterrupts)
                    {
                        i.Deactivate();
                    }
                    interruptActive = false;
                    //Room.Resume();
                    //return;

                    //we want to SUSPEND the current state of this controller and resume the room
                    //return ControllerState.RESUME;
                    return ControllerState.SUSPEND;
                }
            }

            //if we're not then see if we're entering one
            var relevantInterrupts = Interrupts.Where(i => i != null && i.ActivationKey == e.KeyCode);

            //don't waste time looping through objects if we're in a menu or paused
            //note: we'll have to do more here later on as we flesh out the menus of course
            //in case menus have key shortcuts
            //eventually we'll have to add something that checks on load to make sure menus arent configured with the same shortcuts
            //or reserved keys like left/right/up/down/enter
            if (relevantInterrupts.Any())
            {
                interruptActive = true;
                //before we do anything else, we need to suspend the room so that the menus take over event handling
                //Room.Suspend();
                foreach (var i in relevantInterrupts)
                {
                    i.Activate();
                    //call their update functions
                    i.OnKeyPress(e);
                }
                //return;
                //we want to suspend the room but activate this
                //return ControllerState.SUSPEND;
                return ControllerState.ACTIVE;
            }

            //idk what to do about this, we should come back to it
            return ControllerState.UNCHANGED;
        }

        public ControllerState OnKeyUp(KeyEventArgs e)
        {
            //see if we have any menus active
            if (interruptActive)
            {
                //fire off the menu key up events instead.
                var activeInterrupts = Interrupts.Where(i => i != null && i.IsActive);
                foreach (var i in activeInterrupts)
                {
                    i.OnKeyUp(e);
                }
                //return ControllerState.SUSPEND;
                return ControllerState.ACTIVE;
            }

            return ControllerState.UNCHANGED;
        }

        public ControllerState Update()
        {
            if (interruptActive)
            {
                var activeInterrupts = Interrupts.Where(i => i != null && i.IsActive);
                foreach (var i in activeInterrupts)
                {
                    i.Update();
                }
                //return ControllerState.SUSPEND;
                return ControllerState.ACTIVE;
            }
            return ControllerState.UNCHANGED;
        }
    }
}
