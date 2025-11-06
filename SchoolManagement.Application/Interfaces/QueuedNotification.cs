using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public class QueuedNotification
    {
        public Guid Id { get; set; }
        public NotificationType Type { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime QueuedAt { get; set; }
        public int AttemptCount { get; set; }
        public NotificationStatus Status { get; set; }
    }

}
