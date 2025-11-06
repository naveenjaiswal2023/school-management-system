using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Configuration
{
    public class EmailSettings
    {
        public string ApiKey { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Endpoint { get; set; }
        public string BaseUrl { get; set; }
        public bool EnableTracking { get; set; } = true;
    }
}
