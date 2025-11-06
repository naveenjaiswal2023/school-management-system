using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Interfaces
{
    public interface IAuditService
    {
        Task LogAsync(
            string action,
            string entityName,
            string entityId,
            Guid? userId = null,
            string userEmail = null,
            string ipAddress = null,
            string userAgent = null,
            string details = null);
    }
}
