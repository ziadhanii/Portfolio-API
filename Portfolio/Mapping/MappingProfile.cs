using AutoMapper;
using Portfolio.Controllers;
using Portfolio.DTOs;

namespace Portfolio.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectDto>()
            .ForMember(dto => dto.Technologies,
                opt => opt.MapFrom(p => p.ProjectTechnologies.Select(pt => pt.Technology)));

        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();
        

        CreateMap<Technology, TechnologyResponseDto>();
        CreateMap<TechnologyCreateDto, Technology>();

        CreateMap<Skill, SkillDto>();
        CreateMap<CreateSkillDto, Skill>();
    }
}