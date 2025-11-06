using MediatR;
using SchoolManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Menus.Queries
{
    public class GetMenuByIdQuery : IRequest<IEnumerable<MenuDto>>
    {
        public Guid Id { get; set; }
    }

}
