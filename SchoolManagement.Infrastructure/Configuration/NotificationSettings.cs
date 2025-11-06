using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Configuration
{
    public class NotificationSettings
    {
        public SMSSettings SMS { get; set; } = new();
        public EmailSettings Email { get; set; } = new();
        public PushSettings Push { get; set; } = new();
        public bool EnableBulkNotifications { get; set; } = true;
        public int MaxRetryAttempts { get; set; } = 3;
        public TimeSpan RetryDelay { get; set; } = TimeSpan.FromMinutes(5);
    }
}
