using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Data.Entities;
using AutoMapper;

namespace ApiEntregasMentoria.AutoMapperConfiguration
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<Match, MatchCreateDto>().ReverseMap();
            CreateMap<Match, MatchDto>().ReverseMap();
            CreateMap<Match, MatchUpdateDto>().ReverseMap();
        }
    }
}
