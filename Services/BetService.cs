using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Data.ContextEntity;
using Microsoft.EntityFrameworkCore;

namespace ApiEntregasMentoria.Services
{
    public class BetService
    {
        private readonly MyContext _context;

        public BetService(MyContext context)
        {
            _context = context;
        }

        public async Task<List<BetDto>> GetBetsByUserAsync(int userId)
        {
            return await _context.Set<Bet>()
                .Include(b => b.Match)
                    .ThenInclude(m => m.Team1)
                .Include(b => b.Match)
                    .ThenInclude(m => m.Team2)
                .Include(b => b.Match)
                    .ThenInclude(m => m.Championship)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BetDto
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    MatchId = b.MatchId,
                    BetType = b.BetType,
                    BetAmount = b.BetAmount,
                    SelectedOdd = b.SelectedOdd,
                    ResultAmount = b.ResultAmount,
                    Status = b.Status,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt,
                    Match = new BetMatchDto
                    {
                        Id = b.Match.Id,
                        Team1Name = b.Match.Team1.Name,
                        Team2Name = b.Match.Team2.Name,
                        ChampionshipName = b.Match.Championship.Name,
                        MatchDate = b.Match.MatchDate,
                        Status = b.Match.Status,
                        Team1Score = b.Match.Team1Score,
                        Team2Score = b.Match.Team2Score
                    }
                })
                .ToListAsync();
        }

        public async Task<BetDto?> GetBetByIdAsync(int id, int userId)
        {
            return await _context.Set<Bet>()
                .Include(b => b.Match)
                .Where(b => b.Id == id && b.UserId == userId)
                .Select(b => new BetDto
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    MatchId = b.MatchId,
                    BetType = b.BetType,
                    BetAmount = b.BetAmount,
                    SelectedOdd = b.SelectedOdd,
                    ResultAmount = b.ResultAmount,
                    Status = b.Status,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<BetDto> CreateBetAsync(int userId, CreateBetDto request)
        {
            var user = await _context.Set<User>().FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            if (user.Balance < request.BetAmount) throw new InvalidOperationException("Insufficient balance");

            var match = await _context.Set<Match>().FindAsync(request.MatchId);
            if (match == null) throw new KeyNotFoundException("Match not found");
            if (match.Status != "scheduled") throw new InvalidOperationException("Match not available for betting");

            var bet = new Bet
            {
                UserId = userId,
                MatchId = request.MatchId,
                BetType = request.BetType,
                BetAmount = request.BetAmount,
                SelectedOdd = request.SelectedOdd,
                Status = "pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            user.Balance -= request.BetAmount;
            _context.Set<Bet>().Add(bet);
            await _context.SaveChangesAsync();

            return new BetDto
            {
                Id = bet.Id,
                UserId = bet.UserId,
                MatchId = bet.MatchId,
                BetType = bet.BetType,
                BetAmount = bet.BetAmount,
                SelectedOdd = bet.SelectedOdd,
                Status = bet.Status,
                CreatedAt = bet.CreatedAt
            };
        }
    }
}
