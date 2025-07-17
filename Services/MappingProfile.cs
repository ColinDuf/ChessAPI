using AutoMapper;
using OpeningExplorer.Entities;
using OpeningExplorer.DTOs;

namespace OpeningExplorer.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Opening, OpeningDto>();
        CreateMap<CreateOpeningDto, Opening>();
    }
}