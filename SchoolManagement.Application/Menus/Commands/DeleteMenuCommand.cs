using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Menus.Commands
{
    public class DeleteMenuCommand : IRequest<DeleteMenuResponse>
    {
        public Guid Id { get; set; }
    }
}
