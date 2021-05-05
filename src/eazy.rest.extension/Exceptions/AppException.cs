using System;

namespace eazy.rest.extension.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string message) : base(message)
        {
        }

        public virtual string Code { get; }
    }
}