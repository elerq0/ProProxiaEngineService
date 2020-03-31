using System;

namespace ProxiaEngineService.Models.ProxiaFileFieldModels
{
    public class IntProxiaField : ProxiaField
    {
        private int? value;
        public override string Value
        {
            get
            {
                return value.ToString();
            }
            set
            {
                if (value == string.Empty)
                {
                    this.value = null;
                    return;
                }
                int res;
                bool isOK = int.TryParse(value, out res);
                if (isOK)
                    this.value = res;
                else throw new ArgumentException("value has incorrect format");
            }
        }

        public IntProxiaField(bool isRequired, bool hasDefaultValue, int defaultValue)
            : base(isRequired, hasDefaultValue)
        {
            if (!hasDefaultValue)
                throw new ArgumentException("Strange error with hasDefaultValue");

            value = defaultValue;
        }

        public IntProxiaField(bool isRequired, bool hasDefaultValue, string defaultValue = null)
            : base(isRequired, hasDefaultValue)
        {
            if (!hasDefaultValue) return;
            if (defaultValue != null)
            {
                Value = defaultValue;
                int res;
                bool isOK = int.TryParse(defaultValue, out res);
                if (isOK)
                    value = res;
                else throw new FormatException("default value has incorrect format");
            }
            else throw new ArgumentException("default value is null");
        }

        #region Convertions

        public static implicit operator int(IntProxiaField field)
        {
            return field.value.Value;
        }

        #endregion
    }
}

