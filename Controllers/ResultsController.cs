using Microsoft.AspNetCore.Mvc;
using ApiEntregasMentoria.Data.ContextEntity;
using Microsoft.EntityFrameworkCore;

namespace ApiEntregasMentoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly MyContext _context;

        public ResultsController(MyContext context)
        {
            _context = context;
        }

        [HttpPost("update/{matchId}")]
        public async Task<IActionResult> UpdateResult(int matchId, [FromBody] UpdateResultRequest request)
        {
            var match = await _context.Matches.FindAsync(matchId);
            if (match == null) return NotFound();

            match.Team1Score = request.Team1Score;
            match.Team2Score = request.Team2Score;
            match.Status = request.Status;
            match.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Liquidar apostas
            await LiquidateBets(matchId);

            return Ok(new { message = "Result updated and bets liquidated" });
        }

        [HttpGet("live")]
        public async Task<IActionResult> GetLiveMatches()
        {
            var liveMatches = await _context.Matches
                .Include(m => m.Championship)
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .Where(m => m.Status == "live")
                .ToListAsync();

            return Ok(liveMatches);
        }

        private async Task LiquidateBets(int matchId)
        {
            var match = await _context.Matches.FindAsync(matchId);
            if (match?.Status != "finished") return;

            var bets = await _context.Bets
                .Include(b => b.User)
                .Where(b => b.MatchId == matchId && b.Status == "pending")
                .ToListAsync();

            foreach (var bet in bets)
            {
                var isWinner = DetermineBetResult(bet, match);
                
                bet.Status = isWinner ? "won" : "lost";
                bet.UpdatedAt = DateTime.UtcNow;

                if (isWinner)
                {
                    bet.User.Balance += bet.ResultAmount;
                    
                    // Criar transação de ganho
                    var transaction = new Data.Entities.Transaction
                    {
                        UserId = bet.UserId,
                        Type = "win",
                        Amount = bet.ResultAmount,
                        Status = "completed",
                        Description = $"Bet win - {bet.TicketId}",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    
                    _context.Transactions.Add(transaction);
                }
            }

            await _context.SaveChangesAsync();
        }

        private bool DetermineBetResult(Data.Entities.Bet bet, Data.Entities.Match match)
        {
            return bet.BetType switch
            {
                "1x2" when bet.Selection == "Team1" => match.Team1Score > match.Team2Score,
                "1x2" when bet.Selection == "Draw" => match.Team1Score == match.Team2Score,
                "1x2" when bet.Selection == "Team2" => match.Team2Score > match.Team1Score,
                "Goals" when bet.Selection == "Over 2.5" => (match.Team1Score + match.Team2Score) > 2,
                "Goals" when bet.Selection == "Under 2.5" => (match.Team1Score + match.Team2Score) <= 2,
                _ => false
            };
        }
    }

    public class UpdateResultRequest
    {
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public string Status { get; set; } = "finished";
    }
}