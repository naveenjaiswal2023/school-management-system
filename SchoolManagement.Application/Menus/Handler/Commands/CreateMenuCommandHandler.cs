using MediatR;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Menus.Commands;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Menus.Handler.Commands
{
    public class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, CreateMenuResponse>
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMenuCommandHandler(IMenuRepository menuRepository, IUnitOfWork unitOfWork)
        {
            _menuRepository = menuRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateMenuResponse> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var menu = new Menu(
                    request.Name,
                    request.DisplayName,
                    request.Route,
                    (MenuType)request.Type,
                    request.Icon,
                    request.Description,
                    request.ParentMenuId);

                menu.SetSortOrder(request.SortOrder);

                var createdMenu = await _menuRepository.CreateAsync(menu);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new CreateMenuResponse
                {
                    Id = createdMenu.Id,
                    Message = "Menu created successfully",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new CreateMenuResponse
                {
                    Message = $"Error creating menu: {ex.Message}",
                    Success = false
                };
            }
        }
    }
}
