using Microsoft.AspNetCore.Mvc;
using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiEntregasMentoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OddsController : ControllerBase
    {
        private readonly MyContext _context;

        public OddsController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("match/{matchId}")]
        public async Task<IActionResult> GetOddsByMatch(int matchId)
        {
            var odds = await _context.Odds
                .Where(o => o.MatchId == matchId && o.IsActive)
                .OrderBy(o => o.BetType)
                .ToListAsync();

            return Ok(odds);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateOdds([FromBody] List<UpdateOddRequest> requests)
        {
            foreach (var request in requests)
            {
                var odd = await _context.Odds.FindAsync(request.Id);
                if (odd != null)
                {
                    odd.Value = request.Value;
                    odd.UpdatedAt = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Odds updated successfully" });
        }

        [HttpPost("seed/{matchId}")]
        public async Task<IActionResult> SeedOdds(int matchId)
        {
            var match = await _context.Matches.FindAsync(matchId);
            if (match == null) return NotFound();

            var odds = new List<Odd>
            {
                new Odd { MatchId = matchId, BetType = "1x2", Selection = "Team1", Value = 2.50m, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Odd { MatchId = matchId, BetType = "1x2", Selection = "Draw", Value = 3.20m, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Odd { MatchId = matchId, BetType = "1x2", Selection = "Team2", Value = 2.80m, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Odd { MatchId = matchId, BetType = "Goals", Selection = "Over 2.5", Value = 1.85m, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Odd { MatchId = matchId, BetType = "Goals", Selection = "Under 2.5", Value = 1.95m, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };

            _context.Odds.AddRange(odds);
            await _context.SaveChangesAsync();

            return Ok(odds);
        }
    }

    public class UpdateOddRequest
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
    }
}