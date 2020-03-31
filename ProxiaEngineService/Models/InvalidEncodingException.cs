using System;

namespace ProxiaEngineService.Models
{
    class InvalidEncodingException : ProxiaParseException
    {
        public InvalidEncodingException()
        { }

        public InvalidEncodingException(string message)
            : base(message)
        { }
    }
}