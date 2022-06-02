using System;

namespace railway.exception
{
    public class NotDefinedException : Exception
    {
        public string message;
        public NotDefinedException(string message) {
            this.message = message;
        }
        
    }
}