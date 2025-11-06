using System;
using System.Collections.Generic;

namespace SchoolManagement.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Code { get; private set; }
        public Guid? HeadOfDepartmentId { get; private set; }
        public bool IsActive { get; private set; }

        // Navigation Properties
        public virtual Employee HeadOfDepartment { get; private set; }
        public virtual ICollection<Employee> Employees { get; private set; }

        private Department() // EF Core requires private/protected ctor
        {
            Employees = new List<Employee>();
        }

        public Department(string name, string code, string description = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Description = description;
            IsActive = true;
            Employees = new List<Employee>();
        }

        public void UpdateDetails(string name, string code, string description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Description = description;
        }

        public void SetHeadOfDepartment(Guid? headOfDepartmentId)
        {
            HeadOfDepartmentId = headOfDepartmentId;
        }

        public void SetActiveStatus(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
