using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.BackgroundServices
{
    public class AttendanceSyncService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AttendanceSyncService> _logger;
        private readonly TimeSpan _syncInterval = TimeSpan.FromMinutes(5);

        public AttendanceSyncService(IServiceProvider serviceProvider, ILogger<AttendanceSyncService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Attendance Sync Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var attendanceService = scope.ServiceProvider.GetRequiredService<IAttendanceService>();

                    await SyncOfflineDevices(attendanceService);
                    await ProcessPendingNotifications(scope.ServiceProvider);

                    _logger.LogInformation("Attendance sync completed successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during attendance sync");
                }

                await Task.Delay(_syncInterval, stoppingToken);
            }

            _logger.LogInformation("Attendance Sync Service stopped");
        }

        private async Task SyncOfflineDevices(IAttendanceService attendanceService)
        {
            // Sync attendance data from offline biometric devices
            var offlineDevices = await attendanceService.GetOfflineDevicesAsync();

            foreach (var device in offlineDevices)
            {
                try
                {
                    var pendingRecords = await attendanceService.GetPendingRecordsAsync(device.Id);

                    foreach (var record in pendingRecords)
                    {
                        await attendanceService.ProcessOfflineAttendanceAsync(record);
                    }

                    _logger.LogInformation($"Synced {pendingRecords.Count()} records from device {device.Id}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to sync device {device.Id}");
                }
            }
        }

        private async Task ProcessPendingNotifications(IServiceProvider serviceProvider)
        {
            var notificationService = serviceProvider.GetRequiredService<INotificationService>();
            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

            // Process attendance-related notifications
            var pendingNotifications = await GetPendingAttendanceNotificationsAsync(serviceProvider);

            foreach (var notification in pendingNotifications)
            {
                try
                {
                    await notificationService.SendSMSAsync(notification.PhoneNumber, notification.Message);
                    notification.MarkAsSent();
                    await unitOfWork.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to send notification {notification.Id}");
                }
            }
        }

        private async Task<IEnumerable<PendingNotification>> GetPendingAttendanceNotificationsAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<SchoolManagementDbContext>();

            // Implementation would fetch pending notifications from database
            return await Task.FromResult(new List<PendingNotification>());
        }
    }
}
