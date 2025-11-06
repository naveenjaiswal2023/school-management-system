using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Exceptions
{
    public class BiometricVerificationException : DomainException
    {
        public BiometricVerificationException(string message) : base(message) { }
        public BiometricVerificationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
