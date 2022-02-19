// @author Andrew Zmushko (andrewzmushko@gmail.com)

using Reports.DAL.Entities;
using Reports.DAL.Tools.ReportTypes;

namespace Reports.Server.Services
{
    public interface IReportService
    {
        Report MakeReport(IReportType reportType);

        TaskService TaskService
        {
            get;
        }

        EmployeeService EmployeeManager
        {
            get;
        }
    }
}