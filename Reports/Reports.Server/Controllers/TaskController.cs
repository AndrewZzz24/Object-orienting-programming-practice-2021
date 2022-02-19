using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Reports.DAL.Entities;
using Reports.DAL.Tools;
using Reports.Server.Services;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly IReportService _service;

        public TaskController(IReportService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        public Task AddNewTask([FromQuery] string taskDetails)
        {
            if (taskDetails == null)
                throw new ReportsException("Name cannot be null");
            return _service.TaskService.AddNewTask(taskDetails);
        }

        [HttpGet]
        [Route("find-by-id")]
        public IActionResult FindById([FromQuery] Guid id)
        {
            if (id != Guid.Empty)
            {
                Task result = _service.TaskService.FindTaskWithId(id);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return StatusCode((int) HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Route("find-with-date-creation")]
        public IActionResult FindByDateCreation([FromQuery] DateTime date)
        {
            Task result = _service.TaskService.FindTaskWithDateCreation(date);
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("find-by-last-edit-date")]
        public IActionResult FindWithLastEditDate([FromQuery] DateTime date)
        {
            Task result = _service.TaskService.FindTaskWithLastChangeDate(date);
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("find-tasks-assigned-to-employee")]
        public IActionResult GetTasksAssignedToEmployee([FromQuery] Guid employeeId)
        {
            return Ok(_service.TaskService.FindTasksAssignedToEmployee(
                _service.EmployeeManager.GetEmployeeById(employeeId)));
        }

        [HttpGet]
        [Route("find-tasks-edited-by-employee")]
        public IActionResult GetTasksEditedByEmployee([FromQuery] Guid employeeId)
        {
            return Ok(_service.TaskService.FindTasksEditedByEmployee(
                _service.EmployeeManager.GetEmployeeById(employeeId)));
        }

        [HttpPut]
        [Route("change-task-state")]
        public IActionResult ChangeTaskState([FromQuery] Guid taskId, string state)
        {
            switch (state)
            {
                case "Open":
                    return Ok(_service.TaskService.ChangeTaskState(_service.TaskService.FindTaskWithId(taskId),
                        TaskState.Open));
                case "Active":
                    return Ok(_service.TaskService.ChangeTaskState(_service.TaskService.FindTaskWithId(taskId),
                        TaskState.Active));
                case "Resolved":
                    return Ok(_service.TaskService.ChangeTaskState(_service.TaskService.FindTaskWithId(taskId),
                        TaskState.Resolved));
                default:
                    return NotFound();
            }
        }

        [HttpPut]
        [Route("add-comment-to-task")]
        public IActionResult AddCommentToTask([FromQuery] Guid taskId, [FromQuery] string comment)
        {
            if (taskId == Guid.Empty || comment == null)
                throw new ReportsException("null in parameters when adding a comment to task");
            return Ok(_service.TaskService.AddCommentToTask(
                _service.TaskService.FindTaskWithId(taskId) ?? throw new ReportsException("no such task"), comment));
        }

        [HttpPut]
        [Route("set-task-assigner")]
        public IActionResult SetTaskAssigner([FromQuery] Guid taskId, [FromQuery] Guid assignerId)
        {
            return Ok(_service.TaskService.SetTaskAssigner(
                _service.TaskService.FindTaskWithId(taskId) ?? throw new ReportsException("no such task found"),
                _service.EmployeeManager.GetEmployeeById(assignerId) ??
                throw new ReportsException("no such assigner found")));
        }
    }
}