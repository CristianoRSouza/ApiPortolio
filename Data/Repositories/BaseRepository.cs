using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiEntregasMentoria.Data.Repositories
{
    public class BaseRepository<Tentity> : IBaseRepository<Tentity> where Tentity : class
    {
        protected readonly MyContext _meuContexto;
        protected readonly DbSet<Tentity> _dbSet;
        public BaseRepository(MyContext myContext)
        {
            _meuContexto = myContext;
            _dbSet = _meuContexto.Set<Tentity>();
        }
        public async Task Add(Tentity entidade)
        {
            _dbSet.Add(entidade);   
            await SaveChanges();
        }

        public async Task Delete(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await SaveChanges();
            }
        }

        public async Task<Tentity> Get(int id)
        {
            try
            {
                var result = await _dbSet.FindAsync(id);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Tentity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<int> SaveChanges()
        {
            return await _meuContexto.SaveChangesAsync();
        }

        public async Task Update(Tentity entidade)
        {
            _dbSet.Update(entidade);
            await SaveChanges();
        }
    }
}
