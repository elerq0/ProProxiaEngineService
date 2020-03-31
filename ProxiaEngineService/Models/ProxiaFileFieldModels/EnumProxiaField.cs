using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxiaEngineService.Models.ProxiaFileFieldModels
{
    public class EnumProxiaField : ProxiaField
    {
        private IEnumerable<string> values;

        private string value = string.Empty;
        public override string Value
        {
            get
            {
                return value;
            }
            set
            {
                if (!values.Contains(value))    
                    throw new ArgumentException("ENUM - Input value is invalid");
                this.value = value;
            }
        }

        public EnumProxiaField(bool isRequired, bool hasDefaultValue, IEnumerable<string> enumValues, string defaultValue = null) 
            : base(isRequired, hasDefaultValue)
        {
            values = enumValues;
            if (!enumValues.Contains(defaultValue) && hasDefaultValue)
                throw new ArgumentException("Default value is invalid");
            if (hasDefaultValue)
                value = defaultValue;
        }

        public override string ToString()
        {
            return value;
        }
    }
}
