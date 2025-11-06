using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Students.Commands;
using SchoolManagement.Application.Students.Queries;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new student
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<ActionResult<CreateStudentResponse>> CreateStudent(CreateStudentCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.Success)
            {
                return CreatedAtAction(nameof(GetStudent), new { id = response.Id }, response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// Get student by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Principal,Teacher,Parent")]
        public async Task<ActionResult<StudentDto>> GetStudent(Guid id)
        {
            var query = new GetStudentByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Get students by class
        /// </summary>
        [HttpGet("class/{classId}")]
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudentsByClass(Guid classId)
        {
            var query = new GetStudentsByClassQuery { ClassId = classId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Update student information
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<ActionResult<UpdateStudentResponse>> UpdateStudent(Guid id, UpdateStudentCommand command)
        {
            command.Id = id;
            var response = await _mediator.Send(command);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        /// <summary>
        /// Enroll student biometric data
        /// </summary>
        [HttpPost("{id}/biometric")]
        [Authorize(Roles = "Admin,Principal")]
        public async Task<ActionResult<EnrollBiometricResponse>> EnrollBiometric(Guid id, EnrollBiometricCommand command)
        {
            command.StudentId = id;
            var response = await _mediator.Send(command);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
