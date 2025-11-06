using SchoolManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student> GetByIdAsync(Guid id);
        Task<Student> GetByStudentIdAsync(string studentId);
        Task<IEnumerable<Student>> GetByClassAsync(Guid classId);
        Task<Student> CreateAsync(Student student);
        Task<Student> UpdateAsync(Student student);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<IEnumerable<Student>> SearchAsync(string searchTerm, int page, int pageSize);
    }
}
