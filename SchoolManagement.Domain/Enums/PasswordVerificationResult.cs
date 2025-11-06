using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Enums
{
    public enum PasswordVerificationResult
    {
        Failed = 0,
        Success = 1,
        SuccessRehashNeeded = 2
    }
}
