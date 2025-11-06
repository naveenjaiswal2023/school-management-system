using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Students.Commands
{
    public class EnrollBiometricCommand : IRequest<EnrollBiometricResponse>
    {
        public Guid StudentId { get; set; }
        public string BiometricData { get; set; }
        public int BiometricType { get; set; }
        public string DeviceId { get; set; }
        public int Quality { get; set; }
    }
}
