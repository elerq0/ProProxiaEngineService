using ProxiaEngineService.Models.ProxiaFileFieldModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxiaEngineService.Models.FileTypeModels
{
    public class ProductionOrder : DocumentBase
    {
        public StringProxiaField OrderNumber { get; } = new StringProxiaField(true, false, 50);             //00
        public DateTimeProxiaField ScheduledBegin { get; } = new DateTimeProxiaField(true, false);          //01
        public DateTimeProxiaField ScheduledEnd { get; } = new DateTimeProxiaField(true, false);            //02
        public DateTimeProxiaField ProductionTimestamp { get; } = new DateTimeProxiaField(true, false);     //03

        public ProductionOrder()
        {
            Fields = new ProxiaField[]
            {
                OrderNumber,            //00
                ScheduledBegin,         //01
                ScheduledEnd,           //02
                ProductionTimestamp     //03
            };
        }

        public ProductionOrder(IEnumerable<string> values) : this()
        {
            if (values.Count() != Fields.Length)
                throw new ProxiaParseException("amount of values is invalid");

            var i = 0;
            foreach (var value in values)
            {
                Fields[i].Value = value;
                i++;
            }
        }

        protected override string CheckData(string[] dataTab) => string.Empty;

        public override string DeutschName => "PLANDATEN_ORDER";
    }
}
