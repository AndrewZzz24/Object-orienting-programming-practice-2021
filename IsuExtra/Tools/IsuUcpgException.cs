// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Runtime.Serialization;

namespace IsuExtra.Tools
{
    public class IsuUcpgException : Exception
    {
        public IsuUcpgException()
        {
        }

        public IsuUcpgException(string message)
            : base(message)
        {
        }

        public IsuUcpgException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected IsuUcpgException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}