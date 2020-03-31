using System;

namespace ProxiaEngineService.Models.ProxiaFileFieldModels
{
    public class StringProxiaField : ProxiaField
    {
        private readonly int maxLength;
        private string value;

        public override string Value
        {
            get
            {
                return value;
            }
            set
            {
                if (value.Length > maxLength)
                {
                    throw new ArgumentException("Input string is too long");
                }
                this.value = value;
            }
        }

        public StringProxiaField(bool isRequired, bool hasDefaultValue, int maxLength, string defaultValue = null)
            : base(isRequired, hasDefaultValue)
        {
            if (hasDefaultValue && defaultValue == null)
                throw new ArgumentException("defaultValue is required");
            if (defaultValue?.Length > maxLength)
                throw new ArgumentException("defaultValue is too long");

            value = defaultValue;
            this.maxLength = maxLength;
        }
    }
}
