using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Employees.Commands;
using SchoolManagement.Application.Employees.Queries;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/hr/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new employee
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal,HRManager")]
        public async Task<ActionResult<CreateEmployeeResponse>> CreateEmployee(CreateEmployeeCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.Success)
            {
                return CreatedAtAction(nameof(GetEmployee), new { id = response.Id }, response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Principal,HRManager,DepartmentHead")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(Guid id)
        {
            var query = new GetEmployeeByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Get employees by department
        /// </summary>
        [HttpGet("department/{departmentId}")]
        [Authorize(Roles = "Admin,Principal,HRManager,DepartmentHead")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByDepartment(Guid departmentId)
        {
            var query = new GetEmployeesByDepartmentQuery { DepartmentId = departmentId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Update employee information
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Principal,HRManager")]
        public async Task<ActionResult<UpdateEmployeeResponse>> UpdateEmployee(Guid id, UpdateEmployeeCommand command)
        {
            command.Id = id;
            var response = await _mediator.Send(command);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        /// <summary>
        /// Process employee onboarding
        /// </summary>
        [HttpPost("{id}/onboard")]
        [Authorize(Roles = "Admin,Principal,HRManager")]
        public async Task<ActionResult<OnboardEmployeeResponse>> OnboardEmployee(Guid id, OnboardEmployeeCommand command)
        {
            command.EmployeeId = id;
            var response = await _mediator.Send(command);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
