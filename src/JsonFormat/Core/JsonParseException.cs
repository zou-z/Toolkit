using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonFormat.Core
{
    internal class JsonParseException : Exception
    {
        public string ErrorLocation => errorLocation;

        public JsonParseException(string message, string errorLocation) : base(message)
        {
            this.errorLocation = errorLocation;
        }

        private readonly string errorLocation;
    }
}
