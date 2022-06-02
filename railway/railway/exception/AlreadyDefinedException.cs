using System;

namespace railway.exception
{
    public class AlreadyDefinedException : Exception
    {
        public string message;
        public AlreadyDefinedException(string message) {
            this.message = message;
        }
        
    }
}