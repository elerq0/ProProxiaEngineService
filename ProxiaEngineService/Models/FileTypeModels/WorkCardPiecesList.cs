using ProxiaEngineService.Models.ProxiaFileFieldModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxiaEngineService.Models.FileTypeModels
{
    public class WorkCardPiecesList : DocumentBase
    {
        public StringProxiaField WorkCardNumber { get; } = new StringProxiaField(true, false, 50);          //00
        public StringProxiaField MaterialNumber { get; } = new StringProxiaField(true, false, 50);          //01
        public StringProxiaField MeasurementUnit { get; } = new StringProxiaField(true, true, 50, "STK");   //02
        public DoubleProxiaField GoodItemsAmount { get; } = new DoubleProxiaField(true, false);             //03
        public EnumProxiaField Destination { get; } = new EnumProxiaField(false, true,
            new string[] { "A", "B" }, "B");                                                                //04
        public DoubleProxiaField ReservedAmount { get; } = new DoubleProxiaField(false, false);             //05
        public IntProxiaField PositionNumber { get; } = new IntProxiaField(true, false);                    //06
        public BoolProxiaField IsLotNumberRequired { get; } = new BoolProxiaField(false, true, false);      //07

        public WorkCardPiecesList()
        {
            Fields = new ProxiaField[]
            {
                WorkCardNumber,         //00
                MaterialNumber,         //01
                MeasurementUnit,        //02
                GoodItemsAmount,        //03
                Destination,            //04
                ReservedAmount,         //05
                PositionNumber,         //06
                IsLotNumberRequired     //07
            };                          
        }

        public WorkCardPiecesList(IEnumerable<string> values) : this()
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

        public override string DeutschName => "WTMAT";
    }
}
