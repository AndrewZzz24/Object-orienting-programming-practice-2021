// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;

namespace IsuExtra.Entities
{
    public class WeekTimetable
    {
        public WeekTimetable()
        {
            Days = new List<DayTimetable>()
            {
                new DayTimetable(),
                new DayTimetable(),
                new DayTimetable(),
                new DayTimetable(),
                new DayTimetable(),
                new DayTimetable(),
                new DayTimetable(),
            };
        }

        public List<DayTimetable> Days
        {
            get;
        }
    }
}