// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using Isu;
using IsuExtra.Entities;
using IsuExtra.Tools;
using NUnit.Framework;
using Faculty = IsuExtra.Entities.Faculty;

namespace IsuExtra.Tests
{
    public class IsuUcpgTest
    {
        [Test]
        public void AddNewUcpg()
        {
            var isuManager = new IsuUcpgService();
            UnitedCoursePreparationGroup ucpg = isuManager.AddUcpg("Photonics", new Faculty("FTF", "U"), 3);
            Assert.AreEqual(3, isuManager.GetCourseParallels(ucpg).Count);
        }

        [Test]
        public void AssignStudentToCertainUcpg()
        {
            var isuService = new IsuService();
            var isuUcpgService = new IsuUcpgService();
            Group group = isuService.AddGroup(new GroupName("3", new CourseNumber(2), "ITaP"));
            Student student = isuService.AddStudent(group, "Andrew");
            UnitedCoursePreparationGroup ucpg = isuUcpgService.AddUcpg("Photonics", new Faculty("FTF", "U"), 3);
            UcpgStudent ucpgStudent = isuUcpgService.AddUcpgStudent(student);
            isuUcpgService.AddStudentToUcpg(ucpgStudent, ucpg, 3);
            var list = new List<UcpgStudent>()
            {
                ucpgStudent
            };
            Assert.AreEqual(list, isuUcpgService.GetUcpgParallelStudentList(ucpg, 3));
        }


        [Test]
        public void RemoveStudentFromUcpg()
        {
            var isuService = new IsuService();
            var isuUcpgService = new IsuUcpgService();
            Group group = isuService.AddGroup(new GroupName("3", new CourseNumber(2), "ITaP"));
            Student student1 = isuService.AddStudent(group, "Andrew");
            Student student2 = isuService.AddStudent(group, "Alexandr");
            Student student3 = isuService.AddStudent(group, "Raphael");
            Student student4 = isuService.AddStudent(group, "Donatello");
            UnitedCoursePreparationGroup ucpg1 = isuUcpgService.AddUcpg("Photonics", new Faculty("FTF", "U"), 3);
            UnitedCoursePreparationGroup ucpg2 = isuUcpgService.AddUcpg("Computer Technologies", new Faculty("ITaPCT", "M"), 3);
            UcpgStudent ucpgStudent1 = isuUcpgService.AddUcpgStudent(student1);
            UcpgStudent ucpgStudent2 = isuUcpgService.AddUcpgStudent(student2);
            UcpgStudent ucpgStudent3 = isuUcpgService.AddUcpgStudent(student3);
            UcpgStudent ucpgStudent4 = isuUcpgService.AddUcpgStudent(student4);
            isuUcpgService.AddStudentToUcpg(ucpgStudent1, ucpg1, 3);
            isuUcpgService.AddStudentToUcpg(ucpgStudent2, ucpg1, 3);
            isuUcpgService.AddStudentToUcpg(ucpgStudent3, ucpg1, 3);
            isuUcpgService.AddStudentToUcpg(ucpgStudent4, ucpg2, 1);
            var list1 = new List<UcpgStudent>()
            {
                ucpgStudent1,
                ucpgStudent2,
                ucpgStudent3,
            };
            var list2 = new List<UcpgStudent>()
            {
                ucpgStudent4,
            };
            Assert.AreEqual(list1, isuUcpgService.GetUcpgParallelStudentList(ucpg1, 3));
            Assert.AreEqual(list2, isuUcpgService.GetUcpgParallelStudentList(ucpg2, 1));
            Assert.Catch<IsuUcpgException>(() =>
            {
                Group group1 = isuService.AddGroup(new GroupName("3", new CourseNumber(2), "ITaPCT"));
                Student student5 = isuService.AddStudent(group1, "Donatello");
                var ucpgStudent5 = isuUcpgService.AddUcpgStudent(student5);
                isuUcpgService.AddStudentToUcpg(ucpgStudent5, ucpg2, 1);
            });
        }

