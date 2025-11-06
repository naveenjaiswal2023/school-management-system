using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class Section : BaseEntity
    {
        public string Name { get; private set; }
        public Guid ClassId { get; private set; }
        public int Capacity { get; private set; }
        public bool IsActive { get; private set; }

        // Navigation Properties
        public virtual Class Class { get; private set; }
        public virtual ICollection<Student> Students { get; private set; }

        private Section()
        {
            Students = new List<Student>();
        }

        public Section(string name, Guid classId, int capacity)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ClassId = classId;
            Capacity = capacity;
            IsActive = true;
            Students = new List<Student>();
        }

        public void UpdateDetails(string name, int capacity)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Capacity = capacity;
        }

        public void SetActiveStatus(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
