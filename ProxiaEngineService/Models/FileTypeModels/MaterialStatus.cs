using ProxiaEngineService.Models.ProxiaFileFieldModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxiaEngineService.Models.FileTypeModels
{
    public class MaterialStatus : DocumentBase
    {
        public StringProxiaField MaterialNumber { get; } = new StringProxiaField(true, false, 50);      //00
        public StringProxiaField PlantNumber { get; } = new StringProxiaField(true, false, 50);         //01
        public EnumProxiaField StockType { get; } = new EnumProxiaField(true, true,
            new string[] { "B", "A", "O", "E" }, "B");                                                  //02
        public DateTimeProxiaField Timestamp { get; } = new DateTimeProxiaField(false, false);          //03
        public StringProxiaField MeasurementUnit { get; } = new StringProxiaField(true, false, 50);     //04
        public DoubleProxiaField Amount { get; } = new DoubleProxiaField(true, false);                  //05
        public StringProxiaField OrderNumber { get; } = new StringProxiaField(false, false, 50);        //06
        public StringProxiaField WorkCardNumber { get; } = new StringProxiaField(false, false, 50);     //07

        public MaterialStatus()
        {
            Fields = new ProxiaField[]
            {
                MaterialNumber,     //00
                PlantNumber,        //01
                StockType,          //02
                Timestamp,          //03
                MeasurementUnit,    //04
                Amount,             //05
                OrderNumber,        //06
                WorkCardNumber      //07
            };                      
        }

        public MaterialStatus(IEnumerable<string> values) : this()
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

        protected override string CheckData(string[] dataTab)
        {
            switch (dataTab[2])
            {//TODO
                case "A":
                    if (dataTab[3] == string.Empty)
                        return "Timestamp field (3) is required for \"A\" value of StockType (2) ";
                    break;
                case "B":
                    break;
                case "E":
                case "O":
                    if (float.Parse(dataTab[5]) < 0)
                        return "Amount field (5) must be greater than 0 for \"E\" or \"O\" value of StockType (2)";
                    break;
                default:
                    return "Invalid Stock Type (2)";
            }

            return string.Empty;
        }

        public override string DeutschName => "MATBESTAND";
    }
}
