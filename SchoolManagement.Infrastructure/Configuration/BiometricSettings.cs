using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Configuration
{
    public class BiometricSettings
    {
        public double MinimumConfidenceScore { get; set; } = 0.85;
        public string SecretKey { get; set; }
        public int TemplateHashLength { get; set; } = 256;
        public bool EnableLivenessDetection { get; set; } = true;
        public int MaxRetryAttempts { get; set; } = 3;
        public TimeSpan DeviceTimeout { get; set; } = TimeSpan.FromSeconds(30);

        public Dictionary<string, DeviceConfiguration> Devices { get; set; } = new();
    }
}
