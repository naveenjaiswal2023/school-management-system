using MediatR;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Menus.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Menus.Handler.Queries
{
    public class GetAllMenusQueryHandler : IRequestHandler<GetAllMenusQuery, IEnumerable<MenuDto>>
    {
        private readonly IMenuRepository _menuRepository;

        public GetAllMenusQueryHandler(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<IEnumerable<MenuDto>> Handle(GetAllMenusQuery request, CancellationToken cancellationToken)
        {
            var menus = await _menuRepository.GetAllAsync();

            return menus.Select(m => new MenuDto
            {
                Id = m.Id,
                Name = m.Name,
                DisplayName = m.DisplayName,
                Description = m.Description,
                Icon = m.Icon,
                Route = m.Route,
                Component = m.Component,
                Type = m.Type.ToString(),
                SortOrder = m.SortOrder,
                IsActive = m.IsActive,
                IsVisible = m.IsVisible,
                ParentMenuId = m.ParentMenuId,
                ParentMenuName = m.ParentMenu?.DisplayName
            });
        }
    }
}
