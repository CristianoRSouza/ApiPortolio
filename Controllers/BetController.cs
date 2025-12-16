using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiEntregasMentoria.Data.ContextEntity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiEntregasMentoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BetController : ControllerBase
    {
        private readonly MyContext _context;

        public BetController(MyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetBets()
        {
            var userId = GetUserId();
            var bets = await _context.Bets
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return Ok(bets);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBet([FromBody] CreateBetRequest request)
        {
            var userId = GetUserId();
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return NotFound();

            if (user.Balance < request.BetAmount)
                return BadRequest(new { message = "Insufficient balance" });

            var bet = new Data.Entities.Bet
            {
                TicketId = GenerateTicketId(),
                UserId = userId,
                MatchId = request.MatchId,
                BetType = request.BetType,
                Selection = request.Selection,
                BetAmount = request.BetAmount,
                SelectedOdd = request.SelectedOdd,
                ResultAmount = request.BetAmount * request.SelectedOdd,
                Status = "pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            user.Balance -= request.BetAmount;

            _context.Bets.Add(bet);
            await _context.SaveChangesAsync();

            return Ok(bet);
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var userId = GetUserId();
            var bets = await _context.Bets
                .Where(b => b.UserId == userId)
                .ToListAsync();

            var stats = new
            {
                TotalBets = bets.Count,
                WonBets = bets.Count(b => b.Status == "won"),
                LostBets = bets.Count(b => b.Status == "lost"),
                PendingBets = bets.Count(b => b.Status == "pending"),
                TotalBetAmount = bets.Sum(b => b.BetAmount),
                TotalWinnings = bets.Where(b => b.Status == "won").Sum(b => b.ResultAmount),
                WinRate = bets.Count > 0 ? (decimal)bets.Count(b => b.Status == "won") / bets.Count * 100 : 0
            };

            return Ok(stats);
        }

        private string GenerateTicketId()
        {
            return $"BET{DateTime.Now.Ticks.ToString().Substring(8)}";
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim!);
        }
    }

    public class CreateBetRequest
    {
        public int MatchId { get; set; }
        public string BetType { get; set; } = string.Empty;
        public string Selection { get; set; } = string.Empty;
        public decimal BetAmount { get; set; }
        public decimal SelectedOdd { get; set; }
    }
}