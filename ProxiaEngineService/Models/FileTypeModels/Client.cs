using ProxiaEngineService.Models.ProxiaFileFieldModels;
using System.Collections.Generic;
using System.Linq;

namespace ProxiaEngineService.Models.FileTypeModels
{
    public class Client : DocumentBase
    {
        public StringProxiaField ClientNumber { get; } = new StringProxiaField(true, false, 50);    //00
        public StringProxiaField ClientName { get; } = new StringProxiaField(true, false, 50);      //01
        public DeleteFlagProxiaField DeleteFlag { get; } = new DeleteFlagProxiaField();             //02
        public StringProxiaField Color { get; } = new StringProxiaField(false, false, 25);          //03

        public Client()
        {
            Fields = new ProxiaField[]
            {
                ClientNumber,   //00
                ClientName,     //01
                DeleteFlag,     //02
                Color           //03
            };
        }

        public Client(IEnumerable<string> values) : this()
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

        public override string DeutschName => "KUNDENSTAMM";
    }
}
