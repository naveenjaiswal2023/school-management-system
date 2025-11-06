using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SchoolManagement.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagement.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SchoolManagementDbContext _context;
        private IDbContextTransaction _transaction;

        // Repository instances
        private IAuthRepository _authRepository;
        private IEmployeeRepository _employeeRepository;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IRoleMenuPermissionRepository _roleMenuPermissionRepository;
        private IStudentRepository _studentRepository;
        private IAttendanceRepository _attendanceRepository;
        private IMenuRepository _menuRepository;
        // Add other repositories as needed
        private IUserRoleRepository _userRoleRepository;

        public UnitOfWork(SchoolManagementDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Repository Properties (shared DbContext)
        public IAuthRepository AuthRepository =>
            _authRepository ??= new AuthRepository(_context);

        public IEmployeeRepository EmployeeRepository =>
            _employeeRepository ??= new EmployeeRepository(_context);

        public IUserRepository UserRepository =>
            _userRepository ??= new UserRepository(_context);

        public IRoleRepository RoleRepository =>
            _roleRepository ??= new RoleRepository(_context);

        public IRoleMenuPermissionRepository RoleMenuPermissionRepository =>
            _roleMenuPermissionRepository ??= new RoleMenuPermissionRepository(_context);

        public IStudentRepository StudentRepository =>
            _studentRepository ??= new StudentRepository(_context);

        public IAttendanceRepository AttendanceRepository =>
            _attendanceRepository ??= new AttendanceRepository(_context);

        public IUserRoleRepository UserRoleRepository =>
            _userRoleRepository ??= new UserRoleRepository(_context);
        public IMenuRepository MenuRepository =>
            _menuRepository ??= new MenuRepository(_context);

        #endregion

        #region SaveChanges (with safe error handling)
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log or rethrow with meaningful message
                throw new DbUpdateConcurrencyException(
                    "A concurrency conflict occurred while saving changes. " +
                    "The data may have been modified or deleted by another user.", ex);
            }
            catch (DbUpdateException ex)
            {
                // Capture any FK or constraint violation
                throw new InvalidOperationException("Database update failed.", ex);
            }
        }
        #endregion

        #region Transaction Handling
        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
                return; // Prevent nested transactions

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _context.SaveChangesAsync(); // Ensure pending changes are committed
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        #endregion

        #region Disposal
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
