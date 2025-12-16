using ApiEntregasMentoria.Data.Dto;

namespace ApiEntregasMentoria.Interfaces.Services
{
    public interface IUserService
    {
        Task AddUser(UserDto user);
        Task UpdateUser(UserDto user);
        Task<UserDto> GetUser(int id);
        Task<IEnumerable<UserDto>> GetAllUser();
        Task DeleteUser(int id);
    }

}
