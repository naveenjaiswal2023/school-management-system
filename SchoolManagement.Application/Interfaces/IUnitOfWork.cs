using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthRepository AuthRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IMenuRepository MenuRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IStudentRepository StudentRepository { get; }
        IAttendanceRepository AttendanceRepository { get; }
        IRoleMenuPermissionRepository RoleMenuPermissionRepository { get; } // <-- Add this line
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
