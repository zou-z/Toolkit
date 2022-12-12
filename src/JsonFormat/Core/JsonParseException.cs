using System;

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
