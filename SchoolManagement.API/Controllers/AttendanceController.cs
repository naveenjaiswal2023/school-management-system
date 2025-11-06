using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.Attendance.Commands;
using SchoolManagement.Application.Attendance.Queries;
using SchoolManagement.Application.DTOs;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttendanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Mark attendance via biometric capture
        /// </summary>
        [HttpPost("capture")]
        [AllowAnonymous] // Device authentication handled separately
        public async Task<ActionResult<MarkAttendanceResponse>> CaptureAttendance(MarkAttendanceCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        /// <summary>
        /// Get student attendance for today
        /// </summary>
        [HttpGet("student/{id}/today")]
        [Authorize(Roles = "Admin,Principal,Teacher,Parent")]
        public async Task<ActionResult<AttendanceDto>> GetTodayAttendance(Guid id)
        {
            var query = new GetTodayAttendanceQuery { StudentId = id, Date = DateTime.Today };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get class attendance for specific date
        /// </summary>
        [HttpGet("class/{classId}/date/{date}")]
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<ActionResult<IEnumerable<AttendanceDto>>> GetClassAttendance(Guid classId, DateTime date)
        {
            var query = new GetClassAttendanceQuery { ClassId = classId, Date = date };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get attendance statistics
        /// </summary>
        [HttpGet("student/{id}/statistics")]
        [Authorize(Roles = "Admin,Principal,Teacher,Parent")]
        public async Task<ActionResult<AttendanceStatisticsDto>> GetAttendanceStatistics(
            Guid id, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            var query = new GetAttendanceStatisticsQuery
            {
                StudentId = id,
                FromDate = fromDate,
                ToDate = toDate
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Manual attendance override
        /// </summary>
        [HttpPost("manual-override")]
        [Authorize(Roles = "Admin,Principal,Teacher")]
        public async Task<ActionResult<ManualAttendanceResponse>> ManualOverride(ManualAttendanceCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
