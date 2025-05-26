using ApiEntregasMentoria.Data.Entities;

namespace ApiEntregasMentoria.Interfaces.Repositories
{
    public interface IRepositoryRoleToken:IBaseRepository<RolesToken>
    {
        Task<RolesToken> GetByRoleNameAsync(string roleName);
    }
}
