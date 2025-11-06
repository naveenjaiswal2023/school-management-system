using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Services
{
    public class OvertimeCalculation
    {
        public decimal RegularHours { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal OvertimeRate { get; set; }
        public decimal OvertimeAmount { get; set; }
    }
}
