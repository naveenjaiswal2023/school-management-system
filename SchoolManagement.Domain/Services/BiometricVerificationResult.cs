using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Services
{
    public class BiometricVerificationResult
    {
        public bool IsVerified { get; set; }
        public double ConfidenceScore { get; set; }
        public string DeviceId { get; set; }
        public DateTime VerificationTime { get; set; }
    }
}
