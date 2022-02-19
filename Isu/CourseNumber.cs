using Isu.Tools;

namespace Isu
{
    public class CourseNumber
    {
        private const int MaxCourseValue = 8;
        private const int MinCourseValue = 1;
        private int _courseNum;

        public CourseNumber(int courseNum)
        {
            CheckNumCorrectness(courseNum);
            _courseNum = courseNum;
        }

        public static int GetMaxCourseValue()
        {
            return MaxCourseValue;
        }

        public static int GetMinCourseValue()
        {
            return MinCourseValue;
        }

        public int GetCourseNum()
        {
            return _courseNum;
        }

        private void CheckNumCorrectness(int i)
        {
            if (i > MaxCourseValue || i < MinCourseValue)
                throw new IsuException("Invalid course number : " + i);
        }
    }
}