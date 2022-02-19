// @author Andrew Zmushko (andrewzmushko@gmail.com)

namespace IsuExtra.Entities
{
    public class Lesson
    {
        private readonly StartEndTime _startEndTime;

        public Lesson(string lessonName, string teacher, int cabinet, string timeStart, string timeEnd)
        {
            _startEndTime = new StartEndTime(timeStart, timeEnd);
            LessonName = lessonName;
            Teacher = teacher;
            Cabinet = cabinet;
        }

        public string LessonName
        {
            get;
        }

        public string Teacher
        {
            get;
        }

        public int Cabinet
        {
            get;
        }

        public string StartTime => _startEndTime.StartTime;
        public string EndTime => _startEndTime.EndTime;
    }
}