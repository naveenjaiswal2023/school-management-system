using AutoMapper;
using SchoolManagement.Application.DTOs;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Menu, MenuDto>()
                .ForMember(dest => dest.ParentMenuName,
                    opt => opt.MapFrom(src => src.ParentMenu != null ? src.ParentMenu.Name : null))
                .ForMember(dest => dest.SubMenus,
                    opt => opt.MapFrom(src => src.SubMenus))
                .MaxDepth(1)
                .PreserveReferences();
        }
    }
}
