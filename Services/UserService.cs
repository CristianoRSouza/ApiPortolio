using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Interfaces.Services;
using ApiEntregasMentoria.Interfaces.Repositories;

namespace ApiEntregasMentoria.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserDto>> GetAllUser()
        {
            var users = await _unitOfWork._UserRepository.GetAll();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                Nickname = u.Nickname,
                FullName = u.FullName,
                Phone = u.Phone,
                Cpf = u.Cpf,
                Balance = u.Balance,
                IsVerified = u.IsVerified,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                LastLogin = u.LastLogin
            });
        }

        public async Task<UserDto> GetUser(int id)
        {
            var user = await _unitOfWork._UserRepository.Get(id);
            if (user == null) throw new KeyNotFoundException("User not found");

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Nickname = user.Nickname,
                FullName = user.FullName,
                Phone = user.Phone,
                Cpf = user.Cpf,
                Balance = user.Balance,
                IsVerified = user.IsVerified,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin
            };
        }

        public async Task AddUser(UserDto userDto)
        {
            var user = new User
            {
                Email = userDto.Email,
                Nickname = userDto.Nickname,
                FullName = userDto.FullName,
                Phone = userDto.Phone,
                Cpf = userDto.Cpf,
                Balance = userDto.Balance,
                IsVerified = userDto.IsVerified,
                IsActive = userDto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork._UserRepository.Add(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateUser(UserDto userDto)
        {
            var user = await _unitOfWork._UserRepository.Get(userDto.Id);
            if (user == null) throw new KeyNotFoundException("User not found");

            user.Email = userDto.Email;
            user.Nickname = userDto.Nickname;
            user.FullName = userDto.FullName;
            user.Phone = userDto.Phone;
            user.Cpf = userDto.Cpf;
            user.Balance = userDto.Balance;
            user.IsVerified = userDto.IsVerified;
            user.IsActive = userDto.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork._UserRepository.Update(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteUser(int id)
        {
            await _unitOfWork._UserRepository.Delete(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
