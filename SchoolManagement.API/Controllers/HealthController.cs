using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Persistence;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly SchoolManagementDbContext _context;

        public HealthController(SchoolManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> CheckHealth()
        {
            try
            {
                // Check database connectivity
                await _context.Database.CanConnectAsync();

                return Ok(new
                {
                    status = "Healthy",
                    timestamp = DateTime.UtcNow,
                    database = "Connected",
                    version = "1.0.0"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(503, new
                {
                    status = "Unhealthy",
                    timestamp = DateTime.UtcNow,
                    database = "Disconnected",
                    error = ex.Message
                });
            }
        }
    }
}
