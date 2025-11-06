using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; private set; }
        public string Action { get; private set; }
        public string EntityName { get; private set; }
        public string EntityId { get; private set; }
        public Guid? UserId { get; private set; }
        public string UserEmail { get; private set; }
        public string IpAddress { get; private set; }
        public string UserAgent { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Details { get; private set; }

        private AuditLog() { } // For EF Core

        public AuditLog(
            string action,
            string entityName,
            string entityId,
            Guid? userId = null,
            string userEmail = null,
            string ipAddress = null,
            string userAgent = null,
            string details = null)
        {
            Id = Guid.NewGuid();
            Action = action;
            EntityName = entityName;
            EntityId = entityId;
            UserId = userId;
            UserEmail = userEmail;
            IpAddress = ipAddress;
            UserAgent = userAgent;
            Details = details;
            Timestamp = DateTime.UtcNow;
        }
    }
}
