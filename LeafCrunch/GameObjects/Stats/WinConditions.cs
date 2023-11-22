using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeafCrunch.GameObjects.Stats
{
    public enum WinCondition
    {
        None,
        Win,
        Lose
    };

    public class Condition
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public string Comparison { get; set; }
        public string ValueType { get; set; }
        public WinCondition WinCondition { get; set; }

        public WinCondition CheckCondition(object parent)
        {
            //get the property value
            var t = parent.GetType();
            var p = t.GetProperty(PropertyName);
            var propValue = p?.GetValue(parent);

            switch (Comparison)
            {
                case ">=":
                    if (ValueType == "Double")
                        return ((double)propValue >= (double)Value) ? WinCondition : WinCondition.None;
                    else
                        return ((int)propValue >= (int)Value) ? WinCondition : WinCondition.None;
                case "<=":
                    if (ValueType == "Double")
                        return ((double)propValue <= (double)Value) ? WinCondition : WinCondition.None;
                    else
                        return ((double)propValue <= (double)Value) ? WinCondition : WinCondition.None;
                case "==":
                    //this will only work for values so I'm not sure about it
                    return (propValue.Equals(Value)) ? WinCondition : WinCondition.None;
            }
            return WinCondition.None;
        }
    }
}
