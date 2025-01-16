using ApiEntregasMentoria.Data.Entities;

namespace ApiEntregasMentoria.Interfaces.Repositories
{
    public interface IRepositoryUser:IBaseRepository<User>
    {
        Task<User> ValidateUser(string email, string password);
    }
}
