using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;
using SchoolManagement.Domain.ValueObjects;
using SchoolManagement.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Data
{
    public static class MenuDataSeeder
    {
        public static async Task SeedAsync(SchoolManagementDbContext context)
        {
            if (!context.Menus.Any())
                await SeedMenusAsync(context);

            if (!context.Roles.Any())
                await SeedRolesAsync(context);

            if (!context.Permissions.Any())
                await SeedPermissionsAsync(context);

            if (!context.RoleMenuPermissions.Any())
                await SeedRoleMenuPermissionsAsync(context);

            await context.SaveChangesAsync();
        }

        private static async Task SeedMenusAsync(SchoolManagementDbContext context)
        {
            var menus = new List<Menu>
            {
                new Menu("Dashboard", "Dashboard", "/dashboard", MenuType.Module, "dashboard", "System Dashboard"),
                new Menu("StudentManagement", "Student Management", "/students", MenuType.Module, "users", "Student Management System"),
                new Menu("HRMSManagement", "HRMS", "/hrms", MenuType.Module, "briefcase", "Human Resource Management"),
                new Menu("AttendanceManagement", "Attendance", "/attendance", MenuType.Module, "calendar-check", "Attendance Management"),
                new Menu("FeeManagement", "Fee Management", "/fees", MenuType.Module, "credit-card", "Fee Management System"),
                new Menu("ExaminationManagement", "Examinations", "/examinations", MenuType.Module, "book-open", "Examination Management"),
                new Menu("ReportsManagement", "Reports", "/reports", MenuType.Module, "bar-chart", "Reports and Analytics"),
                new Menu("SystemSettings", "Settings", "/settings", MenuType.Module, "settings", "System Configuration")
            };

            context.Menus.AddRange(menus);
            await context.SaveChangesAsync();

            // Fetch parent IDs safely
            var studentModule = menus.FirstOrDefault(m => m.Name == "StudentManagement");
            var hrmsModule = menus.FirstOrDefault(m => m.Name == "HRMSManagement");
            var attendanceModule = menus.FirstOrDefault(m => m.Name == "AttendanceManagement");
            var feeModule = menus.FirstOrDefault(m => m.Name == "FeeManagement");
            var examModule = menus.FirstOrDefault(m => m.Name == "ExaminationManagement");
            var reportsModule = menus.FirstOrDefault(m => m.Name == "ReportsManagement");
            var settingsModule = menus.FirstOrDefault(m => m.Name == "SystemSettings");

            if (studentModule == null || hrmsModule == null || attendanceModule == null || feeModule == null || examModule == null || reportsModule == null || settingsModule == null)
                throw new Exception("One or more parent modules are missing.");

            var subMenus = new List<Menu>
            {
                new Menu("StudentList", "Student List", "/students/list", MenuType.Menu, "list", "View all students", studentModule.Id),
                new Menu("StudentAdd", "Add Student", "/students/add", MenuType.Menu, "user-plus", "Add new student", studentModule.Id),
                new Menu("StudentBiometric", "Biometric Enrollment", "/students/biometric", MenuType.Menu, "fingerprint", "Biometric enrollment", studentModule.Id),
                new Menu("StudentReports", "Student Reports", "/students/reports", MenuType.Menu, "file-text", "Student reports", studentModule.Id),

                new Menu("EmployeeList", "Employee List", "/hrms/employees", MenuType.Menu, "users", "View all employees", hrmsModule.Id),
                new Menu("EmployeeAdd", "Add Employee", "/hrms/employees/add", MenuType.Menu, "user-plus", "Add new employee", hrmsModule.Id),
                new Menu("PayrollManagement", "Payroll", "/hrms/payroll", MenuType.Menu, "dollar-sign", "Payroll management", hrmsModule.Id),
                new Menu("LeaveManagement", "Leave Management", "/hrms/leaves", MenuType.Menu, "calendar-x", "Leave management", hrmsModule.Id),
                new Menu("PerformanceReview", "Performance Review", "/hrms/performance", MenuType.Menu, "trending-up", "Performance reviews", hrmsModule.Id),

                new Menu("AttendanceMarking", "Mark Attendance", "/attendance/mark", MenuType.Menu, "check-circle", "Mark attendance", attendanceModule.Id),
                new Menu("AttendanceReports", "Attendance Reports", "/attendance/reports", MenuType.Menu, "bar-chart-2", "Attendance reports", attendanceModule.Id),
                new Menu("AttendanceDevices", "Device Management", "/attendance/devices", MenuType.Menu, "smartphone", "Biometric devices", attendanceModule.Id),

                new Menu("FeeStructure", "Fee Structure", "/fees/structure", MenuType.Menu, "layers", "Fee structure setup", feeModule.Id),
                new Menu("FeeCollection", "Fee Collection", "/fees/collection", MenuType.Menu, "credit-card", "Collect fees", feeModule.Id),
                new Menu("FeeReports", "Fee Reports", "/fees/reports", MenuType.Menu, "pie-chart", "Fee collection reports", feeModule.Id),

                new Menu("ExamSchedule", "Exam Schedule", "/exams/schedule", MenuType.Menu, "calendar", "Examination schedule", examModule.Id),
                new Menu("MarksEntry", "Marks Entry", "/exams/marks", MenuType.Menu, "edit", "Enter examination marks", examModule.Id),
                new Menu("ResultGeneration", "Result Generation", "/exams/results", MenuType.Menu, "award", "Generate results", examModule.Id),

                new Menu("StudentReportsDetail", "Student Reports", "/reports/students", MenuType.Menu, "users", "Student analytics", reportsModule.Id),
                new Menu("AttendanceReportsDetail", "Attendance Reports", "/reports/attendance", MenuType.Menu, "calendar", "Attendance analytics", reportsModule.Id),
                new Menu("FinancialReports", "Financial Reports", "/reports/financial", MenuType.Menu, "dollar-sign", "Financial analytics", reportsModule.Id),

                new Menu("UserManagement", "User Management", "/settings/users", MenuType.Menu, "user-cog", "Manage users", settingsModule.Id),
                new Menu("RoleManagement", "Role Management", "/settings/roles", MenuType.Menu, "shield", "Manage roles", settingsModule.Id),
                new Menu("MenuManagement", "Menu Management", "/settings/menus", MenuType.Menu, "menu", "Manage menus", settingsModule.Id),
                new Menu("SystemConfiguration", "System Config", "/settings/system", MenuType.Menu, "settings", "System configuration", settingsModule.Id)
            };

            context.Menus.AddRange(subMenus);
            await context.SaveChangesAsync();
        }

        private static async Task SeedRolesAsync(SchoolManagementDbContext context)
        {
            var roles = new List<Role>
            {
                new Role("SuperAdmin", "Super Administrator", "Full system access", true, 0),
                new Role("Admin", "Administrator", "School administration access", true, 1),
                new Role("Principal", "Principal", "Principal level access", true, 2),
                new Role("HRManager", "HR Manager", "Human resource management access", false, 3),
                new Role("DepartmentHead", "Department Head", "Department level access", false, 4),
                new Role("Teacher", "Teacher", "Teaching staff access", false, 5),
                new Role("Accountant", "Accountant", "Financial management access", false, 6),
                new Role("Receptionist", "Receptionist", "Front desk access", false, 7),
                new Role("Parent", "Parent", "Parent portal access", false, 8),
                new Role("Student", "Student", "Student portal access", false, 9)
            };

            context.Roles.AddRange(roles);
            await context.SaveChangesAsync();
        }

        private static async Task SeedPermissionsAsync(SchoolManagementDbContext context)
        {
            var permissions = new List<Permission>
            {
                new Permission("student.view", "View Students", "StudentManagement", "View", "Student", "View student information"),
                new Permission("student.create", "Create Students", "StudentManagement", "Create", "Student", "Create new students"),
                new Permission("student.update", "Update Students", "StudentManagement", "Update", "Student", "Update student information"),
                new Permission("student.delete", "Delete Students", "StudentManagement", "Delete", "Student", "Delete students"),
                new Permission("student.biometric", "Manage Biometric", "StudentManagement", "Manage", "Biometric", "Manage student biometric data"),

                new Permission("employee.view", "View Employees", "HRMSManagement", "View", "Employee", "View employee information"),
                new Permission("employee.create", "Create Employees", "HRMSManagement", "Create", "Employee", "Create new employees"),
                new Permission("employee.update", "Update Employees", "HRMSManagement", "Update", "Employee", "Update employee information"),
                new Permission("employee.delete", "Delete Employees", "HRMSManagement", "Delete", "Employee", "Delete employees"),
                new Permission("payroll.manage", "Manage Payroll", "HRMSManagement", "Manage", "Payroll", "Process payroll"),

                new Permission("attendance.view", "View Attendance", "AttendanceManagement", "View", "Attendance", "View attendance records"),
                new Permission("attendance.mark", "Mark Attendance", "AttendanceManagement", "Create", "Attendance", "Mark attendance"),
                new Permission("attendance.modify", "Modify Attendance", "AttendanceManagement", "Update", "Attendance", "Modify attendance records"),

                new Permission("fee.view", "View Fees", "FeeManagement", "View", "Fee", "View fee information"),
                new Permission("fee.collect", "Collect Fees", "FeeManagement", "Create", "Payment", "Collect fee payments"),
                new Permission("fee.structure", "Manage Fee Structure", "FeeManagement", "Manage", "FeeStructure", "Manage fee structure"),

                new Permission("user.manage", "Manage Users", "SystemSettings", "Manage", "User", "Manage system users"),
                new Permission("role.manage", "Manage Roles", "SystemSettings", "Manage", "Role", "Manage user roles"),
                new Permission("system.configure", "System Configuration", "SystemSettings", "Configure", "System", "Configure system settings")
            };

            context.Permissions.AddRange(permissions);
            await context.SaveChangesAsync();
        }

        private static async Task SeedRoleMenuPermissionsAsync(SchoolManagementDbContext context)
        {
            // Load roles and menus safely
            var roles = context.Roles
                   .AsNoTracking()
                   .ToList() // load into memory first
                   .Where(r => !string.IsNullOrEmpty(r.Name))
                   .ToList();

            var menus = context.Menus
                               .Include(m => m.ParentMenu)
                               .AsNoTracking()
                               .ToList()
                               .Where(m => !string.IsNullOrEmpty(m.Name))
                               .ToList();

            Role superAdminRole = roles.FirstOrDefault(r => r.Name == "SuperAdmin");
            Role adminRole = roles.FirstOrDefault(r => r.Name == "Admin");
            Role principalRole = roles.FirstOrDefault(r => r.Name == "Principal");
            Role teacherRole = roles.FirstOrDefault(r => r.Name == "Teacher");
            Role parentRole = roles.FirstOrDefault(r => r.Name == "Parent");

            if (superAdminRole == null || adminRole == null || principalRole == null || teacherRole == null || parentRole == null)
                throw new Exception("One or more required roles are missing in the database.");

            var roleMenuPermissions = new List<RoleMenuPermission>();

            // SuperAdmin - Full access
            foreach (var menu in menus)
                roleMenuPermissions.Add(new RoleMenuPermission(superAdminRole.Id, menu.Id, MenuPermissions.FullAccess()));

            // Admin - Full except SystemSettings
            foreach (var menu in menus.Where(m => !string.Equals(m.Name, "SystemSettings", StringComparison.OrdinalIgnoreCase)))
                roleMenuPermissions.Add(new RoleMenuPermission(adminRole.Id, menu.Id, MenuPermissions.FullAccess()));

            // Admin - ReadWrite access to SystemSettings submenus (excluding SystemConfiguration)
            var systemSettingsMenus = menus.Where(m =>
                string.Equals(m.Name, "SystemSettings", StringComparison.OrdinalIgnoreCase) ||
                (m.ParentMenu != null && !string.IsNullOrEmpty(m.ParentMenu.Name) &&
                 string.Equals(m.ParentMenu.Name, "SystemSettings", StringComparison.OrdinalIgnoreCase))
            ).Where(m => !string.Equals(m.Name, "SystemConfiguration", StringComparison.OrdinalIgnoreCase));

            foreach (var menu in systemSettingsMenus)
                roleMenuPermissions.Add(new RoleMenuPermission(adminRole.Id, menu.Id, MenuPermissions.ReadWrite()));

            // Principal - Full access
            var principalModules = new[] { "Dashboard", "StudentManagement", "HRMSManagement", "AttendanceManagement", "FeeManagement", "ExaminationManagement", "ReportsManagement" };
            foreach (var moduleName in principalModules)
            {
                var moduleMenus = menus.Where(m =>
                    string.Equals(m.Name, moduleName, StringComparison.OrdinalIgnoreCase) ||
                    (m.ParentMenu != null && !string.IsNullOrEmpty(m.ParentMenu.Name) &&
                     string.Equals(m.ParentMenu.Name, moduleName, StringComparison.OrdinalIgnoreCase))
                );

                foreach (var menu in moduleMenus)
                    roleMenuPermissions.Add(new RoleMenuPermission(principalRole.Id, menu.Id, MenuPermissions.FullAccess()));
            }

            // Teacher - Limited access
            var teacherModules = new[] { "Dashboard", "StudentManagement", "AttendanceManagement", "ExaminationManagement" };
            foreach (var moduleName in teacherModules)
            {
                var moduleMenus = menus.Where(m =>
                    string.Equals(m.Name, moduleName, StringComparison.OrdinalIgnoreCase) ||
                    (m.ParentMenu != null && !string.IsNullOrEmpty(m.ParentMenu.Name) &&
                     string.Equals(m.ParentMenu.Name, moduleName, StringComparison.OrdinalIgnoreCase))
                );

                foreach (var menu in moduleMenus)
                {
                    var permissions = moduleName == "Dashboard" ? MenuPermissions.ViewOnly() : MenuPermissions.ReadWrite();

                    if (string.Equals(menu.Name, "StudentAdd", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(menu.Name, "StudentBiometric", StringComparison.OrdinalIgnoreCase))
                    {
                        permissions = MenuPermissions.ViewOnly();
                    }

                    roleMenuPermissions.Add(new RoleMenuPermission(teacherRole.Id, menu.Id, permissions));
                }
            }

            // Parent - View only
            var parentModuleNames = new[] { "Dashboard", "StudentReports", "AttendanceReports", "FeeCollection" };
            var parentMenus = menus.Where(m => !string.IsNullOrEmpty(m.Name) && parentModuleNames.Contains(m.Name, StringComparer.OrdinalIgnoreCase));
            foreach (var menu in parentMenus)
                roleMenuPermissions.Add(new RoleMenuPermission(parentRole.Id, menu.Id, MenuPermissions.ViewOnly()));

            context.RoleMenuPermissions.AddRange(roleMenuPermissions);
            await context.SaveChangesAsync();
        }
    }
}
