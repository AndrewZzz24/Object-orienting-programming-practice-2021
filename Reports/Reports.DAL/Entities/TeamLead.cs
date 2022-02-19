// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Linq;
using Reports.DAL.Tools;
using Reports.DAL.Tools.ReportTypes;

namespace Reports.DAL.Entities
{
    public class TeamLead : Chief
    {
        public TeamLead()
        {
        }

        public TeamLead(string name, Chief superior) : base(name, superior)
        {
        }

        public override Report MakeReport(IReportType reportType)
        {
            if (Subordinates.Any(subordinate => (subordinate.Report == null || subordinate.Report.State == ReportState.Opened)))
            {
                throw new ReportsException(
                    "impossible to make a report now as not all the subordinates have not finished with their ones");
            }
            Report = reportType.MakeReport(null, Subordinates);
            Report.State = ReportState.Closed;
            return Report;
        }
    }
}