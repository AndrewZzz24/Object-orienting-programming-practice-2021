using Isu.Tools;

namespace Isu
{
    public class GroupName
    {
        private CourseNumber courseNumber;
        private string groupNumber;

        public GroupName(string groupNumber, CourseNumber courseNumber, string faculty)
        {
            CheckGroupNumberCorrectness(groupNumber);
            this.groupNumber = groupNumber;
            this.courseNumber = courseNumber;
            Faculty = faculty;
        }

        public string Faculty
        {
            get;
        }

        public override string ToString()
        {
            return Faculty + courseNumber + groupNumber;
        }

        public string GetFaculty()
        {
            return Faculty;
        }

        public int GetGroupNumber()
        {
            return int.Parse(groupNumber);
        }

        public CourseNumber GetCourseNumber()
        {
            return courseNumber;
        }

        private void CheckGroupNumberCorrectness(string s)
        {
            if (s == null)
                throw new IsuException("Grop Number cannot be null");

            bool result = int.TryParse(s, out int tmpGroupNum);

            if (!result || tmpGroupNum < 0)
                throw new IsuException("Invalid Group Number : " + s);
        }
    }
}