using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            //TODO: implement
            _isuService = new IsuService();
            _isuService.AddGroup(new GroupName("03", new CourseNumber(1), "M3"));
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Setup();

            var testGroupName = new GroupName("03", new CourseNumber(1), "M3");
            var testGroup = new Group(testGroupName);

            Student student = _isuService.AddStudent(testGroup, "Andrew");
            Assert.AreEqual(student.GetGroup(), testGroup);
            Assert.AreEqual(testGroup.GetGroupMembers().Contains(student), true);
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Setup();

            var testGroupName = new GroupName("03", new CourseNumber(1), "M3");
            var testGroup = new Group(testGroupName);

            for (int i = 0; i < 21; i++)
            {
                _isuService.AddStudent(testGroup, "Andrew" + i);
            }

            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddStudent(testGroup, "Andrew22");
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                var testGroupName = new GroupName("-2", new CourseNumber(1), "M3");
                var testGroup = new Group(testGroupName);
            });

            Assert.Catch<IsuException>(() =>
            {
                var testGroupName = new GroupName("2", new CourseNumber(-1), "M3");
                var testGroup = new Group(testGroupName);
            });

            Assert.Catch<IsuException>(() =>
            {
                var testGroupName = new GroupName("2", new CourseNumber(123), "M3");
                var testGroup = new Group(testGroupName);
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            var testGroup1 = new Group(new GroupName("03", new CourseNumber(2), "M3"));
            var testGroup2 = new Group(new GroupName("00", new CourseNumber(2), "M3"));

            Student student = _isuService.AddStudent(testGroup1, "Andrew");
            _isuService.ChangeStudentGroup(student, testGroup2);

            Assert.AreEqual(student.GetGroup(), testGroup2);
            Assert.AreEqual(testGroup1.GetNumberOfStudents(), 0);
            Assert.AreEqual(testGroup2.GetNumberOfStudents(), 1);
        }

        [Test]
        public void ValidArgumentWithConstructors_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                var courseNumber = new CourseNumber(-1);
            });
            Assert.Catch<IsuException>(() =>
            {
                var courseNumber = new CourseNumber(3);
                var groupName = new GroupName(null, courseNumber, "M3");
            });
        }
    }
}