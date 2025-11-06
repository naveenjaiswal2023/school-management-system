using SchoolManagement.Domain.Enums;
using SchoolManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class StudentParent : BaseEntity
    {
        public Guid StudentId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public ParentRelationship Relationship { get; private set; }
        public bool IsPrimaryContact { get; private set; }
        public Address Address { get; private set; }
        public string Occupation { get; private set; }

        // Navigation Properties
        public virtual Student Student { get; private set; }

        private StudentParent() { }

        public StudentParent(Guid studentId, string firstName, string lastName,
                           string email, string phone, ParentRelationship relationship)
        {
            StudentId = studentId;
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email;
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            Relationship = relationship;
            IsPrimaryContact = false;
        }

        public void UpdateContactInfo(string email, string phone, Address address)
        {
            Email = email;
            Phone = phone;
            Address = address;
        }

        public void SetAsPrimaryContact()
        {
            IsPrimaryContact = true;
        }
    }
}
