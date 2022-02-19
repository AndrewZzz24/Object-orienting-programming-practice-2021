// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Runtime.Serialization;

namespace Backups.Tools
{
    public class BackupException : Exception
    {
        public BackupException()
        {
        }

        public BackupException(string message)
            : base(message)
        {
        }

        public BackupException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected BackupException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}