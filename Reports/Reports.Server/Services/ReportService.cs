// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.IO;
using Newtonsoft.Json;
using Reports.DAL.Entities;
using Reports.DAL.Tools;
using Reports.DAL.Tools.ReportTypes;

namespace Reports.Server.Services
{
    public class ReportService : IReportService
    {
        public ReportService()
        {
            TaskService = new TaskService();
            EmployeeManager = new EmployeeService();
        }

        public ReportService(string employeeConfigSavePath, string tasksConfigSavePath)
        {
            EmployeeConfigSavePath = employeeConfigSavePath;
            TasksConfigSavePath = tasksConfigSavePath;

            TaskService = new TaskService();
            EmployeeManager = new EmployeeService();
        }

        public ReportService(
            string employeeConfigSavePath,
            string tasksConfigSavePath,
            EmployeeRepository employeeRepository,
            TasksRepository tasksRepository)
        {
            if (tasksRepository != null)
                TaskService = new TaskService(tasksRepository);
            else
            {
                TaskService = new TaskService();
            }

            if (employeeRepository != null)
                EmployeeManager = new EmployeeService(employeeRepository);
            else
            {
                EmployeeManager = new EmployeeService();
            }

            EmployeeConfigSavePath = employeeConfigSavePath;
            TasksConfigSavePath = tasksConfigSavePath;
        }

        public string EmployeeConfigSavePath
        {
            get;
        }

        public string TasksConfigSavePath
        {
            get;
        }

        public TaskService TaskService
        {
            get;
            private set;
        }

        public EmployeeService EmployeeManager
        {
            get;
            private set;
        }

        public Report MakeReport(IReportType reportType)
        {
            return EmployeeManager.MakeReport(reportType);
        }

        public ReportService DownloadState()
        {
            if (TasksConfigSavePath == null || EmployeeConfigSavePath == null)
                return this;

            var serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };

            EmployeeManager =
                JsonConvert.DeserializeObject<EmployeeService>(File.ReadAllText(EmployeeConfigSavePath),
                    serializerSettings);

            TaskService =
                JsonConvert.DeserializeObject<TaskService>(File.ReadAllText(TasksConfigSavePath), serializerSettings);

            return this;
        }

        public ReportService SaveState()
        {
            if (EmployeeConfigSavePath == null)
                return this;
            File.Create(EmployeeConfigSavePath).Close();
            var serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };
            File.WriteAllText(EmployeeConfigSavePath,
                JsonConvert.SerializeObject(EmployeeManager, Formatting.Indented, serializerSettings));

            if (TasksConfigSavePath == null)
                return this;
            File.Create(TasksConfigSavePath).Close();
            File.WriteAllText(TasksConfigSavePath,
                JsonConvert.SerializeObject(TaskService, Formatting.Indented, serializerSettings));

            return this;
        }
    }
}