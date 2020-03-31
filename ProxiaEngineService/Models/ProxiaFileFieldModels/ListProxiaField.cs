using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProxiaEngineService.Models.ProxiaFileFieldModels
{
    public class ListProxiaField : ProxiaField
    {
        private string[] value;
        public override string Value
        {
            get
            {
                return String.Join(", ", value);
            }
            set
            {
                SetValue(value);
            }
        }

        public ListProxiaField(bool isRequired, bool hasDefaultValue, string defaultValue = null) 
            : base(isRequired, hasDefaultValue)
        {
            if (defaultValue != null)
                SetValue(defaultValue);
            else
                value = new string[] { };
        }

        private void SetValue(string newValue)
        {
            value = newValue.Split(new string[] { ", " }, StringSplitOptions.None);
        }
    }
}
