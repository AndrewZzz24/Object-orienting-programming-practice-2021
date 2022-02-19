using System.Collections.Generic;
using Isu.Tools;

namespace Isu
{
    public class Faculty
    {
        private string _name;
        private Dictionary<int, List<Group>> courseGroupsDict = new Dictionary<int, List<Group>>();
        private int _lastId;

        public Faculty(string name)
        {
            for (int courseNum = CourseNumber.GetMinCourseValue();
                courseNum <= CourseNumber.GetMaxCourseValue();
                courseNum++)
            {
                courseGroupsDict.Add(courseNum, new List<Group>());
            }

            _name = name ?? throw new IsuException("Invalid name, it cannot be null");
            _lastId = 0;
        }

        public void AddGroup(Group @group)
        {
            courseGroupsDict[group.GetCourseNum()].Add(group);
        }

        public int NextId()
        {
            return ++_lastId;
        }

        public int GetLastId()
        {
            return _lastId;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            student.GetGroup().DeleteGroupMember(student);
            newGroup.AddToGroup(student);
            student.SetGroup(newGroup);
        }

        public Student FindStudent(int id)
        {
            return DoFindStudent(id, null);
        }

        public Student FindStudent(string name)
        {
            return DoFindStudent(0, name);
        }

        public Group FindGroup(GroupName groupName)
        {
            return DoFindGroup(groupName);
        }

        public List<Group> FindSameCourseStudentsAsGroupList(CourseNumber courseNumber)
        {
            return courseGroupsDict[courseNumber.GetCourseNum()];
        }

        public List<Student> FindSameCourseStudents(CourseNumber courseNumber)
        {
            var res = new List<Student>();
            bool success =
                courseGroupsDict.TryGetValue(courseNumber.GetCourseNum(), out List<Group> currentCourseGroupList);

            if (!success || currentCourseGroupList != null)
                throw new IsuException("Invalid Course Number when finding same course students");

            foreach (Group group in currentCourseGroupList)
            {
                res.AddRange(@group.GetGroupMembers());
            }

            return res;
        }

        private Student DoFindStudent(int id_, string name_)
        {
            foreach (List<Group> course in courseGroupsDict.Values)
            {
                foreach (Group group in course)
                {
                    Student student;

                    if (name_ != null)
                        student = group.FindStudent(name_);
                    else
                        student = @group.FindStudent(id_);

                    if (student != null)
                        return student;
                }
            }

            return null;
        }

        private Group DoFindGroup(GroupName groupName)
        {
            foreach (List<Group> course in courseGroupsDict.Values)
            {
                foreach (Group group in course)
                {
                    if (group.GetGroupName().Equals(groupName))
                        return group;
                }
            }

            return null;
        }
    }
}