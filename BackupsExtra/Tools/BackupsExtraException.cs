// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Runtime.Serialization;

namespace BackupsExtra.Tools
{
    public class BackupsExtraException : Exception
    {
        public BackupsExtraException()
        {
        }

        public BackupsExtraException(string message)
            : base(message)
        {
        }

        public BackupsExtraException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected BackupsExtraException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}