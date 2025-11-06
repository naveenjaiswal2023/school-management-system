using MediatR;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Menus.Queries
{
    public class GetMenuHierarchyQuery : IRequest<IEnumerable<MenuDto>>
    {
    }
}
