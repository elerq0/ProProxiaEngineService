using System;
using System.Globalization;

namespace ProxiaEngineService.Models.ProxiaFileFieldModels
{
    public class DoubleProxiaField : ProxiaField
    {
        public double value;
        public override string Value
        {
            get
            {
                if (value - Math.Truncate(value) == 0)
                    return value.ToString();
                return value.ToString("0.0000", CultureInfo.InvariantCulture);
            }
            set
            {
                if(value != null && value != string.Empty)
                    this.value = double.Parse(value, CultureInfo.InvariantCulture);
            }
        }

        public DoubleProxiaField(bool isRequired, bool hasDefaultValue, double? defaultValue = null)
            : base(isRequired, hasDefaultValue)
        {
            if (hasDefaultValue)
            {
                if (defaultValue.HasValue)
                {
                    value = defaultValue.Value;
                }
                else throw new ArgumentException("default value is null");
            }
        }

        public DoubleProxiaField(bool isRequired, bool hasDefaultValue, string defaultValue)
            : base(isRequired, hasDefaultValue)
        {
            if (hasDefaultValue)
            {
                if (defaultValue != null)
                {
                    double res;
                    bool isOK = double.TryParse(defaultValue, out res);
                    if (isOK)
                    {
                        value = res;
                    }
                    else throw new FormatException("default value has incorrect format");
                }
                else throw new ArgumentException("default value is null");
            }
        }

        #region Operators

        public static bool operator >(DoubleProxiaField left, DoubleProxiaField right)
        {
            return left.value > right.value;
        }

        public static bool operator <(DoubleProxiaField left, DoubleProxiaField right)
        {
            return left.value < right.value;
        }

        #endregion

        #region Convertions

        public static implicit operator double(DoubleProxiaField field)
        {
            return field.value;
        }

        #endregion
    }
}
