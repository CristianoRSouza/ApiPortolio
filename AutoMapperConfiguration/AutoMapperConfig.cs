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
            //CreateMap<IEnumerable<UserDto>, IEnumerable<User>>().ReverseMap();
        }
    }
}
