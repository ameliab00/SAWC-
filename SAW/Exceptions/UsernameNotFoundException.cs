using System;

namespace SAW.Exceptions
{
    public class UsernameNotFoundException : Exception
    {
        public UsernameNotFoundException(string message) : base(message)
        {
        }
        
        public UsernameNotFoundException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}