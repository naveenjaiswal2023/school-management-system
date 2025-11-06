using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Configuration
{
    public class DeviceConfiguration
    {
        public string DeviceType { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastSyncTime { get; set; }
    }
}
