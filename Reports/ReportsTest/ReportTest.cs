using NUnit.Framework;
using Reports.DAL.Entities;
using Reports.DAL.Tools;
using Reports.DAL.Tools.ReportTypes;
using Reports.Server.Services;

namespace ReportsTest
{
    public class Tests
    {
        [Test]
        public void AddTaskAddEmployees_GetReport()
        {
            var reportManager = new ReportService(null, null);

            var teamLead = new TeamLead("Bodgan", null);
            var chief = new Chief("Vitaliy", teamLead);
            var defaultEmployee = new Employee("Ilya", chief);

            Task task1 = reportManager.TaskService.AddNewTask("Press kachat");
            Task task2 = reportManager.TaskService.AddNewTask("Anzhumaniya");

            Assert.AreEqual(2, reportManager.TaskService.GetAllTasks().Count);
            Assert.AreEqual(chief, defaultEmployee.Superior);

            reportManager.EmployeeManager.CreateNewEmployee(teamLead);
            reportManager.EmployeeManager.TeamLead = teamLead;
            reportManager.EmployeeManager.CreateNewEmployee(chief);
            reportManager.EmployeeManager.CreateNewEmployee(defaultEmployee);

            reportManager.TaskService.SetTaskAssigner(task1, defaultEmployee);
            reportManager.TaskService.SetTaskAssigner(task2, chief);

            Assert.AreEqual(3, reportManager.EmployeeManager.GetAllEmployees().Count);

            reportManager.TaskService.MakeEdit(task1, defaultEmployee,
                new EditLogMessage(defaultEmployee, "press sdelan 3 podhoda"));

            Assert.Catch<ReportsException>(() =>
            {
                chief.MakeReport(new DefaultReport());
            });
            defaultEmployee.MakeReport(new DefaultReport());

            Assert.Catch<ReportsException>(() =>
            {
                teamLead.MakeReport(new DefaultReport());
            });

            chief.MakeReport(new DefaultReport());
            Assert.AreEqual(1, teamLead.MakeReport(new DefaultReport()).SubordinatesReports.Count);
        }

        [Test]
        public void AddTasksEditTasks_FindTasksByDifferentFilters()
        {
            var reportManager = new ReportService(null, null);

            var teamLead = new TeamLead("Bodgan", null);
            var chief = new Chief("Vitaliy", teamLead);
            var defaultEmployee = new Employee("Ilya", chief);

            Task task1 = reportManager.TaskService.AddNewTask("Press kachat");
            Task task2 = reportManager.TaskService.AddNewTask("Anzhumaniya");
            Task task3 = reportManager.TaskService.AddNewTask("press");
            Task task4 = reportManager.TaskService.AddNewTask("otdih");

            Assert.AreEqual(4, reportManager.TaskService.GetAllTasks().Count);
            Assert.AreEqual(chief, defaultEmployee.Superior);

            reportManager.EmployeeManager.CreateNewEmployee(teamLead);
            reportManager.EmployeeManager.CreateNewEmployee(chief);
            reportManager.EmployeeManager.CreateNewEmployee(defaultEmployee);
            reportManager.EmployeeManager.TeamLead = teamLead;

            reportManager.TaskService.SetTaskAssigner(task1, defaultEmployee);
            reportManager.TaskService.SetTaskAssigner(task2, chief);
            reportManager.TaskService.SetTaskAssigner(task3, chief);
            reportManager.TaskService.SetTaskAssigner(task4, defaultEmployee);

            Assert.AreEqual(2, reportManager.TaskService.FindTasksAssignedToEmployee(chief).Count);
            Assert.AreEqual(2, reportManager.TaskService.FindTasksAssignedToEmployee(defaultEmployee).Count);
            Assert.AreEqual(0, reportManager.TaskService.FindTasksEditedByEmployee(chief).Count);

            reportManager.TaskService.MakeEdit(task2, chief, new EditLogMessage(chief, "anzhumaniya sdelani"));

            Assert.AreEqual(1, reportManager.TaskService.FindTasksEditedByEmployee(chief).Count);

            reportManager.TaskService.MakeEdit(task3, chief, new EditLogMessage(chief, "press sdelan"));

            Assert.AreEqual(2, reportManager.TaskService.FindTasksEditedByEmployee(chief).Count);
        }

        [Test]
        public void AddEmployees_CheckTheirCorrectness()
        {
            var reportManager = new ReportService(null, null);

            var teamLead = new TeamLead("Bodgan", null);
            var chief = new Chief("Vitaliy", teamLead);
            var defaultEmployee = new Employee("Ilya", chief);

            Task task1 = reportManager.TaskService.AddNewTask("Press kachat");
            Task task2 = reportManager.TaskService.AddNewTask("Anzhumaniya");
            Task task3 = reportManager.TaskService.AddNewTask("press");

            Assert.AreEqual(3, reportManager.TaskService.GetAllTasks().Count);
            Assert.AreEqual(chief, defaultEmployee.Superior);

            reportManager.EmployeeManager.CreateNewEmployee(teamLead);
            reportManager.EmployeeManager.CreateNewEmployee(chief);
            reportManager.EmployeeManager.CreateNewEmployee(defaultEmployee);
            reportManager.EmployeeManager.TeamLead = teamLead;

            reportManager.TaskService.SetTaskAssigner(task1, defaultEmployee);
            reportManager.TaskService.SetTaskAssigner(task2, chief);

            Assert.AreEqual(defaultEmployee, task1.Assigner);
            Assert.AreEqual(chief, task2.Assigner);
            Assert.AreEqual(null, task3.Assigner);

            Assert.AreEqual(null, teamLead.Superior);
            Assert.AreEqual(1, teamLead.Subordinates.Count);
            Assert.AreEqual(chief, reportManager.EmployeeManager.GetEmployeeByName("Vitaliy"));

            Assert.AreEqual(defaultEmployee, reportManager.EmployeeManager.GetEmployeeById(defaultEmployee.Id));
        }
    }
}