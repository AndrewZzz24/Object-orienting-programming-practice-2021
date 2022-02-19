using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Reports.DAL.Entities;
using Reports.DAL.Tools;
using Reports.DAL.Tools.ReportTypes;
using Reports.Server.Services;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly IReportService _service;

        public EmployeeController(IReportService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        public Employee CreateNewEmployee([FromQuery] string name, [FromQuery] string position)
        {
            if (name == null || position == null)
                throw new ReportsException("Name cannot be null");

            position = position.ToLower().Replace(" ", "");

            switch (position)
            {
                case "teamlead":
                    return _service.EmployeeManager.CreateNewEmployee(new TeamLead(name, null));
                case "chief":
                    return _service.EmployeeManager.CreateNewEmployee(new Chief(name, null));
                case "employee":
                    return _service.EmployeeManager.CreateNewEmployee(new Employee(name, null));
                default:
                    throw new ReportsException("unknown position");
            }
        }

        [HttpGet]
        [Route("find-by-id")]
        public IActionResult FindById([FromQuery] Guid id)
        {
            if (id != Guid.Empty)
            {
                Employee result = _service.EmployeeManager.GetEmployeeById(id);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return StatusCode((int) HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Route("find-by-name")]
        public IActionResult FindByName([FromQuery] string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Employee result = _service.EmployeeManager.GetEmployeeByName(name);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return StatusCode((int) HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Route("get-all-employees")]
        public IActionResult GetAllEmployees()
        {
            return Ok(_service.EmployeeManager.GetAllEmployees());
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult DeleteEmployee([FromQuery] Guid id)
        {
            return Ok(_service.EmployeeManager.DeleteEmployee(_service.EmployeeManager.GetEmployeeById(id)));
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateEmployee([FromQuery] Guid employeeId, [FromQuery] string newName)
        {
            return Ok(_service.EmployeeManager.UpdateEmployee(_service.EmployeeManager.GetEmployeeById(employeeId),
                newName));
        }

        [HttpPut]
        [Route("make-report")]
        public IActionResult MakeReport([FromQuery] Guid employeeId, string reportType)
        {
            if (employeeId == Guid.Empty)
                throw new ReportsException("guid cannot be empty");
            switch (reportType)
            {
                case "weekly":
                    return Ok(_service.EmployeeManager.MakeReport(new WeeklyReport()));
                case "daily":
                    return Ok(_service.EmployeeManager.MakeReport(new DailyReport()));
                case "default":
                    return Ok(_service.EmployeeManager.MakeReport(new DefaultReport()));
                default:
                    throw new ReportsException("invalid report type");
            }
        }

        [HttpPut]
        [Route("set-superior")]
        public IActionResult SetSuperior([FromQuery] Guid employeeId, [FromQuery] Guid superiorId)
        {
            return Ok(_service.EmployeeManager.SetSuperior(
                _service.EmployeeManager.GetEmployeeById(employeeId) ??
                throw new ReportsException("no such employee found"),
                _service.EmployeeManager.GetEmployeeById(superiorId) ??
                throw new ReportsException("no such superior found")));
        }
    }
}