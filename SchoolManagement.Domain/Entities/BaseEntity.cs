using System;

namespace SchoolManagement.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public string CreatedBy { get; protected set; }
        public string? UpdatedBy { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public string CreatedIP { get; set; }

        // Concurrency token for optimistic concurrency
        public byte[] RowVersion { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }

        public void MarkAsDeleted(string user = "SYSTEM")
        {
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = user;
        }

        public void SetCreated(string user, string ipAddress)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = user;
            CreatedIP = ipAddress;

            // Also set Updated fields on insert to avoid SQL NOT NULL errors
            //UpdatedAt = DateTime.UtcNow;
            //UpdatedBy = user;
        }

        public void SetUpdated(string user, string ipAddress)
        {
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = user;
            CreatedIP = ipAddress;
        }
    }
}
