using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class Designation : BaseEntity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int Level { get; private set; }
        public decimal MinSalary { get; private set; }
        public decimal MaxSalary { get; private set; }
        public bool IsActive { get; private set; }

        // Navigation Properties
        public virtual ICollection<Employee> Employees { get; private set; }

        private Designation()
        {
            Employees = new List<Employee>();
        }

        public Designation(string title, string description, int level,
                          decimal minSalary, decimal maxSalary)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description;
            Level = level;
            MinSalary = minSalary;
            MaxSalary = maxSalary;
            IsActive = true;
            Employees = new List<Employee>();
        }

        public void UpdateDetails(string title, string description, int level,
                                 decimal minSalary, decimal maxSalary)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description;
            Level = level;
            MinSalary = minSalary;
            MaxSalary = maxSalary;
        }

        public void SetActiveStatus(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
