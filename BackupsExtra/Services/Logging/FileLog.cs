// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.IO;
using BackupsExtra.Tools;

namespace BackupsExtra.Services.Logging
{
    public class FileLog : IBackupsLog
    {
        public FileLog()
        {
        }

        public FileLog(string logFilePath, bool showDate)
        {
            if (!Directory.Exists(logFilePath))
                throw new BackupsExtraException();

            StreamWriter = new StreamWriter(logFilePath);
            ShowDate = showDate;
        }

        public StreamWriter StreamWriter
        {
            get;
        }

        public bool ShowDate
        {
            get;
        }

        public void MakeLog(string message)
        {
            StreamWriter.WriteLine(ShowDate ? DateTime.Now + " " + message : message);
        }
    }
}