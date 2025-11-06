using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Enums
{
    public enum PayrollStatus
    {
        Draft = 1,
        Processing = 2,
        Processed = 3,
        Approved = 4,
        Paid = 5,
        OnHold = 6
    }
}
