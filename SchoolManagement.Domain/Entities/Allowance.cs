using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class Allowance : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public AllowanceType Type { get; private set; }
        public decimal Amount { get; private set; }
        public bool IsPercentage { get; private set; }
        public bool IsTaxable { get; private set; }
        public bool IsActive { get; private set; }

        private Allowance() { }

        public Allowance(string name, string description, AllowanceType type,
                        decimal amount, bool isPercentage = false, bool isTaxable = true)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Type = type;
            Amount = amount;
            IsPercentage = isPercentage;
            IsTaxable = isTaxable;
            IsActive = true;
        }
    }

}
