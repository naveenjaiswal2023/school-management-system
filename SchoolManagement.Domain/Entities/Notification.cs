using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class Notification:BaseEntity
    {
        public Guid Id { get; set; }
        public NotificationType Type { get; set; }
        public string Recipient { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationStatus Status { get; set; }
        
        public DateTime? SentAt { get; set; }
        public int RetryCount { get; set; }
        public string? ErrorMessage { get; set; }
        [NotMapped] // ✅ Add this
        public Dictionary<string, string>? Metadata { get; set; }

        public Notification()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            Status = NotificationStatus.Pending;
            RetryCount = 0;
        }
    }
}
