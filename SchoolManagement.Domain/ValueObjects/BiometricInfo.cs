using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.ValueObjects
{
    public class BiometricInfo
    {
        public string TemplateHash { get; private set; }
        public BiometricType Type { get; private set; }
        public DateTime EnrolledAt { get; private set; }
        public string DeviceId { get; private set; }
        public int Quality { get; private set; }

        private BiometricInfo() { } // EF Constructor

        public BiometricInfo(string templateHash, BiometricType type, string deviceId, int quality)
        {
            TemplateHash = templateHash ?? throw new ArgumentNullException(nameof(templateHash));
            Type = type;
            DeviceId = deviceId;
            Quality = quality;
            EnrolledAt = DateTime.UtcNow;
        }
    }
}