        [Test]
        public void GetCourseParallels()
        {
            var isuService = new IsuService();
            var isuUcpgService = new IsuUcpgService();
            Group group = isuService.AddGroup(new GroupName("3", new CourseNumber(2), "ITaP"));
            Student student1 = isuService.AddStudent(group, "Andrew");
            Student student2 = isuService.AddStudent(group, "Alexandr");
            Student student3 = isuService.AddStudent(group, "Raphael");
            Student student4 = isuService.AddStudent(group, "Donatello");
            UnitedCoursePreparationGroup ucpg1 = isuUcpgService.AddUcpg("Photonics", new Faculty("FTF", "U"), 3);
            UnitedCoursePreparationGroup ucpg2 = isuUcpgService.AddUcpg("Computer Technologies",  new Faculty("ITaPCT", "M"), 3);
            UcpgStudent ucpgStudent1 = isuUcpgService.AddUcpgStudent(student1);
            UcpgStudent ucpgStudent2 = isuUcpgService.AddUcpgStudent(student2);
            UcpgStudent ucpgStudent3 = isuUcpgService.AddUcpgStudent(student3);
            UcpgStudent ucpgStudent4 = isuUcpgService.AddUcpgStudent(student4);
            isuUcpgService.AddStudentToUcpg(ucpgStudent1, ucpg1, 3);
            isuUcpgService.AddStudentToUcpg(ucpgStudent2, ucpg1, 2);
            isuUcpgService.AddStudentToUcpg(ucpgStudent3, ucpg1, 3);
            isuUcpgService.AddStudentToUcpg(ucpgStudent4, ucpg2, 3);
            Assert.AreEqual(3, isuUcpgService.GetCourseParallels(ucpg1).Count);
            Assert.AreEqual(3, isuUcpgService.GetCourseParallels(ucpg2).Count);
            Assert.AreEqual(19, isuUcpgService.GetCourseParallels(ucpg1)[2].FreePlaces);
            Assert.AreEqual(20, isuUcpgService.GetCourseParallels(ucpg2)[2].FreePlaces);
        }

        [Test]
        public void GetExactParallelStudentList()
        {
            var isuService = new IsuService();
            var isuUcpgService = new IsuUcpgService();
            Group group = isuService.AddGroup(new GroupName("3", new CourseNumber(2), "ITaP"));
            Student student1 = isuService.AddStudent(group, "Andrew");
            Student student2 = isuService.AddStudent(group, "Alexandr");
            Student student3 = isuService.AddStudent(group, "Raphael");
            Student student4 = isuService.AddStudent(group, "Donatello");
            UnitedCoursePreparationGroup ucpg1 = isuUcpgService.AddUcpg("Photonics", new Faculty("FTF", "U"), 3);
            UnitedCoursePreparationGroup ucpg2 = isuUcpgService.AddUcpg("Computer Technologies",  new Faculty("ITaPCT", "M"), 3);
            UcpgStudent ucpgStudent1 = isuUcpgService.AddUcpgStudent(student1);
            UcpgStudent ucpgStudent2 = isuUcpgService.AddUcpgStudent(student2);
            UcpgStudent ucpgStudent3 = isuUcpgService.AddUcpgStudent(student3);
            UcpgStudent ucpgStudent4 = isuUcpgService.AddUcpgStudent(student4);
            isuUcpgService.AddStudentToUcpg(ucpgStudent1, ucpg1, 3);
            isuUcpgService.AddStudentToUcpg(ucpgStudent2, ucpg1, 2);
            isuUcpgService.AddStudentToUcpg(ucpgStudent3, ucpg1, 3);
            isuUcpgService.AddStudentToUcpg(ucpgStudent4, ucpg2, 3);
            var list = new List<UcpgStudent>()
            {
                ucpgStudent1,
                ucpgStudent3,
            };
            Assert.AreEqual(list, isuUcpgService.GetUcpgParallelStudentList(ucpg1, 3));
        }

        [Test]
        public void GetUnassignedToUcpgStudents()
        {
            var isuService = new IsuService();
            var isuUcpgService = new IsuUcpgService();
            Group group = isuService.AddGroup(new GroupName("3", new CourseNumber(2), "ITaP"));
            Student student1 = isuService.AddStudent(group, "Andrew");
            Student student2 = isuService.AddStudent(group, "Alexandr");
            Student student3 = isuService.AddStudent(group, "Raphael");
            Student student4 = isuService.AddStudent(group, "Donatello");
            UnitedCoursePreparationGroup ucpg1 = isuUcpgService.AddUcpg("Photonics", new Faculty("FTF", "U"), 3);
            UnitedCoursePreparationGroup ucpg2 = isuUcpgService.AddUcpg("Computer Technologies",  new Faculty("ITaPCT", "M"), 3);
            UcpgStudent ucpgStudent1 = isuUcpgService.AddUcpgStudent(student1);
            UcpgStudent ucpgStudent2 = isuUcpgService.AddUcpgStudent(student2);
            UcpgStudent ucpgStudent3 = isuUcpgService.AddUcpgStudent(student3);
            UcpgStudent ucpgStudent4 = isuUcpgService.AddUcpgStudent(student4);
            isuUcpgService.AddStudentToUcpg(ucpgStudent1, ucpg1, 3);
            isuUcpgService.AddStudentToUcpg(ucpgStudent2, ucpg1, 2);
            var list = new List<UcpgStudent>()
            {
                ucpgStudent3,
                ucpgStudent4
            };
            Assert.AreEqual(list, isuUcpgService.GetUnsignedToUcpgStudentsInGroup(group));
        }
    }
}