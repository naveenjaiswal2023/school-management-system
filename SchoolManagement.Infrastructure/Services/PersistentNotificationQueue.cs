using Microsoft.EntityFrameworkCore;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;
using SchoolManagement.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Services
{
    public class PersistentNotificationQueue : INotificationQueue
    {
        private readonly SchoolManagementDbContext _context;
        private readonly SemaphoreSlim _signal;

        public PersistentNotificationQueue(SchoolManagementDbContext context)
        {
            _context = context;
            _signal = new SemaphoreSlim(0);
        }

        public bool IsEmpty => !_context.Set<Notification>()
            .Any(n => n.Status == NotificationStatus.Pending);

        public async Task EnqueueAsync(Notification notification, CancellationToken cancellationToken = default)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            notification.Status = NotificationStatus.Pending;
            //notification.CreatedAt = DateTime.UtcNow;

            _context.Set<Notification>().Add(notification);
            await _context.SaveChangesAsync(cancellationToken);

            _signal.Release();
        }

        public async Task<Notification?> DequeueAsync(CancellationToken cancellationToken = default)
        {
            await _signal.WaitAsync(cancellationToken);

            var notification = await _context.Set<Notification>()
                .Where(n => n.Status == NotificationStatus.Pending)
                .OrderBy(n => n.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (notification != null)
            {
                notification.Status = NotificationStatus.Processing;
                await _context.SaveChangesAsync(cancellationToken);
            }

            return notification;
        }

        public async Task<IEnumerable<Notification>> DequeueBatchAsync(int batchSize, CancellationToken cancellationToken = default)
        {
            var notifications = await _context.Set<Notification>()
                .Where(n => n.Status == NotificationStatus.Pending)
                .OrderBy(n => n.CreatedAt)
                .Take(batchSize)
                .ToListAsync(cancellationToken);

            foreach (var notification in notifications)
            {
                notification.Status = NotificationStatus.Processing;
            }

            await _context.SaveChangesAsync(cancellationToken);
            return notifications;
        }

        public async Task<int> GetQueueCountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Notification>()
                .CountAsync(n => n.Status == NotificationStatus.Pending, cancellationToken);
        }

        public Task<IEnumerable<QueuedNotification>> DequeueAsync(int count)
        {
            throw new NotImplementedException();
        }

        public Task MarkAsProcessedAsync(Guid notificationId)
        {
            throw new NotImplementedException();
        }

        public Task MarkAsFailedAsync(Guid notificationId, string errorMessage)
        {
            throw new NotImplementedException();
        }

        public Task EnqueueAsync(QueuedNotification notification)
        {
            throw new NotImplementedException();
        }
    }
}
