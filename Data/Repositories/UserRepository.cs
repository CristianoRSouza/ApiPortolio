using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiEntregasMentoria.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IRepositoryUser
    {
        public UserRepository(MyContext myContext) : base(myContext)
        {
        }

        public async Task<User> GetByEmail(string email)
        {
            var result = await _dbSet.Include(p => p.RolesToken).FirstOrDefaultAsync(u => u.Email == email);

            return result;
        }
    }
}
