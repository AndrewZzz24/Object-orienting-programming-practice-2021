// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using Isu;
using IsuExtra.Entities;
using Faculty = IsuExtra.Entities.Faculty;

namespace IsuExtra.Services
{
    public interface IIsuUcpgService
    {
        UnitedCoursePreparationGroup AddUcpg(string name, Faculty faculty, int numOfParallels);
        void AddStudentToUcpg(UcpgStudent student, UnitedCoursePreparationGroup ucpg, int parallel);
        void RemoveStudentFromUcpg(UcpgStudent student, UnitedCoursePreparationGroup ucpg);
        List<StudentParallel> GetCourseParallels(UnitedCoursePreparationGroup ucpg);
        List<UcpgStudent> GetUcpgParallelStudentList(UnitedCoursePreparationGroup ucpg, int parallel);
        List<UcpgStudent> GetUnsignedToUcpgStudentsInGroup(Group @group);
    }
}