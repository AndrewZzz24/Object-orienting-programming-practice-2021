// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Reports.DAL.Tools;

namespace Reports.DAL.Entities
{
    public class TasksRepository
    {
        public TasksRepository(string path)
        {
            Tasks = new List<Task>();
            ConfigPath = path;
            DownloadState();
        }

        public TasksRepository()
        {
            Tasks = new List<Task>();
            ConfigPath = "./tasks.json";
            DownloadState();
        }

        public List<Task> Tasks
        {
            get;
            set;
        }

        public string ConfigPath
        {
            get;
            set;
        }

        public Task AddNewTask(string taskDetails)
        {
            var newTask = new Task(taskDetails);
            Tasks.Add(newTask);
            SaveState();
            return newTask;
        }

        public Task FindTaskWithId(Guid id)
        {
            foreach (Task task in Tasks.Where(task => task.Id.ToString().Equals(id.ToString())))
            {
                return task;
            }

            throw new ReportsException("No task with such id");
        }

        public Task FindTaskWithDateCreation(DateTime dateTime)
        {
            return FindTaskWithDate(dateTime);
        }

        public Task FindTaskWithLastChangeDate(DateTime dateTime)
        {
            return FindTaskWithDate(dateTime);
        }

        public List<Task> FindTasksAssignedToEmployee(Employee employee)
        {
            return Tasks.Where(task => task.Assigner == employee).ToList();
        }

        public List<Task> FindTasksEditedByEmployee(Employee employee)
        {
            return Tasks.Where(task => task.EditLog.Any(log => log.Editor == employee)).ToList();
        }

        public TasksRepository ChangeTaskState(Task task, TaskState newTaskState)
        {
            task.TaskState = newTaskState;
            SaveState();
            return this;
        }

        public TasksRepository AddCommentToTask(Task task, string comment)
        {
            task.Comments.Add(comment ?? throw new ReportsException("new assigner cannot be null"));
            SaveState();
            return this;
        }

        public TasksRepository SetTaskAssigner(Task task, Employee newAssigner)
        {
            if (task.TaskState == TaskState.Resolved)
                throw new ReportsException("Impossible to set task assigner as the task is already resolved");

            task.Assigner = newAssigner ?? throw new ReportsException("new assigner cannot be null");
            SaveState();
            return this;
        }

        public List<Task> GetAllTasks()
        {
            return Tasks;
        }

        public TasksRepository MakeEdit(Task task, Employee employee, EditLogMessage editLogMessage)
        {
            if (task.TaskState == TaskState.Resolved || task.Assigner != employee)
                throw new ReportsException("Impossible to make edit to task");

            task.EditLog.Add(editLogMessage ?? throw new ReportsException("edit log message cannot be null"));
            SaveState();

            return this;
        }

        private Task FindTaskWithDate(DateTime dateTime)
        {
            foreach (Task task in Tasks.Where(task => task.CreationDateTime == dateTime))
            {
                return task;
            }

            throw new ReportsException("No task with such id");
        }

        private TasksRepository DownloadState()
        {
            if (ConfigPath == null)
                return this;

            var serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };

            Tasks =
                JsonConvert.DeserializeObject<List<Task>>(File.ReadAllText(ConfigPath),
                    serializerSettings);

            if (Tasks == null)
                Tasks = new List<Task>();
            return this;
        }

        private TasksRepository SaveState()
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
                JsonConvert.SerializeObject(Tasks, Formatting.Indented, serializerSettings));

            return this;
        }
    }
}