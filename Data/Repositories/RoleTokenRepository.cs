using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiEntregasMentoria.Data.Repositories
{
    public class RoleTokenRepository : BaseRepository<RolesToken>,IRepositoryRoleToken
    {
        public RoleTokenRepository(MyContext myContext) : base(myContext)
        {
        }
        
        public async Task<RolesToken> GetByRoleNameAsync(string roleName)
        {
            return await _meuContexto.RolesToken.FirstOrDefaultAsync(r => r.Role == roleName);
        }
    }
}
