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

        protected double ValueAsDouble
        {
            get
            {
                try
                {
                    return Double.Parse(Value.ToString());
                }
                catch { return 0; }
            }
        }

        protected int ValueAsInt
        {
            get
            {
                try
                {
                    return Int32.Parse(Value.ToString());
                }
                catch { return 0; }
            }
        }

        protected string ValueAsString
        {
            get
            {
                try
                {
                    return Value.ToString();
                }
                catch { return string.Empty; }
            }
        }

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
                        return ((double)propValue >= ValueAsDouble) ? WinCondition : WinCondition.None;
                    else
                        return ((int)propValue >= ValueAsInt) ? WinCondition : WinCondition.None;
                case "<=":
                    if (ValueType == "Double")
                        return ((double)propValue <= ValueAsDouble) ? WinCondition : WinCondition.None;
                    else
                        return ((int)propValue <= ValueAsInt) ? WinCondition : WinCondition.None;
                case "==":
                    if (ValueType == "Double")
                        return ((double)propValue == ValueAsDouble) ? WinCondition : WinCondition.None;
                    else if (ValueType == "Integer")
                        return ((int)propValue == ValueAsInt) ? WinCondition : WinCondition.None;
                    else
                        return (propValue.ToString() == ValueAsString) ? WinCondition : WinCondition.None;
            }
            return WinCondition.None;
        }
    }
}
