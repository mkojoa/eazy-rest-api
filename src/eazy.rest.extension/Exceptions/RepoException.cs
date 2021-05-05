using System;

namespace eazy.rest.extension.Exceptions
{
    public class RepoException : Exception
    {
        public RepoException(string message) : base(message)
        {
        }
    }
}