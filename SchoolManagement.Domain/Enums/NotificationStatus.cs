using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Enums
{
    public enum NotificationStatus
    {
        Queued = 1,
        Processing = 2,
        Sent = 3,
        Failed = 4,
        Retry = 5,
        Pending=6
    }
}
