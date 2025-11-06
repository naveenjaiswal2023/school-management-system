using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendSMSAsync(string phoneNumber, string message);
        Task SendEmailAsync(string email, string subject, string body);
        Task SendPushNotificationAsync(string userId, string title, string message);
        //Task SendBulkNotificationAsync(IEnumerable<string> recipients, string message, NotificationType type);
    }
}
