using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class RefreshToken: BaseEntity
    {
        public Guid Id { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public bool IsRevoked { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        private RefreshToken() { } // For EF Core

        public RefreshToken(string token, DateTime expiryDate, Guid userId)
        {
            Id = Guid.NewGuid();
            Token = token ?? throw new ArgumentNullException(nameof(token));
            ExpiryDate = expiryDate;
            UserId = userId;
            IsRevoked = false;
            CreatedAt = DateTime.UtcNow;
        }

        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;

        public bool IsActive => !IsRevoked && !IsExpired;

        public void Revoke()
        {
            IsRevoked = true;
        }
    }
}
