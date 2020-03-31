using ProxiaEngineService.Models.ProxiaFileFieldModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxiaEngineService.Models.FileTypeModels
{
    public class WorkCard : DocumentBase
    {
        public StringProxiaField WorkCardNumber { get; } = new StringProxiaField(true, false, 50);              //00
        public StringProxiaField WorkCardDescription { get; } = new StringProxiaField(true, false, 50);         //01
        public IntProxiaField OperationNumber { get; } = new IntProxiaField(true, false);                       //02
        public DeleteFlagProxiaField DeleteFlag { get; } = new DeleteFlagProxiaField();                         //03
        public StringProxiaField OrderNumber { get; } = new StringProxiaField(true, false, 50);                 //04
        public StringProxiaField OrderDescription { get; } = new StringProxiaField(false, false, 50);           //05
        public StringProxiaField ItemNumber { get; } = new StringProxiaField(false, false, 50);                 //06
        public StringProxiaField ItemDescription { get; } = new StringProxiaField(false, false, 50);            //07
        public DoubleProxiaField MakeReadyTime { get; } = new DoubleProxiaField(true, false);                   //08
        public DoubleProxiaField FullDesiredTime { get; } = new DoubleProxiaField(true, false);                 //09
        public DateTimeProxiaField ShouldBegin { get; } = new DateTimeProxiaField(true, false);                 //10
        public DateTimeProxiaField ShouldEnd { get; } = new DateTimeProxiaField(true, false);                   //11
        public StringProxiaField SignNumber { get; } = new StringProxiaField(false, false, 255);                //12
        public BoolProxiaField IsInternalOrder { get; } = new BoolProxiaField(true, false);                     //13
        public DoubleProxiaField RequiredAmount { get; } = new DoubleProxiaField(true, false);                  //14
        public ListProxiaField WorkCenter { get; } = new ListProxiaField(true, false);                          //15
        public BoolProxiaField IsToProductionPlanning { get; } = new BoolProxiaField(false, true, true);        //16
        public DoubleProxiaField RemainingAmount { get; } = new DoubleProxiaField(false, false);                //17
        public DoubleProxiaField GoodProducedAmount { get; } = new DoubleProxiaField(false, false);             //18
        public DoubleProxiaField BadProducedAmount { get; } = new DoubleProxiaField(false, false);              //19
        public StringProxiaField EndOfTask { get; } = new StringProxiaField(false, false, 50);                  //20//20
        public IntProxiaField CapacityMultiplier { get; } = new IntProxiaField(false, false);                   //21
        public DoubleProxiaField MoveTime { get; } = new DoubleProxiaField(false, false);                       //22
        public DoubleProxiaField WaitTime { get; } = new DoubleProxiaField(false, false);                       //23
        public EnumProxiaField Overlap { get; } = new EnumProxiaField(false, true, 
            new[] { "K", "Q", "R", "ZQ", "ZR" }, "K");                                                   //24
        public DoubleProxiaField OverlapAmount { get; } = new DoubleProxiaField(false, false);                  //25
        public DoubleProxiaField OverlapRate { get; } = new DoubleProxiaField(false, true, 0.2);                //26
        public IntProxiaField Priority { get; } = new IntProxiaField(false, true, 0);                           //27
        public EnumProxiaField WorkCardStatus { get; } = new EnumProxiaField(false, true, 
            new[] { "C_FREI", "C_TFRTG", "C_FRTG" }, "C_FREI");                                          //28
        public StringProxiaField Info1 { get; } = new StringProxiaField(false, false, 255);                     //29
        public StringProxiaField Info2 { get; } = new StringProxiaField(false, false, 255);                     //30
        public StringProxiaField Info3 { get; } = new StringProxiaField(false, false, 255);                     //31
        public StringProxiaField Info4 { get; } = new StringProxiaField(false, false, 255);                     //32
        public StringProxiaField Info5 { get; } = new StringProxiaField(false, false, 255);                     //33
        public StringProxiaField Info6 { get; } = new StringProxiaField(false, false, 255);                     //34
        public StringProxiaField Info7 { get; } = new StringProxiaField(false, false, 255);                     //35
        public StringProxiaField Info8 { get; } = new StringProxiaField(false, false, 255);                     //36
        public StringProxiaField Info9 { get; } = new StringProxiaField(false, false, 255);                     //37
        public StringProxiaField Info10 { get; } = new StringProxiaField(false, false, 255);                    //38
        public BoolProxiaField IsParallelWithPrevious { get; } = new BoolProxiaField(false, true, false);       //39
        public DoubleProxiaField TeardownTime { get; } = new DoubleProxiaField(false, false);                   //40
        public StringProxiaField LanguageText { get; } = new StringProxiaField(false, false, 2000);             //41
        public DoubleProxiaField ItemsPerCycleAmount { get; } = new DoubleProxiaField(false, true, 1);          //42
        public IntProxiaField WorkCardPartsAmount { get; } = new IntProxiaField(false, true, 1);                //43
        public BoolProxiaField IsPreview { get; } = new BoolProxiaField(false, true, false);                    //44
        public BoolProxiaField IsBlocked { get; } = new BoolProxiaField(false, true, false);                    //45
        public IntProxiaField ImpulsPerItemAmount { get; } = new IntProxiaField(false, true, 1);                //46
        public BoolProxiaField IsExlusive { get; } = new BoolProxiaField(false, true, false);                   //47

        public WorkCard()
        {
            Fields = new ProxiaField[]
            {
                WorkCardNumber,             //00
                WorkCardDescription,        //01
                OperationNumber,            //02
                DeleteFlag,                 //03
                OrderNumber,                //04
                OrderDescription,           //05
                ItemNumber,                 //06
                ItemDescription,            //07
                MakeReadyTime,              //08
                FullDesiredTime,            //09
                ShouldBegin,                //10
                ShouldEnd,                  //11
                SignNumber,                 //12
                IsInternalOrder,            //13
                RequiredAmount,             //14
                WorkCenter,                 //15
                IsToProductionPlanning,     //16
                RemainingAmount,            //17
                GoodProducedAmount,         //18
                BadProducedAmount,          //19
                EndOfTask,                  //20
                CapacityMultiplier,         //21
                MoveTime,                   //22
                WaitTime,                   //23
                Overlap,                    //24
                OverlapAmount,              //25
                OverlapRate,                //26
                Priority,                   //27
                WorkCardStatus,             //28
                Info1,                      //29
                Info2,                      //30
                Info3,                      //31
                Info4,                      //32
                Info5,                      //33
                Info6,                      //34
                Info7,                      //35
                Info8,                      //36
                Info9,                      //37
                Info10,                     //38
                IsParallelWithPrevious,     //39
                TeardownTime,               //40
                LanguageText,               //41
                ItemsPerCycleAmount,        //42
                WorkCardPartsAmount,        //43
                IsPreview,                  //44
                IsBlocked,                  //45
                ImpulsPerItemAmount,        //46
                IsExlusive                  //47
            };
        }

        public WorkCard(IEnumerable<string> values) : this()
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
            DateTimeProxiaField sb = new DateTimeProxiaField(false, false) { Value = dataTab[10] };
            DateTimeProxiaField se = new DateTimeProxiaField(false, false) { Value = dataTab[11] };
            if (sb >= se)
                return "ShouldBegin (10) and ShouldEnd (11) fields have invalid values";

            var var26 = float.Parse(dataTab[26]);
            if (var26 < 0 || var26 > 1)
                return "OverlapRate (26) field has invalid value";

            var var27 = int.Parse(dataTab[27]);
            if (var27 < 1 || var27 > 100)
                return "Priority (27) field has invalid value";

            return string.Empty;
        }

        public override string DeutschName => "WT";
    }
}
