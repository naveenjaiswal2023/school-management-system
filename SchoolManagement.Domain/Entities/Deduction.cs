using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class Deduction : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DeductionType Type { get; private set; }
        public decimal Amount { get; private set; }
        public bool IsPercentage { get; private set; }
        public bool IsStatutory { get; private set; }
        public bool IsActive { get; private set; }

        private Deduction() { }

        public Deduction(string name, string description, DeductionType type,
                        decimal amount, bool isPercentage = false, bool isStatutory = false)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Type = type;
            Amount = amount;
            IsPercentage = isPercentage;
            IsStatutory = isStatutory;
            IsActive = true;
        }
    }
}
