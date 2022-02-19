// @author Andrew Zmushko (andrewzmushko@gmail.com)

namespace IsuExtra.Entities
{
    public class Faculty
    {
        public Faculty(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public string Name
        {
            get;
        }

        public string Code
        {
            get;
        }

        public UnitedCoursePreparationGroup UCPG
        {
            get;
        }
    }
}