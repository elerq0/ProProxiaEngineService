using System;
using System.Globalization;

namespace ProxiaEngineService.Models.ProxiaFileFieldModels
{
    public class DateTimeProxiaField : ProxiaField
    {
        private const string Format = "yyyyMMddHHmmss";

        public DateTime? value;
        public override string Value
        {
            get { return ToString(); }
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                try
                {
                    this.value = DateTime.ParseExact(value, Format, CultureInfo.InvariantCulture);
                }
                catch (FormatException e)
                {
                    throw new FormatException($"format = {Format}, value = {value}", e);
                }
            }
        }

        public DateTimeProxiaField(bool isRequired, bool hasDefaultValue, string defaltValue = null) 
            : base(isRequired, hasDefaultValue)
        {
            if (!hasDefaultValue)
            {
                value = null;
                return;
            }

            if (defaltValue == null) throw new ArgumentException("default value is required");


            var format = defaltValue.Length == 14 ? "yyyyMMddHHmmss" : "yyyyMMddHHmmssfff";

            value = DateTime.ParseExact(defaltValue, format, CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            if (!value.HasValue)
                return string.Empty;

            string res = value.Value.Year.ToString("D4") + value.Value.Month.ToString("D2") 
                + value.Value.Day.ToString("D2") + value.Value.Hour.ToString("D2") 
                + value.Value.Minute.ToString("D2") + value.Value.Second.ToString("D2");
            return value.Value.Millisecond == 0 ? res : res + value.Value.Millisecond.ToString("D3");
        }

        #region Operators

        public static bool operator >=(DateTimeProxiaField left, DateTimeProxiaField right)
        {
            return left.value >= right.value;
        }

        public static bool operator <=(DateTimeProxiaField left, DateTimeProxiaField right)
        {
            return left.value <= right.value;
        }

        #endregion
    }
}
