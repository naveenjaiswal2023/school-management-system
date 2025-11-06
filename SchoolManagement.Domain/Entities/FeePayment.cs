using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class FeePayment : BaseEntity
    {
        public Guid StudentId { get; private set; }
        public string ReceiptNumber { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string TransactionId { get; private set; }
        public string Remarks { get; private set; }
        public Guid FeeStructureId { get; private set; }

        // Navigation Properties
        public virtual Student Student { get; private set; }

        private FeePayment() { }

        public FeePayment(Guid studentId, decimal amount, PaymentMethod paymentMethod,
                         Guid feeStructureId, string transactionId = null)
        {
            StudentId = studentId;
            Amount = amount;
            PaymentMethod = paymentMethod;
            FeeStructureId = feeStructureId;
            TransactionId = transactionId;
            PaymentDate = DateTime.UtcNow;
            Status = PaymentStatus.Pending;
            ReceiptNumber = GenerateReceiptNumber();
        }

        public void ConfirmPayment()
        {
            Status = PaymentStatus.Completed;
        }

        public void FailPayment(string remarks)
        {
            Status = PaymentStatus.Failed;
            Remarks = remarks;
        }

        private string GenerateReceiptNumber()
        {
            return $"FEE{DateTime.UtcNow:yyyyMM}{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
        }
    }
}
