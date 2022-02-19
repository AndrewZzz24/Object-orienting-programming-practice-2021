// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using Reports.DAL.Tools;
using Reports.DAL.Tools.ReportTypes;

namespace Reports.DAL.Entities
{
    public class Report
    {
        public Report(List<EditLogMessage> edits, List<Report> subordinatesReports)
        {
            if (edits == null && subordinatesReports == null)
                throw new ReportsException("Both own edits and subordinates reports cannot be nulls");

            Edits = edits;
            SubordinatesReports = subordinatesReports;
            State = ReportState.Opened;
        }

        public List<Report> SubordinatesReports
        {
            get;
            init;
        }

        public List<EditLogMessage> Edits
        {
            get;
            init;
        }

        public ReportState State
        {
            get;
            internal set;
        }
    }
}