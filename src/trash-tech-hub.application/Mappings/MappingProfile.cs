using AutoMapper;
using TrashTechHub.Core.DTOs;
using TrashTechHub.Core.Models;

namespace TrashTechHub.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<Post, PostDto>();
        CreateMap<Project, ProjectDto>();
    }
}
