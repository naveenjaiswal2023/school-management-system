using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Configuration
{
    public class SMSSettings
    {
        public string ApiKey { get; set; }
        public string SenderId { get; set; }
        public string Endpoint { get; set; }
        public string BaseUrl { get; set; }
        public bool EnableDeliveryReports { get; set; } = true;
    }
}
