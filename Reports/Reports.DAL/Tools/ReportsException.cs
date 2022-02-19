// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Runtime.Serialization;

namespace Reports.DAL.Tools
{
    public class ReportsException : Exception
    {
        public ReportsException()
        {
        }

        public ReportsException(string message)
            : base(message)
        {
        }

        public ReportsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ReportsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}