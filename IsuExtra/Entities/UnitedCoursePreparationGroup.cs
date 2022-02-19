// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using IsuExtra.Tools;

namespace IsuExtra.Entities
{
    public class UnitedCoursePreparationGroup
    {
        public UnitedCoursePreparationGroup(string name, Faculty faculty, int numOfParallels)
        {
            if (faculty == null || numOfParallels < 0)
                throw new IsuUcpgException("Invalid Ucpg constructor parameters");

            Name = name;
            Parallels = new List<StudentParallel>();
            for (int i = 0; i < numOfParallels; i++)
                Parallels.Add(new StudentParallel());
            Faculty = faculty;
        }

        public List<StudentParallel> Parallels
        {
            get;
        }

        public string Name
        {
            get;
        }

        public Faculty Faculty
        {
            get;
        }

        public void SetParallelsSchedule(int parallelNum, Schedule schedule)
        {
            CheckParallelsNumberCorrectness(parallelNum);
            Parallels[parallelNum - 1].SetSchedule(schedule);
        }

        public void AddStudent(UcpgStudent student, int parallel)
        {
            CheckParallelsNumberCorrectness(parallel);
            Parallels[parallel - 1].AddStudent(student);
        }

        public void RemoveStudent(UcpgStudent student)
        {
            foreach (StudentParallel studentParallel in Parallels)
            {
                if (studentParallel.IsStudentInParallel(student))
                {
                    studentParallel.RemoveStudent(student);
                    return;
                }
            }

            throw new IsuUcpgException("This student cannot be removed as he's not assigned to any UCPG");
        }

        private void CheckParallelsNumberCorrectness(int parallel)
        {
            if (parallel - 1 >= Parallels.Count || parallel - 1 < 0) // 0 - lowest possible list index
                throw new IsuUcpgException("No " + parallel + " parallel in " + Name + " UCPG");
        }
    }
}