using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Enums
{
    public enum ReviewStatus
    {
        Draft = 1,
        Submitted = 2,
        UnderReview = 3,
        Acknowledged = 4,
        Completed = 5
    }
}
