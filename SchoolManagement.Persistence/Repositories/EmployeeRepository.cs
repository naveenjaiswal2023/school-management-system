using Microsoft.EntityFrameworkCore;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Persistence.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SchoolManagementDbContext _context;

        public EmployeeRepository(SchoolManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetByIdAsync(Guid id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task<Employee> GetByEmployeeIdAsync(string employeeId)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId && !e.IsDeleted);
        }

        public async Task<IEnumerable<Employee>> GetByDepartmentAsync(Guid departmentId)
        {
            return await _context.Employees
                .Where(e => e.DepartmentId == departmentId && !e.IsDeleted)
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .ToListAsync();
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            return employee;
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            return employee;
        }

        public async Task DeleteAsync(Guid id)
        {
            var employee = await GetByIdAsync(id);
            if (employee != null)
            {
                employee.MarkAsDeleted();
                _context.Employees.Update(employee);
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Employees.AnyAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
        {
            return await _context.Employees
                .Where(e => !e.IsDeleted && e.Status == EmployeeStatus.Active)
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .ToListAsync();
        }
    }
}
