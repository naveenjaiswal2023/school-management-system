using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.DTOs
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public string Resource { get; set; }
        public bool IsSystemPermission { get; set; }
    }
}
