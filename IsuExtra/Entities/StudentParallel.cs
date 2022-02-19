// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
using System.Linq;
using IsuExtra.Tools;

namespace IsuExtra.Entities
{
    public class StudentParallel
    {
        private const int MaximumFreePlacesValue = 21;
        private const int MinimumFreePlacesValue = 0;

        private List<UcpgStudent> _studentsList;

        public StudentParallel(Schedule schedule)
        {
            _studentsList = new List<UcpgStudent>();
            Schedule = schedule;
        }

        public StudentParallel()
        {
            _studentsList = new List<UcpgStudent>();
            Schedule = new Schedule();
        }

        public Schedule Schedule
        {
            get;
            private set;
        }

        public int FreePlaces => MaximumFreePlacesValue - _studentsList.Count;

        public void SetSchedule(Schedule schedule)
        {
            Schedule = schedule;
        }

        public void AddStudent(UcpgStudent student)
        {
            if (FreePlaces == MinimumFreePlacesValue)
                throw new IsuUcpgException("No free places in parallel");

            if (student == null)
                throw new NullReferenceException();

            _studentsList.Add(student);
        }

        public void RemoveStudent(UcpgStudent student)
        {
            if (!_studentsList.Contains(student))
                throw new IsuUcpgException("No student " + student.Info.GetName() + " in parallel");

            _studentsList.Remove(student);
        }

        public bool IsStudentInParallel(UcpgStudent student)
        {
            return _studentsList.Contains(student);
        }

        public List<UcpgStudent> GetStudentList()
        {
            return _studentsList.ToList();
        }
    }
}