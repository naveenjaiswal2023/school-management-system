using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Services
{
    public interface IBiometricVerificationService
    {
        Task<BiometricVerificationResult> VerifyAsync(string templateData, BiometricType type);
        Task<string> GenerateTemplateHashAsync(string rawBiometricData, BiometricType type);
    }
}
