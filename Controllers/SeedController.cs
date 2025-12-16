using Microsoft.AspNetCore.Mvc;
using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Data.Entities;

namespace ApiEntregasMentoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly MyContext _context;

        public SeedController(MyContext context)
        {
            _context = context;
        }

        [HttpPost("clear")]
        public async Task<IActionResult> ClearData()
        {
            try
            {
                _context.Set<Bet>().RemoveRange(_context.Set<Bet>());
                _context.Set<Transaction>().RemoveRange(_context.Set<Transaction>());
                _context.Set<Notification>().RemoveRange(_context.Set<Notification>());
                _context.Set<Match>().RemoveRange(_context.Set<Match>());
                _context.Set<Team>().RemoveRange(_context.Set<Team>());
                _context.Set<Championship>().RemoveRange(_context.Set<Championship>());
                _context.Set<User>().RemoveRange(_context.Set<User>());

                await _context.SaveChangesAsync();
                return Ok(new { message = "Data cleared successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("seed")]
        public async Task<IActionResult> SeedData()
        {
            try
            {
                // Seed Championships
                var championships = new List<Championship>
                {
                    new Championship { Name = "Premier League", Country = "England", IsActive = true, CreatedAt = DateTime.UtcNow },
                    new Championship { Name = "La Liga", Country = "Spain", IsActive = true, CreatedAt = DateTime.UtcNow },
                    new Championship { Name = "Serie A", Country = "Italy", IsActive = true, CreatedAt = DateTime.UtcNow }
                };
                _context.Set<Championship>().AddRange(championships);

                // Seed Teams
                var teams = new List<Team>
                {
                    new Team { Name = "Manchester United", Country = "England", CreatedAt = DateTime.UtcNow },
                    new Team { Name = "Barcelona", Country = "Spain", CreatedAt = DateTime.UtcNow },
                    new Team { Name = "Juventus", Country = "Italy", CreatedAt = DateTime.UtcNow }
                };
                _context.Set<Team>().AddRange(teams);

                await _context.SaveChangesAsync();
                return Ok(new { message = "Data seeded successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
