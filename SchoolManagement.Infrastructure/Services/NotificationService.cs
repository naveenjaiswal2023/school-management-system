using Microsoft.Extensions.Options;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Domain.Enums;
using SchoolManagement.Domain.Exceptions;
using SchoolManagement.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public NotificationService(IOptions<NotificationSettings> settings, IHttpClientFactory httpClientFactory)
        {
            _settings = settings.Value;
            _httpClientFactory = httpClientFactory;
        }

        public async Task SendSMSAsync(string phoneNumber, string message)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("SMS");

                var payload = new
                {
                    api_key = _settings.SMS.ApiKey,
                    sender = _settings.SMS.SenderId,
                    mobile = phoneNumber,
                    message = message
                };

                var response = await httpClient.PostAsJsonAsync(_settings.SMS.Endpoint, payload);

                if (!response.IsSuccessStatusCode)
                {
                    throw new NotificationException($"SMS sending failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new NotificationException($"SMS service error: {ex.Message}", ex);
            }
        }

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("Email");

                var payload = new
                {
                    api_key = _settings.Email.ApiKey,
                    to = email,
                    from = _settings.Email.FromEmail,
                    subject = subject,
                    body = body,
                    html = true
                };

                var response = await httpClient.PostAsJsonAsync(_settings.Email.Endpoint, payload);

                if (!response.IsSuccessStatusCode)
                {
                    throw new NotificationException($"Email sending failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new NotificationException($"Email service error: {ex.Message}", ex);
            }
        }

        public async Task SendPushNotificationAsync(string userId, string title, string message)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("Push");

                var payload = new
                {
                    app_id = _settings.Push.AppId,
                    headings = new { en = title },
                    contents = new { en = message },
                    filters = new[] { new { field = "tag", key = "userId", relation = "=", value = userId } }
                };

                var response = await httpClient.PostAsJsonAsync(_settings.Push.Endpoint, payload);

                if (!response.IsSuccessStatusCode)
                {
                    throw new NotificationException($"Push notification failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new NotificationException($"Push notification service error: {ex.Message}", ex);
            }
        }

        public async Task SendBulkNotificationAsync(IEnumerable<string> recipients, string message, NotificationType type)
        {
            var tasks = recipients.Select(recipient =>
            {
                return type switch
                {
                    NotificationType.SMS => SendSMSAsync(recipient, message),
                    NotificationType.Email => SendEmailAsync(recipient, "School Notification", message),
                    NotificationType.Push => SendPushNotificationAsync(recipient, "School Alert", message),
                    _ => Task.CompletedTask
                };
            });

            await Task.WhenAll(tasks);
        }

        //public async Task SendBulkNotificationAsync(IEnumerable<string> recipients, string message, NotificationType type)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
