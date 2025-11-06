using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Configuration
{
    public class PayrollSettings
    {
        public decimal PFContributionRate { get; set; } = 0.12m;
        public decimal ESIContributionRate { get; set; } = 0.0175m;
        public decimal ProfessionalTaxAmount { get; set; } = 200m;
        public decimal IncomeTaxThreshold { get; set; } = 250000m;
        public bool EnableAutomaticCalculation { get; set; } = true;
        public int PayrollProcessingDay { get; set; } = 30; // 30th of every month
    }
}
