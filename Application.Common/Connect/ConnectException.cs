using System;
namespace ExecutionEngine.Common.Connect
{
    public class ConnectException : Exception
    {
        public ConnectException(string message) : base(message)
        {
        }
        public ConnectException(string message, Exception t) : base(message, t)
        {
        }
    }
}