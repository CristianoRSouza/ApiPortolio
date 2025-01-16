using ApiEntregasMentoria.Controllers;
using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Data.Repositories;
using ApiEntregasMentoria.Interfaces.Repositories;
using ApiEntregasMentoria.Interfaces.Services;
using AutoMapper;

namespace ApiEntregasMentoria.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryUser _repositoryUser;
        private readonly IMapper _Mapper;
        public UserService(IRepositoryUser userRepository, IMapper mapper)
        {
            _repositoryUser = userRepository;
            _Mapper = mapper;
        }
        public async Task AddUser(UserDto user)
        {
            await _repositoryUser.Add(_Mapper.Map<User>(user));
        }

        public async Task DeleteUser(int id)
        {
            await _repositoryUser.Delete(id);
        }

        public async Task<IEnumerable<UserDto>> GetAllUser()
        {
            return _Mapper.Map<IEnumerable<UserDto>>(await _repositoryUser.GetAll());
        }

        public async Task<UserDto> GetUser(int id)
        {
            return _Mapper.Map<UserDto>(await _repositoryUser.Get(id));
        }

        public async Task UpdateUser(UserDto user)
        {
            var existingUser = await _repositoryUser.Get(user.Id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"Usuário com ID {user.Id} não foi encontrado.");
            }

            _Mapper.Map(user, existingUser);


            await _repositoryUser.Update(_Mapper.Map<User>(existingUser));
        }
    }
}
