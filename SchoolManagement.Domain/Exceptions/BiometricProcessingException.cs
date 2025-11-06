using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Exceptions
{
    public class BiometricProcessingException : Exception
    {
        public BiometricProcessingException(string message) : base(message) { }
        public BiometricProcessingException(string message, Exception innerException) : base(message, innerException) { }
    }
}
