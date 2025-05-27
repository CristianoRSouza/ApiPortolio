using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Interfaces.Repositories;
using ApiEntregasMentoria.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApiEntregasMentoria.Data.Repositories
{
    public class MatchRepository : BaseRepository<Match>, IRepositoryMatch
    {
        private readonly MyContext _context;

        public MatchRepository(MyContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Match> GetMatchIncludeAsync(int id)
        {
            var match = await _context.Matchs
                .Include(x => x.HomeTeam)
                .Include(x => x.AwayTeam)
                .Include(x => x.Markets)
                    .ThenInclude(m => m.BetItems)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (match is null)
                throw new MatchNotFound("Match não encontrado.");

            return match;
        }

        public async Task<IEnumerable<Match>> GetAllIncludeAsync()
        {
            return await _context.Matchs
                .Include(x => x.HomeTeam)
                .Include(x => x.AwayTeam)
                .Include(x => x.Markets)
                    .ThenInclude(m => m.BetItems)
                .ToListAsync();
        }

        public async Task UpdateCustomAsync(Match entidade)
        {
            var existing = await _context.Matchs.FindAsync(entidade.Id);
            if (existing is null)
                throw new MatchNotFound("Match não encontrado.");

            existing.HomeTeamId = entidade.HomeTeamId;
            existing.AwayTeamId = entidade.AwayTeamId;
            existing.MatchDateTime = entidade.MatchDateTime;
            existing.HomeGoals = entidade.HomeGoals;
            existing.AwayGoals = entidade.AwayGoals;

            _context.Matchs.Update(existing);
        }
    }
}
