// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;

namespace BackupsExtra.Services.Logging
{
    public class ConsoleLog : IBackupsLog
    {
        public ConsoleLog()
        {
        }

        public ConsoleLog(bool showDate)
        {
            ShowDate = showDate;
        }

        public bool ShowDate
        {
            get;
            set;
        }

        public void MakeLog(string message)
        {
            Console.WriteLine(ShowDate ? DateTime.Now + " " + message : message);
        }
    }
}