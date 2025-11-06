using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace SchoolManagement.Domain.Entities
{
    public class Class : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Capacity { get; private set; }
        public bool IsActive { get; private set; }

        // Navigation Properties
        public virtual ICollection<Student> Students { get; private set; } = new List<Student>();
        public virtual ICollection<Section> Sections { get; private set; } = new List<Section>();


        private Class()
        {
            Students = new List<Student>();
            Sections = new List<Section>();
        }

        public Class(string name, string description, int capacity)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Capacity = capacity;
            IsActive = true;
            Students = new List<Student>();
            Sections = new List<Section>();
        }

        public void UpdateDetails(string name, string description, int capacity)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Capacity = capacity;
        }

        public void SetActiveStatus(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
