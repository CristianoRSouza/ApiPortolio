using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Interfaces.Repositories;
using ApiEntregasMentoria.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ApiEntregasMentoria.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserService( IMapper mapper, IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }
        public async Task AddUser(UserDto user)
        {
            var userEntity = _mapper.Map<User>(user);
            userEntity.PasswordHash = _passwordHasher.HashPassword(userEntity,user.Password);

            userEntity.RolesToken = await _unitOfWork._RoleTokenRepository.GetByRoleNameAsync("Client");

            await _unitOfWork._UserRepository.Add(userEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteUser(int id)
        {
            await _unitOfWork._UserRepository.Delete(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<UserDto>> GetAllUser()
        {
            return _mapper.Map<IEnumerable<UserDto>>(await _unitOfWork._UserRepository.GetAll());
        }

        public async Task<UserDto> GetUser(int id)
        {
            return _mapper.Map<UserDto>(await _unitOfWork._UserRepository.Get(id));
        }

        public async Task UpdateUser(UserDto user)
        {
            var existingUser = await _unitOfWork._UserRepository.Get(user.Id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"Usuário com ID {user.Id} não foi encontrado.");
            }

            _mapper.Map(user, existingUser);


            await _unitOfWork._UserRepository.Update(_mapper.Map<User>(existingUser));
            await _unitOfWork.CommitAsync();
        }
    }
}
