using SchoolManagement.Application.Interfaces;
using SchoolManagement.Domain.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Services
{
    public class InMemoryNotificationQueue : INotificationQueue
    {
        private readonly ConcurrentQueue<Notification> _queue;
        private readonly SemaphoreSlim _signal;

        public InMemoryNotificationQueue()
        {
            _queue = new ConcurrentQueue<Notification>();
            _signal = new SemaphoreSlim(0);
        }

        public bool IsEmpty => _queue.IsEmpty;

        public Task EnqueueAsync(Notification notification, CancellationToken cancellationToken = default)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            _queue.Enqueue(notification);
            _signal.Release();

            return Task.CompletedTask;
        }

        public async Task<Notification?> DequeueAsync(CancellationToken cancellationToken = default)
        {
            await _signal.WaitAsync(cancellationToken);

            if (_queue.TryDequeue(out var notification))
            {
                return notification;
            }

            return null;
        }

        public async Task<IEnumerable<Notification>> DequeueBatchAsync(int batchSize, CancellationToken cancellationToken = default)
        {
            var batch = new List<Notification>();

            for (int i = 0; i < batchSize; i++)
            {
                if (_queue.TryDequeue(out var notification))
                {
                    batch.Add(notification);
                }
                else
                {
                    break;
                }
            }

            return batch;
        }

        public Task<int> GetQueueCountAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_queue.Count);
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
