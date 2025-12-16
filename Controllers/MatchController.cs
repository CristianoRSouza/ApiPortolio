using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiEntregasMentoria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _service;
        private readonly MyContext _context;

        public MatchController(IMatchService service, MyContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var matches = await _service.GetAllAsync();
            return Ok(matches);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var match = await _service.GetByIdAsync(id);
            return Ok(match);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MatchCreateDto matchDto)
        {
            var match = await _service.CreateAsync(matchDto);
            return CreatedAtAction(nameof(GetById), new { id = match.Id }, match);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MatchUpdateDto matchDto)
        {
            await _service.UpdateAsync(id, matchDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("championships")]
        public async Task<IActionResult> GetChampionships()
        {
            var championships = await _context.Championships.ToListAsync();
            return Ok(championships);
        }

        [HttpGet("teams")]
        public async Task<IActionResult> GetTeams()
        {
            var teams = await _context.Teams.ToListAsync();
            return Ok(teams);
        }

        [HttpGet("teams/search")]
        public async Task<IActionResult> SearchTeams([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
                return Ok(new List<object>());

            var teams = await _context.Teams
                .Where(t => t.Name.Contains(query))
                .Take(10)
                .ToListAsync();

            return Ok(teams);
        }

        [HttpGet("matches")]
        public async Task<IActionResult> GetMatches([FromQuery] int? championshipId)
        {
            var query = _context.Matches
                .Include(m => m.Championship)
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .AsQueryable();

            if (championshipId.HasValue)
            {
                query = query.Where(m => m.ChampionshipId == championshipId.Value);
            }

            var matches = await query
                .OrderBy(m => m.MatchDate)
                .ToListAsync();

            return Ok(matches);
        }
    }
}
