// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using System.Linq;
using Reports.DAL.Tools;
using Reports.DAL.Tools.ReportTypes;

namespace Reports.DAL.Entities
{
    public class Chief : Employee
    {
        public Chief()
        {
        }

        public Chief(string name, Chief superior) : base(name, superior)
        {
            Subordinates = new List<Employee>();
        }

        public List<Employee> Subordinates
        {
            get;
        }

        public Chief AddSubordinate(Employee employee)
        {
            Subordinates.Add(employee ?? throw new ReportsException("employee cannot be null"));
            return this;
        }

        public Chief RemoveSubordinate(Employee employee)
        {
            Subordinates.Remove(employee ?? throw new ReportsException("employee cannot be null"));
            return this;
        }

        public override Report MakeReport(IReportType reportType)
        {
            if (Subordinates.Any(subordinate => (subordinate.Report == null || subordinate.Report.State == ReportState.Opened)))
            {
                throw new ReportsException(
                    "impossible to make a report now as not all the subordinates have not finished with their ones");
            }

            Report = reportType.MakeReport(Edits, Subordinates);
            Report.State = ReportState.Closed;
            return Report;
        }
    }
}