// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;
using Reports.DAL.Entities;

namespace Reports.DAL.Tools.ReportTypes
{
    public class DefaultReport : IReportType
    {
        public Report MakeReport(List<EditLogMessage> edits, List<Employee> subordinates)
        {
            if (edits == null && subordinates == null)
                throw new ReportsException("both own edits and subordinates list cannot be nulls");

            List<Report> subordinatesReports = null;
            if (subordinates != null)
                subordinatesReports = subordinates.Select(subordinate => subordinate.Report).ToList();

            return new Report(edits, subordinatesReports);
        }
    }
}