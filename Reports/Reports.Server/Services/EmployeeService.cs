// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
using Reports.DAL.Entities;
using Reports.DAL.Tools;
using Reports.DAL.Tools.ReportTypes;

namespace Reports.Server.Services
{
    public class EmployeeService : IEmployeeService
    {
        public EmployeeService(EmployeeRepository repository)
        {
            TeamLead = null;
            Repository = repository ?? throw new ReportsException("repository cannot be null");
        }

        public EmployeeService()
        {
            TeamLead = null;
            Repository = new EmployeeRepository(null);
        }

        public TeamLead TeamLead
        {
            get;
            set;
        }

        public EmployeeRepository Repository
        {
            get;
            set;
        }

        public Employee CreateNewEmployee(Employee newEmployee)
        {
            Repository.AddEmployee(newEmployee);
            return newEmployee;
        }

        public EmployeeService DeleteEmployee(Employee employee)
        {
            Repository.DeleteEmployee(employee);
            return this;
        }

        public EmployeeService UpdateEmployee(Employee employee, string newName)
        {
            Repository.UpdateEmployee(employee, newName);
            return this;
        }

        public Employee GetEmployeeById(Guid id)
        {
            return Repository.GetEmployeeById(id);
        }

        public Employee GetEmployeeByName(string name)
        {
            return Repository.GetEmployeeByName(name);
        }

        public List<Employee> GetAllEmployees()
        {
            return Repository.GetAllEmployees();
        }

        public Report MakeReport(IReportType reportType)
        {
            return TeamLead.MakeReport(reportType);
        }

        public EmployeeService SetSuperior(Employee employee, Employee superior)
        {
            Repository.SetSuperior(employee, superior);
            return this;
        }
    }
}