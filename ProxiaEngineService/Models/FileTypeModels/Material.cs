using ProxiaEngineService.Models.ProxiaFileFieldModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace ProxiaEngineService.Models.FileTypeModels
{
    public class Material : DocumentBase
    {
        public StringProxiaField MaterialNumber { get; } = new StringProxiaField(true, false, 50);          //00
        public StringProxiaField PlantNumber { get; } = new StringProxiaField(true, false, 50);             //01
        public StringProxiaField Designation { get; } = new StringProxiaField(true, false, 50);             //02
        public StringProxiaField Description { get; } = new StringProxiaField(false, false, 50);            //03
        public StringProxiaField LanguageText { get; } = new StringProxiaField(false, false, 500);          //04
        public DeleteFlagProxiaField DeleteFlag { get; } = new DeleteFlagProxiaField();                     //05
        public StringProxiaField DrawingNumber { get; } = new StringProxiaField(false, false, 50);          //06
        public StringProxiaField VersionNumber { get; } = new StringProxiaField(false, false, 10);          //07
        public EnumProxiaField ObjectState { get; } = new EnumProxiaField(false, true,
            new string[] { "F", "G" }, "F");                                                                //08
        public IntProxiaField LockReasonId { get; } = new IntProxiaField(false, true, 0);                   //09
        public BoolProxiaField IsToProductionPlanning { get; } = new BoolProxiaField(false, true, true);    //10
        public EnumProxiaField ArticleType { get; } = new EnumProxiaField(false, true,
            new string[] { "EINFERTIGUNG", "FREMDBESCHAFFUNG" }, "EINFERTIGUNG");                           //11
        public StringProxiaField Unit { get; } = new StringProxiaField(true, false, 50);                    //12
        public DoubleProxiaField RecoveryTime { get; } = new DoubleProxiaField(false, false);               //13

        public Material()
        {
            Fields = new ProxiaField[]
            {
                MaterialNumber,             //00
                PlantNumber,                //01
                Designation,                //02
                Description,                //03
                LanguageText,               //04
                DeleteFlag,                 //05
                DrawingNumber,              //06
                VersionNumber,              //07
                ObjectState,                //08
                LockReasonId,               //09
                IsToProductionPlanning,     //10
                ArticleType,                //11
                Unit,                       //12
                RecoveryTime                //13
            };
        }

        public Material(IEnumerable<string> values) : this()
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

        public override string DeutschName => "MATSTAMM";

        public override void Load(SqlDataReader reader)
        {
            MaterialNumber.Value = reader["MaterialNumber"].ToString();
            PlantNumber.Value = reader["PlantNumber"].ToString();
            Designation.Value = reader["Designation"].ToString();
            Description.Value = reader["Description"].ToString();
            LanguageText.Value = reader["LanguageText"].ToString();
            DeleteFlag.Value = reader["DeleteFlag"].ToString();
            DrawingNumber.Value = reader["DrawingNumber"].ToString();
            VersionNumber.Value = reader["VersionNumber"].ToString();
            ObjectState.Value = reader["ObjectState"].ToString();
            LockReasonId.Value = reader["LockReasonId"].ToString();
            if (reader["IsToProductionPlanning"].ToString() == "True")
                IsToProductionPlanning.Value = "1";
            else
                IsToProductionPlanning.Value = "0";
            ArticleType.Value = reader["ArticleType"].ToString();
            Unit.Value = reader["Unit"].ToString();
            RecoveryTime.Value = reader["RecoveryTime"].ToString();

            var res = CheckFilling();
            if (!string.IsNullOrEmpty(res))
            {
                //error
                //_logger?.Log(res);
            }
        }

    }
}
