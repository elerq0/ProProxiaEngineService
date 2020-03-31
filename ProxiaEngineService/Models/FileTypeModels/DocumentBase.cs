using Newtonsoft.Json;
using ProxiaEngineService.Models.ProxiaFileFieldModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProxiaEngineService.Models.FileTypeModels
{
    public abstract class DocumentBase
    {
        public abstract string DeutschName { get; }
        public ProxiaField this[ int index]
        {
            get
            {
                return Fields[ index ];
            }
            
        }

        protected ProxiaField[] Fields;
        private readonly Logger _logger;

        protected DocumentBase(Logger logger = null)
        {
            _logger = logger;
        }

        public virtual void ReadFromProxiaFormat(string dataLine)
        {
            string[] firstDataTab = Regex.Split(dataLine, @"(?<!\\)\|");
            var dataTab = new string[firstDataTab.Length - 1];
            for (var i = 0; i < firstDataTab.Length; i++)
            {
                if (i != firstDataTab.Length - 1)
                    dataTab[i] = firstDataTab[i];
            }

            if (dataTab.Length != Fields.Length)
                throw new ProxiaParseException("Invalid dataLine string");

            var error = CheckData(dataTab);
            if (!string.IsNullOrEmpty(error))
                throw new ProxiaParseException(error);

            for (var i = 0; i < dataTab.Length; i++)
            {
                Fields[i].Value = dataTab[i];
            }

            var res = CheckFilling();
            if (!string.IsNullOrEmpty(res)) throw new ProxiaParseException(res);
        }

        public virtual string ToProxiaFormat()
        {
            var builder = new StringBuilder();

            foreach (var field in Fields)
                builder.Append(field +  "|");

            return builder.ToString();
        }

        protected abstract string CheckData(string[] dataTab);

        protected virtual string CheckFilling()
        {
            for (int i = 0; i < Fields.Length; i++)
                if (Fields[i].IsRequired && Fields[i].Value == string.Empty)
                    return $"Required field {i} isn't filled";
            return string.Empty;
        }

        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Loads document data from SQL database
        /// </summary>
        public virtual void Load(SqlDataReader reader)
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                property.SetValue(this, reader[property.Name].ToString());
            }

            var res = CheckFilling();
            if (!string.IsNullOrEmpty(res))
            {
                //error
                _logger?.Log(res);
            }
        }

        /// <summary>
        /// Returns all supported deutch docs names
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetDeutchNames()
        {
            var classes = GetUnfilledDocs();

            return classes.Select(obj => obj.DeutschName);
        }

        // TODO optimization needed
        /// <summary>
        /// Returns document class for given deutsch document name
        /// </summary>
        /// <param name="deutschName">deutsch document name of class</param>
        /// <returns>unfilled document class (deriving from <see cref="DocumentBase"/>)</returns>
        public static DocumentBase Translate(string deutschName)
        {
            var classes = GetUnfilledDocs();

            return classes.First(doc => doc.DeutschName == deutschName);
        }

        /// <summary>
        /// Returns collection of empty objects for every class
        /// </summary>
        /// <returns>collection of instances</returns>
        private static IEnumerable<DocumentBase> GetUnfilledDocs()
        {
            // Get TypeInfo for each class from curr assembly, deriving from DocumentBase
            var infos = Assembly.GetExecutingAssembly()
                .DefinedTypes.Where(type => type.IsSubclassOf(typeof(DocumentBase)));

            // Instantiate them with parameterless constructor
            var classes = infos.Select(info => info.DeclaredConstructors.First().Invoke(new object[0]))
                .Cast<DocumentBase>();

            return classes;
        }
        public override string ToString()
        {
            string fieldsStr = string.Empty;
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (property.Name != "Item" && property.Name != "DeutschName")
                  fieldsStr = fieldsStr + property.Name + ":" + property.GetValue(this) + "; ";
            }
/*
                foreach (ProxiaField f in Fields)
            {
                fieldsStr = fieldsStr + f. + ":" + f.ToString() + "; ";
            }
  */          
            return "{" + DeutschName + ": {" + fieldsStr + "}}";
        }
    }
}
