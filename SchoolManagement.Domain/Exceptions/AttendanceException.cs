using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Exceptions
{
    public class AttendanceException : DomainException
    {
        public AttendanceException(string message) : base(message) { }
        public AttendanceException(string message, Exception innerException) : base(message, innerException) { }
    }

}
