using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Configuration
{
    public class PushSettings
    {
        public string AppId { get; set; }
        public string ApiKey { get; set; }
        public string Endpoint { get; set; }
        public bool EnableRichNotifications { get; set; } = true;
    }
}
