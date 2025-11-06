using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Menus.Commands
{
    public class CreateMenuCommand : IRequest<CreateMenuResponse>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Route { get; set; }
        public string Component { get; set; }
        public int Type { get; set; } // MenuType enum
        public int SortOrder { get; set; }
        public Guid? ParentMenuId { get; set; }
    }
}
