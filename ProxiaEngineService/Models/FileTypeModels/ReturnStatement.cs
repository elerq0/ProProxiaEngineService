using ProxiaEngineService.Models.ProxiaFileFieldModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxiaEngineService.Models.FileTypeModels
{
    public class ReturnStatement : DocumentBase
    {
        public StringProxiaField WorkCardNumber { get; } = new StringProxiaField(true, false, 50);          //00
        public StringProxiaField MachineNumber { get; } = new StringProxiaField(false, false, 50);          //01
        public StringProxiaField OperatorNumber { get; } = new StringProxiaField(false, false, 50);         //02
        public DoubleProxiaField SetupTime { get; } = new DoubleProxiaField(false, false);                  //03
        public DoubleProxiaField ProcessTime { get; } = new DoubleProxiaField(false, false);                //04
        public DoubleProxiaField GoodItemsAmount { get; } = new DoubleProxiaField(false, false);            //05
        public DoubleProxiaField BadItemsAmount { get; } = new DoubleProxiaField(false, false);             //06
        public EnumProxiaField WorkCardStatus { get; } = new EnumProxiaField(true, false,                   
            new string[] {"C_START", "C_RUECK", "C_TFRTG", "C_FRTG" });                                     //07
        public StringProxiaField OperationDescription { get; } = new StringProxiaField(false, false, 255);  //08
        public DateTimeProxiaField BeginTime { get; } = new DateTimeProxiaField(false, false);              //09
        public DateTimeProxiaField EndTime { get; } = new DateTimeProxiaField(false, false);                //10
        public StringProxiaField EndCharge { get; } = new StringProxiaField(false, false, 50);              //11
        public EnumProxiaField WorkPlaceType { get; } = new EnumProxiaField(false, false,                   
            new string[] { "C_MACH", "C_WKPL", "C_COLLECTIVE_WKPL" });                                      //12
        public StringProxiaField CommitteeBasic { get; } = new StringProxiaField(false, false, 50);         //13
        public StringProxiaField OperatorId { get; } = new StringProxiaField(false, false, 50);             //14
        public StringProxiaField RMComment { get; } = new StringProxiaField(false, false, 2000);            //15

        public ReturnStatement()
        {
            Fields = new ProxiaField[]
            {
                WorkCardNumber,         //00
                MachineNumber,          //01
                OperatorNumber,         //02
                SetupTime,              //03
                ProcessTime,            //04
                GoodItemsAmount,        //05
                BadItemsAmount,         //06
                WorkCardStatus,         //07
                OperationDescription,   //08
                BeginTime,              //09
                EndTime,                //10
                EndCharge,              //11
                WorkPlaceType,          //12
                CommitteeBasic,         //13
                OperatorId,             //14
                RMComment               //15
            };
        }

        public ReturnStatement(IEnumerable<string> values) : this()
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

        public override string DeutschName => "WTRUECK";
    }
}
