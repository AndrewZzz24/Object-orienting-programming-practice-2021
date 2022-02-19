// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Reports.DAL.Tools;

namespace Reports.DAL.Entities
{
    public class EmployeeRepository
    {
        public EmployeeRepository(string configPath)
        {
            AllEmployees = new List<Employee>();
            ConfigPath = configPath;
            DownloadState();
        }

        public EmployeeRepository()
        {
            AllEmployees = new List<Employee>();
            ConfigPath = "./empls.json";
            DownloadState();
        }

        public List<Employee> AllEmployees
        {
            get;
            set;
        }

        public string ConfigPath
        {
            get;
            set;
        }

        public Employee AddEmployee(Employee newEmployee)
        {
            AllEmployees.Add(newEmployee);
            SaveState();
            return newEmployee;
        }

        public EmployeeRepository DeleteEmployee(Employee employee)
        {
            if (AllEmployees.Contains(employee ?? throw new ReportsException("Employee cannot be null")))
                AllEmployees.Remove(employee);
            else
            {
                throw new ReportsException("There is no such employee in system");
            }

            SaveState();

            return this;
        }

        public EmployeeRepository UpdateEmployee(Employee employee, string newName)
        {
            employee.Name = newName ?? throw new ReportsException("new name cannot be null");
            SaveState();
            return this;
        }

        public Employee GetEmployeeById(Guid id)
        {
            foreach (Employee employee in AllEmployees.Where(employee => employee.Id == id))
            {
                return employee;
            }

            throw new ReportsException("No employee with such id");
        }

        public Employee GetEmployeeByName(string name)
        {
            foreach (Employee employee in AllEmployees.Where(employee => employee.Name.Equals(name)))
            {
                return employee;
            }

            throw new ReportsException("no employee with such name");
        }

        public List<Employee> GetAllEmployees()
        {
            return AllEmployees;
        }

        public EmployeeRepository SetSuperior(Employee employee, Employee superior)
        {
            if (employee == null || superior == null)
                throw new ReportsException("null parameter in SetSuperior func");

            var tmp = (Chief) (superior);
            employee.Superior = tmp;
            tmp.Subordinates.Add(employee);
            SaveState();

            return this;
        }

        public EmployeeRepository DownloadState()
        {
            if (ConfigPath == null)
                return this;

            var serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };

            AllEmployees =
                JsonConvert.DeserializeObject<List<Employee>>(File.ReadAllText(ConfigPath),
                    serializerSettings);

            if (AllEmployees == null)
                AllEmployees = new List<Employee>();
            return this;
        }

        public EmployeeRepository SaveState()
        {
            if (ConfigPath == null)
                return this;

            File.Create(ConfigPath).Close();
            var serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };
            File.WriteAllText(ConfigPath,
                JsonConvert.SerializeObject(AllEmployees, Formatting.Indented, serializerSettings));

            return this;
        }
    }
}