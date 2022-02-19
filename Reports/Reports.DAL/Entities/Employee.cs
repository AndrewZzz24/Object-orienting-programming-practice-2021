using System;
using System.Collections.Generic;
using Reports.DAL.Tools.ReportTypes;

namespace Reports.DAL.Entities
{
    public class Employee
    {
        public Employee()
        {
        }

        public Guid Id
        {
            get;
            init;
        }

        public string Name
        {
            get;
            set;
        }

        public Employee(string name, Chief superior)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Name is invalid");
            }

            Id = Guid.NewGuid();
            Name = name;
            Superior = superior;
            superior?.Subordinates.Add(this);
            Edits = new List<EditLogMessage>();
        }

        public Chief Superior
        {
            get;
            set;
        }

        public List<EditLogMessage> Edits
        {
            get;
            init;
        }

        public Report Report
        {
            get;
            protected set;
        }

        public virtual Report MakeReport(IReportType reportType)
        {
            Report = reportType.MakeReport(Edits, null);
            Report.State = ReportState.Closed;
            return Report;
        }
    }
}