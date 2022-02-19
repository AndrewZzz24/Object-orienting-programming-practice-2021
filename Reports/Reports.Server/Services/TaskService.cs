// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
using Reports.DAL.Entities;
using Reports.DAL.Tools;
using Reports.DAL.Tools.ReportTypes;

namespace Reports.Server.Services
{
    public class TaskService : ITaskService
    {
        public TaskService()
        {
            Repository = new TasksRepository(null);
        }

        public TaskService(TasksRepository tasksRepository)
        {
            Repository = tasksRepository ?? throw new ReportsException("repository cannot be null");
        }

        public TasksRepository Repository
        {
            get;
            set;
        }

        public Task AddNewTask(string taskDetails)
        {
            Task newTask = Repository.AddNewTask(taskDetails);
            return newTask;
        }

        public Task FindTaskWithId(Guid id)
        {
            return Repository.FindTaskWithId(id);
        }

        public Task FindTaskWithDateCreation(DateTime dateTime)
        {
            return Repository.FindTaskWithDateCreation(dateTime);
        }

        public Task FindTaskWithLastChangeDate(DateTime dateTime)
        {
            return Repository.FindTaskWithLastChangeDate(dateTime);
        }

        public List<Task> FindTasksAssignedToEmployee(Employee employee)
        {
            return Repository.FindTasksAssignedToEmployee(employee);
        }

        public List<Task> FindTasksEditedByEmployee(Employee employee)
        {
            return Repository.FindTasksEditedByEmployee(employee);
        }

        public ITaskService ChangeTaskState(Task task, TaskState newTaskState)
        {
            Repository.ChangeTaskState(task, newTaskState);
            return this;
        }

        public ITaskService AddCommentToTask(Task task, string comment)
        {
            Repository.AddCommentToTask(task, comment);
            return this;
        }

        public ITaskService SetTaskAssigner(Task task, Employee newAssigner)
        {
            Repository.SetTaskAssigner(task, newAssigner);
            return this;
        }

        public List<Task> GetAllTasks()
        {
            return Repository.GetAllTasks();
        }

        public TaskService MakeEdit(Task task, Employee employee, EditLogMessage editLogMessage)
        {
            Repository.MakeEdit(task, employee, editLogMessage);

            if (employee.Report == null || employee.Report.State == ReportState.Opened)
                employee.Edits.Add(editLogMessage);

            return this;
        }
    }
}