using System;

namespace MongoDBApi.CustomExceptions
{
    public class NoDBConnectionException : Exception
    {
        public NoDBConnectionException() {} //parameterless constructor
        public NoDBConnectionException(string message) : base(message) {}
        public NoDBConnectionException(string message, Exception inner) : base(message, inner) {}
    }
}