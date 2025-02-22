using System;

namespace SAW.Exceptions
{
    public class EmailExistsException : Exception
    {
        public EmailExistsException(string message) : base(message)
        {
        }
    }
}