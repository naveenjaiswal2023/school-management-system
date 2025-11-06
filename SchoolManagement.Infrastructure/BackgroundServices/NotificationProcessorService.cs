using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.BackgroundServices
{
    public class NotificationProcessorService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotificationProcessorService> _logger;
        private readonly TimeSpan _processingInterval = TimeSpan.FromSeconds(5);

        public NotificationProcessorService(
            IServiceProvider serviceProvider,
            ILogger<NotificationProcessorService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Notification Processor Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessNotificationQueue(stoppingToken);
                    await Task.Delay(_processingInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing notification queue");
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
            }

            _logger.LogInformation("Notification Processor Service stopped");
        }

        private async Task ProcessNotificationQueue(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var notificationQueue = scope.ServiceProvider.GetService<INotificationQueue>();
            if (notificationQueue == null)
            {
                _logger.LogWarning("INotificationQueue service not registered. Skipping notification processing.");
                return;
            }

            var notificationService = scope.ServiceProvider.GetService<INotificationService>();
            if (notificationService == null)
            {
                _logger.LogWarning("INotificationService not registered. Skipping notification processing.");
                return;
            }

            try
            {
                //var queueCount = await notificationQueue.GetQueueCountAsync(cancellationToken);
                //if (queueCount == 0)
                //{
                //    return;
                //}

                //_logger.LogInformation("Processing {Count} notifications from queue", queueCount);

                //// Process in batches
                //var batchSize = 10;
                //var notifications = await notificationQueue.DequeueBatchAsync(batchSize, cancellationToken);

                //foreach (var notification in notifications)
                //{
                //    try
                //    {
                //        await ProcessSingleNotification(notification, notificationService, cancellationToken);
                //    }
                //    catch (Exception ex)
                //    {
                //        _logger.LogError(ex, "Error processing notification {NotificationId}", notification.Id);
                //    }
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ProcessNotificationQueue");
            }
        }

        private async Task ProcessSingleNotification(
            Notification notification,
            INotificationService notificationService,
            CancellationToken cancellationToken)
        {
            try
            {
                notification.Status = NotificationStatus.Processing;

                bool success = notification.Type switch
                {
                    //NotificationType.Email => await notificationService.SendEmailAsync(
                    //    notification.Recipient,
                    //    notification.Subject,
                    //    notification.Message,
                    //    cancellationToken),
                    //NotificationType.SMS => await notificationService.SendSMSAsync(
                    //    notification.Recipient,
                    //    notification.Message,
                    //    cancellationToken),
                    //_ => false
                };

                if (success)
                {
                    notification.Status = NotificationStatus.Sent;
                    notification.SentAt = DateTime.UtcNow;
                    _logger.LogInformation("Notification {NotificationId} sent successfully", notification.Id);
                }
                else
                {
                    notification.Status = NotificationStatus.Failed;
                    notification.RetryCount++;
                    _logger.LogWarning("Failed to send notification {NotificationId}", notification.Id);
                }
            }
            catch (Exception ex)
            {
                notification.Status = NotificationStatus.Failed;
                notification.ErrorMessage = ex.Message;
                notification.RetryCount++;
                _logger.LogError(ex, "Exception while processing notification {NotificationId}", notification.Id);
            }
        }
    }
}
