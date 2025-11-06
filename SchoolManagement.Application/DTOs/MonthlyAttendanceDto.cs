using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.DTOs
{
    public class MonthlyAttendanceDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int PresentDays { get; set; }
        public int TotalDays { get; set; }
        public decimal Percentage { get; set; }
    }
}
