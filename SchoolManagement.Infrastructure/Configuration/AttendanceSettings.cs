using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Configuration
{
    public class AttendanceSettings
    {
        public TimeSpan StandardWorkingHours { get; set; } = TimeSpan.FromHours(8);
        public TimeSpan LateArrivalGracePeriod { get; set; } = TimeSpan.FromMinutes(15);
        public TimeSpan MinimumHalfDayDuration { get; set; } = TimeSpan.FromHours(4);
        public decimal OvertimeRate { get; set; } = 1.5m;
        public bool EnableAutomaticStatusCalculation { get; set; } = true;
        public bool AllowManualOverride { get; set; } = true;
        public TimeSpan AttendanceMarkingWindow { get; set; } = TimeSpan.FromHours(2);
    }
}
