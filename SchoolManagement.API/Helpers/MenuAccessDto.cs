using SchoolManagement.Domain.ValueObjects;

namespace SchoolManagement.API.Helpers
{
    public class MenuAccessDto
    {
        public Guid MenuId { get; set; }
        public bool HasAccess { get; set; }
        public MenuPermissions Permissions { get; set; }
    }
}
