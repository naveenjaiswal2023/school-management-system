using SchoolManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByIdAsync(Guid id);
        Task<Employee> GetByEmployeeIdAsync(string employeeId);
        Task<IEnumerable<Employee>> GetByDepartmentAsync(Guid departmentId);
        Task<Employee> CreateAsync(Employee employee);
        Task<Employee> UpdateAsync(Employee employee);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
    }
}
