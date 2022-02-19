// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using IsuExtra.Tools;

namespace IsuExtra.Entities
{
    public class StartEndTime
    {
        public StartEndTime(string startTime, string endTime)
        {
            CheckTimeCorrectness(startTime, endTime);
            StartTime = startTime;
            EndTime = endTime;
        }

        public string StartTime
        {
            get;
        }

        public string EndTime
        {
            get;
        }

        private void CheckTimeCorrectness(string start, string end)
        {
            int div1 = start.IndexOf(":", StringComparison.Ordinal);
            int div2 = end.IndexOf(":", StringComparison.Ordinal);

            if (div1 == -1 || div2 == -1)
                throw new IsuUcpgException("Invalid time format");

            bool result1 = int.TryParse(start.Substring(0, div1), out int hours1);
            bool result2 = int.TryParse(end.Substring(0, div2), out int hours2);
            bool result3 = int.TryParse(start.Substring(div1 + 1, 2), out int minutes1);
            bool result4 = int.TryParse(end.Substring(div2 + 1, 2), out int minutes2);

            if (!result1 || !result2 || !result3 || !result4)
                throw new IsuUcpgException("Invalid time format");
        }
    }
}