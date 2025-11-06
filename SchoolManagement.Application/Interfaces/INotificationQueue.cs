using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface INotificationQueue
    {
        Task<IEnumerable<QueuedNotification>> DequeueAsync(int count);
        Task MarkAsProcessedAsync(Guid notificationId);
        Task MarkAsFailedAsync(Guid notificationId, string errorMessage);
        Task EnqueueAsync(QueuedNotification notification);
    }
}
