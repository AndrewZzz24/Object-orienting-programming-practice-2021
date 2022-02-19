// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using Isu;

namespace IsuExtra.Entities
{
    public class UcpgStudent : ICloneable
    {
        private readonly Student _student;

        public UcpgStudent(Student student)
        {
            GroupWithSchedule = new GroupWithSchedule(student.GetGroup());
            _student = student;
        }

        public Student Info => _student;

        public GroupWithSchedule GroupWithSchedule
        {
            get;
        }

        public object Clone()
        {
            return new UcpgStudent(_student);
        }
    }
}