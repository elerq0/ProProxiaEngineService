using ProxiaEngineService.Models.ProxiaFileFieldModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxiaEngineService.Models.FileTypeModels
{
    public class ProductionPlan : DocumentBase
    {
        public StringProxiaField WorkCardNumber { get; } = new StringProxiaField(true, false, 50 /* TODO: w przypadku podziału karty pracy na części, przesyłana w tym polu wartość odnosi się do numeru pierwotnej karty pracy. */);          //00
        public StringProxiaField ItemsCode { get; } = new StringProxiaField(true, false, 50);               //01
        public DateTimeProxiaField ScheduledBegin { get; } = new DateTimeProxiaField(true, false);          //02
        public DateTimeProxiaField ScheduledEnd { get; } = new DateTimeProxiaField(true, false);            //03
        public DateTimeProxiaField ProductionTimestamp { get; } = new DateTimeProxiaField(true, false);     //04
        public IntProxiaField PlannedPallet { get; } = new IntProxiaField(false, false /* TODO: tylko wtedy, gdy zostanie to wygenerowane przez planowanie produkcji.*/);     //05
        public IntProxiaField CountOfSplits { get; } = new IntProxiaField(false, false);     //06

        public ProductionPlan()
        {
            Fields = new ProxiaField[]
            {
                WorkCardNumber,         //00
                ItemsCode,              //01
                ScheduledBegin,         //02
                ScheduledEnd,           //03
                ProductionTimestamp,    //04
                PlannedPallet,          //05
                CountOfSplits           //06
            };
        }

        public ProductionPlan(IEnumerable<string> values) : this()
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

        public override string DeutschName => "PLANDATEN";
    }
}
