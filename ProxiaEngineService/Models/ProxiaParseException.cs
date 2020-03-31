using System;

namespace ProxiaEngineService.Models
{
    public class ProxiaParseException : ApplicationException
    {
        public ProxiaParseException()
        { }

        public ProxiaParseException(string message) : base(message)
        { }

        public ProxiaParseException(string message, Exception innerException) 
            : base(message, innerException)
        { }
    }
}