using SchoolManagement.Domain.Enums;
using System;

namespace SchoolManagement.Domain.Entities
{
    public class LeaveApplication : BaseEntity
    {
        public Guid EmployeeId { get; private set; }
        public LeaveType LeaveType { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public int NumberOfDays { get; private set; }
        public string Reason { get; private set; }
        public LeaveStatus Status { get; private set; }
        public Guid? ApprovedBy { get; private set; }
        public DateTime? ApprovedDate { get; private set; }
        public string ApprovalRemarks { get; private set; }
        public string DocumentPath { get; private set; }

        // Navigation Properties
        public virtual Employee Employee { get; private set; }

        /// <summary>
        /// Manager/Employee who approved/rejected the leave.
        /// Self-referencing relationship to Employee.
        /// </summary>
        public virtual Employee ApprovedByEmployee { get; private set; }

        private LeaveApplication() { } // EF Core

        public LeaveApplication(Guid employeeId, LeaveType leaveType, DateTime fromDate,
                                DateTime toDate, string reason, string documentPath = null)
        {
            if (fromDate > toDate)
                throw new ArgumentException("FromDate cannot be later than ToDate.");

            EmployeeId = employeeId;
            LeaveType = leaveType;
            FromDate = fromDate.Date;
            ToDate = toDate.Date;
            Reason = reason ?? throw new ArgumentNullException(nameof(reason));
            DocumentPath = documentPath;
            Status = LeaveStatus.Pending;

            NumberOfDays = CalculateNumberOfDays();
        }

        public void Approve(Guid approvedBy, string remarks = null)
        {
            Status = LeaveStatus.Approved;
            ApprovedBy = approvedBy;
            ApprovedDate = DateTime.UtcNow;
            ApprovalRemarks = remarks;
        }

        public void Reject(Guid rejectedBy, string remarks)
        {
            Status = LeaveStatus.Rejected;
            ApprovedBy = rejectedBy;
            ApprovedDate = DateTime.UtcNow;
            ApprovalRemarks = remarks ?? throw new ArgumentNullException(nameof(remarks));
        }

        private int CalculateNumberOfDays()
        {
            return (ToDate - FromDate).Days + 1;
        }
    }
}
