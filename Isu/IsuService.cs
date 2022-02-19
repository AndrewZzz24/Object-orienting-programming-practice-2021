using System.Collections.Generic;
using Isu.Services;

namespace Isu
{
    public class IsuService : IIsuService
    {
        private Faculty itapFaculty = new Faculty("ITaP");

        public Group AddGroup(GroupName name)
        {
            var group = new Group(name);
            itapFaculty.AddGroup(group);
            return group;
        }

        public Student AddStudent(Group @group, string name)
        {
            var student = new Student(group, name, itapFaculty.NextId());
            group.AddToGroup(student);
            return student;
        }

        public Student GetStudent(int id)
        {
            return itapFaculty.FindStudent(id);
        }

        public Student FindStudent(string name)
        {
            return itapFaculty.FindStudent(name);
        }

        public List<Student> FindStudents(GroupName groupName)
        {
            return itapFaculty.FindGroup(groupName).GetGroupMembers();
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            return itapFaculty.FindSameCourseStudents(courseNumber);
        }

        public Group FindGroup(GroupName groupName)
        {
            return itapFaculty.FindGroup(groupName);
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return itapFaculty.FindSameCourseStudentsAsGroupList(courseNumber);
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            itapFaculty.ChangeStudentGroup(student, newGroup);
        }
    }
}