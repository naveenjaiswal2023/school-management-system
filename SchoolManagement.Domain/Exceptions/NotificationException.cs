using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Exceptions
{
    public class NotificationException : DomainException
    {
        public NotificationException(string message) : base(message) { }
        public NotificationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
