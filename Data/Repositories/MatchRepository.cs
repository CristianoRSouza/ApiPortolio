using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiEntregasMentoria.Data.Repositories
{
    public class MatchRepository : BaseRepository<Match>, IRepositoryMatch
    {
        public MatchRepository(MyContext context) : base(context) { }

        public async Task<IEnumerable<Match>> GetAllIncludeAsync()
        {
            return await _meuContexto.Set<Match>()
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .Include(m => m.Championship)
                .ToListAsync();
        }

        public async Task<Match?> GetMatchIncludeAsync(int id)
        {
            return await _meuContexto.Set<Match>()
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .Include(m => m.Championship)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task UpdateCustomAsync(Match match)
        {
            _meuContexto.Set<Match>().Update(match);
            await _meuContexto.SaveChangesAsync();
        }
    }
}
