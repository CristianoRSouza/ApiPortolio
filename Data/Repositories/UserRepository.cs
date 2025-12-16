using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiEntregasMentoria.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IRepositoryUser
    {
        public UserRepository(MyContext context) : base(context) { }

        public async Task<User?> GetByEmail(string email)
        {
            return await _meuContexto.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
