using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public class PendingNotification
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsSent { get; set; }

        public void MarkAsSent()
        {
            IsSent = true;
        }
    }
}
