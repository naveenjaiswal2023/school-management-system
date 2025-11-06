using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Persistence
{
    public class SchoolManagementDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SchoolManagementDbContext(DbContextOptions<SchoolManagementDbContext> options,
                                     IHttpContextAccessor httpContextAccessor = null)
        : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        //public SchoolManagementDbContext(DbContextOptions<SchoolManagementDbContext> options)
        //    : base(options)
        //{ }

        #region DbSets

        // Master Data
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RoleMenuPermission> RoleMenuPermissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        // Academic
        public DbSet<Student> Students { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<StudentParent> StudentParents { get; set; }
        public DbSet<FeePayment> FeePayments { get; set; }

        // Employee & Payroll
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<LeaveApplication> LeaveApplications { get; set; }
        public DbSet<PayrollRecord> PayrollRecords { get; set; }
        public DbSet<PerformanceReview> PerformanceReviews { get; set; }
        public DbSet<Allowance> Allowances { get; set; }
        public DbSet<Deduction> Deductions { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SchoolManagementDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            ApplyAuditInfo();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInfo()
        {
            var entries = ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            var currentUser = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "System";
            var currentIp = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.SetCreated(currentUser, currentIp);
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.SetUpdated(currentUser, currentIp);
                }
            }
        }
    }
}
