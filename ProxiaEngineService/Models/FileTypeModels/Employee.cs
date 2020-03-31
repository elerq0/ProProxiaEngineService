using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxiaEngineService.Models.ProxiaFileFieldModels;

namespace ProxiaEngineService.Models.FileTypeModels
{
    public class Employee : DocumentBase
    {

        public StringProxiaField EmployeeNumber { get; } = new StringProxiaField(true, false, 50);          //00
        public StringProxiaField PlantNumber { get; } = new StringProxiaField(true, false, 50);             //01
        public StringProxiaField FirstName { get; } = new StringProxiaField(true, false, 50);               //02
        public StringProxiaField LastName { get; } = new StringProxiaField(true, false, 50);                //03
        public DeleteFlagProxiaField DeleteFlag { get; } = new DeleteFlagProxiaField();                     //04
        public StringProxiaField Email { get; } = new StringProxiaField(false, false, 50);                  //05
        public StringProxiaField PhoneNumber { get; } = new StringProxiaField(false, false, 50);            //06
        public StringProxiaField CardID { get; } = new StringProxiaField(false, false, 50);                 //07
        public StringProxiaField Language { get; } = new StringProxiaField(false, false, 50);               //08
        public StringProxiaField DBLanguage { get; } = new StringProxiaField(false, false, 50);             //09


        public Employee()
        {
            Fields = new ProxiaField[]
            {
                EmployeeNumber, //00
                PlantNumber,    //01
                FirstName,      //02
                LastName,       //03
                DeleteFlag,     //04
                Email,          //05
                PhoneNumber,    //06
                CardID,         //07
                Language,       //08
                DBLanguage,     //09
            };
        }
 
        public Employee(IEnumerable<string> values) : this()
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

        public override string DeutschName => "PERSSTAMM";
    }
}
