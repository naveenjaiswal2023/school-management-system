using Microsoft.Extensions.Logging;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly SchoolManagementDbContext _context;
        private readonly ILogger<AuditService> _logger;

        public AuditService(SchoolManagementDbContext context, ILogger<AuditService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task LogAsync(
            string action,
            string entityName,
            string entityId,
            Guid? userId = null,
            string userEmail = null,
            string ipAddress = null,
            string userAgent = null,
            string details = null)
        {
            try
            {
                var auditLog = new AuditLog(
                    action,
                    entityName,
                    entityId,
                    userId,
                    userEmail,
                    ipAddress,
                    userAgent,
                    details);

                _context.AuditLogs.Add(auditLog);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Audit log created: {Action} on {EntityName} by {UserEmail}",
                    action, entityName, userEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create audit log for action: {Action}", action);
            }
        }
    }
}
