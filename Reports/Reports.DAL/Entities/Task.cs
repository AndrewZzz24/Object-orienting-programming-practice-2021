// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
using Reports.DAL.Tools;

namespace Reports.DAL.Entities
{
    public class Task
    {
        public Task()
        {
        }

        public Task(string taskDetails)
        {
            TaskDetails = taskDetails ?? throw new ReportsException("Task details cannot be null");
            Assigner = null;
            Id = Guid.NewGuid();
            Console.WriteLine(Id);
            CreationDateTime = DateTime.Now;
            LastChangeDateTime = CreationDateTime;
            EditLog = new List<EditLogMessage>();
            Comments = new List<string>();
            TaskState = TaskState.Open;
        }

        public Guid Id
        {
            get;
            init;
        }

        public DateTime CreationDateTime
        {
            get;
            init;
        }

        public DateTime LastChangeDateTime
        {
            get;
            init;
        }

        public TaskState TaskState
        {
            get;
            set;
        }

        public List<string> Comments
        {
            get;
            init;
        }

        public Employee Assigner
        {
            get;
            set;
        }

        public string TaskDetails
        {
            get;
            init;
        }

        public List<EditLogMessage> EditLog
        {
            get;
            init;
        }
    }
}