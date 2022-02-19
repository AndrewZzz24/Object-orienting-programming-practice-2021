// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using System.Linq;
using Isu;
using IsuExtra.Entities;
using IsuExtra.Services;
using IsuExtra.Tools;
using Faculty = IsuExtra.Entities.Faculty;

namespace IsuExtra
{
    public class IsuUcpgService : IIsuUcpgService
    {
        private IsuService _isuService = new IsuService();

        private List<UcpgStudentUcpgCourses> _studentUcpg =
            new List<UcpgStudentUcpgCourses>();

        public UnitedCoursePreparationGroup AddUcpg(string name, Faculty faculty, int numOfParallels)
        {
            if (numOfParallels < 0)
                throw new IsuUcpgException("Negative num of parallels in UCPG " + name);
            var newUcpg = new UnitedCoursePreparationGroup(name, faculty, numOfParallels);
            return newUcpg;
        }

        public UcpgStudent AddUcpgStudent(Student student)
        {
            var ucpgStudent = new UcpgStudent(student);
            _studentUcpg.Add(new UcpgStudentUcpgCourses(ucpgStudent, new List<UnitedCoursePreparationGroup>()));
            return ucpgStudent;
        }

        public void AddStudentToUcpg(UcpgStudent student, UnitedCoursePreparationGroup ucpg, int parallel)
        {
            if (ucpg.Faculty.Name.Equals(student.GroupWithSchedule.Group.GetGroupName().Faculty))
            {
                throw new IsuUcpgException("Impossible to register student to UGCP of his faculty");
            }

            CheckSchedule(student, ucpg.Parallels[parallel - 1]);

            foreach (UcpgStudentUcpgCourses studentUcpgs in _studentUcpg.Where(studentUcpgs =>
                studentUcpgs.Student.Info.GetId() == student.Info.GetId()))
            {
                if (studentUcpgs.UCPGs.Contains(ucpg) || studentUcpgs.UCPGs.Count > 1)
                    throw new IsuUcpgException("Impossible to add student to UCPG");

                studentUcpgs.UCPGs.Add(ucpg);
                ucpg.AddStudent(student, parallel);
                AddParallelsLessonsToStudentsSchedule(student, ucpg.Parallels[parallel - 1]);

                return;
            }

            _studentUcpg.Add(new UcpgStudentUcpgCourses(student, new List<UnitedCoursePreparationGroup>()));
            _studentUcpg[^1].UCPGs.Add(ucpg);

            ucpg.AddStudent(student, parallel);
            AddParallelsLessonsToStudentsSchedule(student, ucpg.Parallels[parallel - 1]);
        }

        public void RemoveStudentFromUcpg(UcpgStudent student, UnitedCoursePreparationGroup ucpg)
        {
            foreach (UcpgStudentUcpgCourses studentUcpgs in _studentUcpg.Where(studentUcpgs =>
                studentUcpgs.Student.Info.GetId() == student.Info.GetId()))
            {
                if (!studentUcpgs.UCPGs.Contains(ucpg))
                    throw new IsuUcpgException("This student is not assigned to " + ucpg.Name + " UCPG");

                studentUcpgs.UCPGs.Remove(ucpg);
                ucpg.RemoveStudent(student);

                return;
            }

            throw new IsuUcpgException("This student is not assigned in any UCPG");
        }

        public List<StudentParallel> GetCourseParallels(UnitedCoursePreparationGroup ucpg)
        {
            return ucpg.Parallels;
        }

        public List<UcpgStudent> GetUcpgParallelStudentList(UnitedCoursePreparationGroup ucpg, int parallel)
        {
            parallel--;
            if (parallel < 0 || parallel > ucpg.Parallels.Count)
                throw new IsuUcpgException("Invalid parallel number");

            return ucpg.Parallels[parallel].GetStudentList();
        }

        public List<UcpgStudent> GetUnsignedToUcpgStudentsInGroup(Group @group)
        {
            var result = new List<UcpgStudent>();
            foreach (UcpgStudentUcpgCourses studentUcpg in _studentUcpg)
            {
                if (group.FindStudent(studentUcpg.Student.Info.GetId()) != null && studentUcpg.UCPGs.Count == 0)
                    result.Add(studentUcpg.Student);
            }

            return result;
        }

        private void CheckSchedule(UcpgStudent student, StudentParallel studentParallel)
        {
            for (int i = 0; i < Schedule.MaxDayOfWeek; i++)
            {
                for (int j = 0; j < Schedule.MaxLessonsPerDay; j++)
                {
                    if (student.GroupWithSchedule.Schedule.GetLesson(i, j) != null &&
                        studentParallel.Schedule.GetLesson(i, j) != null)
                    {
                        throw new IsuUcpgException(
                            "Impossible to add student to the parallel due to crossing timetable");
                    }
                }
            }
        }

        private void AddParallelsLessonsToStudentsSchedule(UcpgStudent student, StudentParallel studentParallel)
        {
            for (int i = 0; i < Schedule.MaxDayOfWeek; i++)
            {
                for (int j = 0; j < Schedule.MaxLessonsPerDay; j++)
                {
                    if (student.GroupWithSchedule.Schedule.GetLesson(i, j) == null &&
                        studentParallel.Schedule.GetLesson(i, j) != null)
                    {
                        student.GroupWithSchedule.Schedule.SetLesson(
                            i,
                            j,
                            studentParallel.Schedule.GetLesson(i, j));
                    }
                }
            }
        }
    }
}