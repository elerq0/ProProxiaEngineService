using System;

namespace ProxiaEngineService.Models.ProxiaFileFieldModels
{
    public class BoolProxiaField : ProxiaField
    {
        private bool value;
        public override string Value
        {
            get
            {
                return value ? "1" : "0";
            }
            set
            {
                this.value = ToBool(value);
            }
        }

        public BoolProxiaField(bool isRequired, bool hasDefaultValue, bool defaultValue) 
            : base(isRequired, hasDefaultValue)
        {
            if (!hasDefaultValue)
                throw new ArgumentException("Strange error with hasDefaultValue");

            value = defaultValue;
        }

        public BoolProxiaField(bool isRequired, bool hasDefaultValue, string defaultValue = null)
            : base(isRequired, hasDefaultValue)
        {
            if (hasDefaultValue && defaultValue == null)
                throw new ArgumentException("Default value is invalid");

            if (hasDefaultValue)
            {
                value = ToBool(defaultValue);
            }
        }

        public override string ToString()
        {
            return value ? "1" : "0";
        }

        #region Helpers

        private static bool ToBool(string str)
        {
            switch (str)
            {
                case "0":
                    return false;
                case "1":
                    return true;
                default:
                    throw new ArgumentException("Input value is invalid");
            }
        }

        #endregion
    }
}
