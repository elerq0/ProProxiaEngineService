using ProxiaEngineService.Models.FileTypeModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxiaEngineService.Models
{
    public class ValuesList<T> : List<T> where T : DocumentBase
    {
        public void WriteToFile(string path)
        {
            if (this.Any(doc => doc.GetType() != this.First().GetType()))
                throw new ProxiaParseException("All elements should have same type");

            string[] lines = this.Select(elem => elem.ToProxiaFormat()).ToArray();
            File.WriteAllLines(path, lines);
        }

        public string[] WriteToData()
        {
            string[] lines = this.Select(elem => elem.ToProxiaFormat()).ToArray();
            return lines;
        }
    }
}
