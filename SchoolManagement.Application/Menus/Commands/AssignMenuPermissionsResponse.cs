using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Menus.Commands
{
    public class AssignMenuPermissionsResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public int PermissionsAssigned { get; set; }
    }
}
