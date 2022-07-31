using xmen.api.Models;
using xmen.core.Dtos;
using xmen.infraestructure.Entities;

namespace xmen.api.Mapper
{
    public class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<MutantModel, MutantDto>().ReverseMap();
            CreateMap<MutantDto, Mutant>().ReverseMap();
            CreateMap<Mutant, MutantResponse>().ReverseMap();
        }
    }
}
