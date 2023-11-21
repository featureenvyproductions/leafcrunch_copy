using LeafCrunch.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LeafCrunch.GameObjects
{
    //this exists because I need ways for objects to operate on or talk to each other
    //without them being hard coded
    public class GenericGameObjectRegistry
    {
        private static Dictionary<string, GenericGameObject> _registeredObjects = null;
        public static Dictionary<string, GenericGameObject> RegisteredObjects {
            get
            {
                if (_registeredObjects == null) _registeredObjects = new Dictionary<string, GenericGameObject>();
                return _registeredObjects;
            }
            set
            {
                _registeredObjects = value;
            }
        }
    }

    //the base thing all the useful things inherit from
    public abstract class GenericGameObject
    {
        public Control Control { get; set; } //the associated forms control

        public GenericGameObject Parent { get; set; }

        virtual public string ConfigFile { get; set; }

        public GenericGameObject()
        {
            Control = new Control();
        }

        public GenericGameObject(Control control)
        {
            Control = control;
        }

        public GenericGameObject(Control control, GenericGameObject parent)
        {
            Control = control;
            Parent = parent;
        }
        protected virtual string Load()
        {
            //for a bigger more involved game
            //we'd probably want to load config data into some kind of indexed cache
            //or grab it as needed from a database
            //but for this tiny game it's ok to just load things into memory
            if (ConfigFile == null) return string.Empty;
            return File.ReadAllText(UtilityMethods.GetConfigPath(ConfigFile));
        }

        public virtual void Initialize() { }
        public virtual void Update() { }
        public virtual void OnKeyPress(KeyEventArgs e) { }
        public virtual void OnKeyUp(KeyEventArgs e) { }
    }
}
