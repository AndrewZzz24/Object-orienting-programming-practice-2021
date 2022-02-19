// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using Reports.DAL.Entities;

namespace Reports.DAL.Tools.ReportTypes
{
    public interface IReportType
    {
        Report MakeReport(List<EditLogMessage> edits, List<Employee> subordinates);
    }
}