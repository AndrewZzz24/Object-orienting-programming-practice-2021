// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
using Reports.DAL.Entities;
using Reports.DAL.Tools;

namespace Reports.Server.Services
{
    public interface ITaskService
    {
        Task FindTaskWithId(Guid id);

        Task FindTaskWithDateCreation(DateTime dateTime);

        Task FindTaskWithLastChangeDate(DateTime dateTime);
        
        List<Task> FindTasksAssignedToEmployee(Employee employee);
        
        List<Task> FindTasksEditedByEmployee(Employee employee);
        
        Task AddNewTask(string taskDetails);

        ITaskService ChangeTaskState(Task task, TaskState newTaskState);

        ITaskService AddCommentToTask(Task task, string comment);

        ITaskService SetTaskAssigner(Task task, Employee newAssigner);
        
    }
}