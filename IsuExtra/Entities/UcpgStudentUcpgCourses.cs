// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
namespace IsuExtra.Entities
{
    public class UcpgStudentUcpgCourses
    {
        public UcpgStudentUcpgCourses(UcpgStudent student, List<UnitedCoursePreparationGroup> list)
        {
            if (list == null || student == null)
                throw new NullReferenceException();
            Student = student;
            UCPGs = list;
        }

        public UcpgStudent Student
        {
            get;
        }

        public List<UnitedCoursePreparationGroup> UCPGs
        {
            get;
        }
    }
}