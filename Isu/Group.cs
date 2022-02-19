using System.Collections.Generic;
using Isu.Tools;

namespace Isu
{
    public class Group
    {
        private GroupName _name;
        private List<Student> _members = new List<Student>();

        public Group(GroupName name)
        {
            _name = name;
        }

        public GroupName GetGroupName()
        {
            return _name;
        }

        public int GetCourseNum()
        {
            return _name.GetCourseNumber().GetCourseNum();
        }

        public int GetNumberOfStudents()
        {
            return _members.Count;
        }

        public bool AddToGroup(Student student)
        {
            CheckGroupLimit();

            _members.Add(student);
            return true;
        }

        public bool DeleteGroupMember(Student student_)
        {
            foreach (Student student in _members)
            {
                if (student.GetId().Equals(student_.GetId()))
                {
                    _members.Remove(student);
                    return true;
                }
            }

            return false;
        }

        public Student FindStudent(int id)
        {
            return DoFindStudent(id, null);
        }

        public Student FindStudent(string name)
        {
            return DoFindStudent(0, name);
        }

        public List<Student> GetGroupMembers()
        {
            return _members;
        }

        private Student DoFindStudent(int id, string name)
        {
            foreach (Student student in _members)
            {
                if (name == null && student.GetId().Equals(id))
                    return student;
                if (name != null && student.GetName().Equals(name))
                    return student;
            }

            return null;
        }

        private void CheckGroupLimit()
        {
            if (GetNumberOfStudents() >= 21)
                throw new IsuException("Too many students per group. 21 is max value");
        }
    }
}