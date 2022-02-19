// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using IsuExtra.Tools;

namespace IsuExtra.Entities
{
    public class Schedule
    {
        public const int MaxDayOfWeek = 7;
        public const int MaxLessonsPerDay = 7;

        private static List<StartEndTime> lessonsTime = new List<StartEndTime>()
        {
            new StartEndTime("8:20", "9:50"), // num in list == day of the week
            new StartEndTime("10:00", "11:30"),
            new StartEndTime("11:40", "13:10"),
            new StartEndTime("13:30", "15:00"),
            new StartEndTime("15:20", "16:50"),
            new StartEndTime("17:00", "18:30"),
            new StartEndTime("18:40", "20:10"),
        };

        public Schedule(WeekTimetable weekLessons)
        {
            CheckScheduleCorrectness(weekLessons);

            WeekLessons = weekLessons;
        }

        public Schedule()
        {
            WeekLessons = new WeekTimetable();
        }

        private WeekTimetable WeekLessons
        {
            get;
        }

        public Lesson GetLesson(int dayOfTheWeek, int lessonNum)
        {
            CheckDayAndWeekCorrectness(dayOfTheWeek, lessonNum);
            return WeekLessons.Days[dayOfTheWeek].Lessons[lessonNum];
        }

        public void SetLesson(int dayOfTheWeek, int lessonNum, Lesson lesson)
        {
            CheckDayAndWeekCorrectness(dayOfTheWeek, lessonNum);
            WeekLessons.Days[dayOfTheWeek].Lessons[lessonNum] = lesson;
        }

        private static void CheckScheduleCorrectness(WeekTimetable weekLessons)
        {
            foreach (DayTimetable day in weekLessons.Days)
            {
                if (day.Lessons.Count != MaxLessonsPerDay)
                    throw new IsuUcpgException("Invalid schedule format");

                for (int j = 0; j < day.Lessons.Count; j++)
                {
                    if (day.Lessons[j] != null && !lessonsTime[j].StartTime.Equals(day.Lessons[j].StartTime))
                        throw new IsuUcpgException("Invalid Lesson Start time");
                    if (day.Lessons[j] != null && !lessonsTime[j].EndTime.Equals(day.Lessons[j].EndTime))
                        throw new IsuUcpgException("Invalid Lesson End time");
                }
            }
        }

        private static void CheckDayAndWeekCorrectness(int dayOfTheWeek, int lessonNum)
        {
            if (dayOfTheWeek >= MaxDayOfWeek || dayOfTheWeek < 0 || lessonNum < 0 || lessonNum >= MaxLessonsPerDay)
                throw new IsuUcpgException("Invalid day or lesson time");
        }
    }
}