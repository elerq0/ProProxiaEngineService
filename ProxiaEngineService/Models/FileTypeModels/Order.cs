using ProxiaEngineService.Models.ProxiaFileFieldModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ProxiaEngineService.Models.FileTypeModels
{
    public class Order : DocumentBase
    {
        public StringProxiaField OrderNumber { get; } = new StringProxiaField(true, false, 50);            //00
        public StringProxiaField OrderDescription { get; } = new StringProxiaField(false, false, 50);      //01
        public StringProxiaField PlantNumber { get; } = new StringProxiaField(false, false, 50);           //02
        public DeleteFlagProxiaField DeleteFlag { get; } = new DeleteFlagProxiaField();                    //03
        public EnumProxiaField OrderType { get; } = new EnumProxiaField(false, true,
            new[] { "F", "P", "I", "S", "N", "M", "W" }, "F");                                      //04
        public StringProxiaField CustomerNumber { get; } = new StringProxiaField(false, false, 50);        //05
        public DateTimeProxiaField ShouldBegin { get; } = new DateTimeProxiaField(true, false);            //06
        public DateTimeProxiaField ShouldEnd { get; } = new DateTimeProxiaField(true, false);              //07
        public StringProxiaField ItemNumber { get; } = new StringProxiaField(true, false, 50);             //08
        public StringProxiaField ItemDesciption { get; } = new StringProxiaField(true, false, 50);         //09
        public StringProxiaField SignNumber { get; } = new StringProxiaField(false, false, 255);           //10
        public DoubleProxiaField RequiredAmount { get; } = new DoubleProxiaField(true, false);             //11
        public StringProxiaField LanguageText { get; } = new StringProxiaField(false, false, 2000);        //12
        public StringProxiaField ParentOrderNumber { get; } = new StringProxiaField(false, false, 50);     //13
        public StringProxiaField Info1 { get; } = new StringProxiaField(false, false, 50);                 //14
        public StringProxiaField Info2 { get; } = new StringProxiaField(false, false, 50);                 //15
        public StringProxiaField Info3 { get; } = new StringProxiaField(false, false, 50);                 //16
        public StringProxiaField Info4 { get; } = new StringProxiaField(false, false, 50);                 //17
        public StringProxiaField Info5 { get; } = new StringProxiaField(false, false, 50);                 //18
        public StringProxiaField ParentWorkCardNumber { get; } = new StringProxiaField(false, false, 50);  //19
        public DoubleProxiaField Priority { get; } = new DoubleProxiaField(false, true, 0);                //20

        public Order()
        {
            Fields = new ProxiaField[]
            {
                OrderNumber,            //00
                OrderDescription,       //01
                PlantNumber,            //02
                DeleteFlag,             //03
                OrderType,              //04
                CustomerNumber,         //05
                ShouldBegin,            //06
                ShouldEnd,              //07
                ItemNumber,             //08
                ItemDesciption,         //09
                SignNumber,             //10
                RequiredAmount,         //11
                LanguageText,           //12
                ParentOrderNumber,      //13
                Info1,                  //14
                Info2,                  //15
                Info3,                  //16
                Info4,                  //17
                Info5,                  //18
                ParentWorkCardNumber,   //19
                Priority                //20
            };
        }

        public Order(IEnumerable<string> values) : this()
        {
            if (values.Count() != Fields.Length)
                throw new ProxiaParseException("Amount of values is invalid");

            var i = 0;
            foreach (var value in values)
            {
                Fields[i].Value = value;
                i++;
            }
        }

        #region Helpers

        /*protected static readonly Map<string, string> OldNewDictionary = new Map<string, string>
        {
            {"ORDER_NR", "OrderNumber"},
            {"ORDER_BEZ", "OrderDescription"},
            {"WERK_NR", "PlantNumber"},
            {"DELETE_FLAG", "DeleteFlag"},
            {"ORDER_TYPE", "OrderType"},
            {"KUNDEN_NR", "CustomerNumber"},
            {"SOLL_BEGIN", "ShouldBegin"},
            {"SOLL_END", "ShouldEnd"},
            {"ARTIKEL_NR", "ItemNumber"},
            {"ARTIKEL_BEZ", "ItemDescription"},
            {"ZEICH_NR", "SignNumber"},
            {"QTY_SOLL", "RequiredAmount"},
            {"LANGTXT", "LanguageText"},
            {"PARENT_ORDER_NR", "ParentOrderNumber"},
            {"INFO_01", "Info1"},
            {"INFO_02", "Info2"},
            {"INFO_03", "Info3"},
            {"INFO_04", "Info4"},
            {"INFO_05", "Info5"},
            {"PARENT_WT_NR", "ParentWorkCardNumber"},
            {"PRIORITÄT", "Priority"}
        };*/

        #endregion

        protected override string CheckData(string[] dataTab)
        {
            if (dataTab[13] == string.Empty && dataTab[19] != string.Empty)
                return "Field 19: field 13 is required for fill field 19";

            if (Math.Abs(float.Parse(dataTab[20])) > 10)
                return "Field 20: abs of value is too big";

            return string.Empty;
        }

        public override string DeutschName => "ORDER";
    }
}
