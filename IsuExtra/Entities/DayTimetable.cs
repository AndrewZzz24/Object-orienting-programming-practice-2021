// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;

namespace IsuExtra.Entities
{
    public class DayTimetable
    {
        public DayTimetable()
        {
            Lessons = new List<Lesson>() { null, null, null, null, null, null, null };
        }

        public List<Lesson> Lessons
        {
            get;
        }
    }
}