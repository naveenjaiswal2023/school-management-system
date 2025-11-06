using SchoolManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        // Add any custom methods for UserRole if needed
    }
}
