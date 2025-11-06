using Microsoft.AspNetCore.Authorization;

namespace SchoolManagement.API.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class MenuPermissionAttribute : Attribute, IAuthorizationRequirement
    {
        public string MenuName { get; }
        public string[] RequiredPermissions { get; }

        public MenuPermissionAttribute(string menuName, params string[] requiredPermissions)
        {
            MenuName = menuName ?? throw new ArgumentNullException(nameof(menuName));
            RequiredPermissions = requiredPermissions ?? throw new ArgumentNullException(nameof(requiredPermissions));
        }
    }
}
