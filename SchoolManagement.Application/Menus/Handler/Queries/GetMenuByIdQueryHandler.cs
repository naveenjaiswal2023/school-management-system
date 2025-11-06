using AutoMapper;
using MediatR;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Menus.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolManagement.Application.Menus.Handler.Queries
{
    public class GetMenuByIdQueryHandler : IRequestHandler<GetMenuByIdQuery, IEnumerable<MenuDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMenuByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<MenuDto>> Handle(GetMenuByIdQuery request, CancellationToken cancellationToken)
        {
            // Fetch list of menus (filtered by role ID)
            var menus = await _unitOfWork.MenuRepository.GetByIdAsync(request.Id);

            if (menus == null || !menus.Any())
                return Enumerable.Empty<MenuDto>();

            // Map list of Menu → list of MenuDto
            var menuDtos = _mapper.Map<IEnumerable<MenuDto>>(menus);

            return menuDtos;
        }
    }
}
