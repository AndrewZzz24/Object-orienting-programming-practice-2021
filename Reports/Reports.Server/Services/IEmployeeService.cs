// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
using Reports.DAL.Entities;
using Reports.DAL.Tools.ReportTypes;

namespace Reports.Server.Services
{
    public interface IEmployeeService
    {
        Employee CreateNewEmployee(Employee newEmployee);

        EmployeeService DeleteEmployee(Employee employee);

        EmployeeService UpdateEmployee(Employee employee, string newName);

        Employee GetEmployeeById(Guid id);

        Employee GetEmployeeByName(string name);

        List<Employee> GetAllEmployees();

        Report MakeReport(IReportType reportType);

        EmployeeService SetSuperior(Employee employee, Employee superior);
    }
}