using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiEntregasMentoria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _service;

        public MatchController(IMatchService service)
        {
            _service = service;
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
        public async Task<IActionResult> Create([FromBody] MatchCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MatchUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpPatch("{id}/corners")]
        public async Task<IActionResult> UpdateCorners(int id, [FromQuery] int corners)
        {
            await _service.UpdateCornersAsync(id, corners);
            return NoContent();
        }

        [HttpPatch("{id}/fouls")]
        public async Task<IActionResult> UpdateFouls(int id, [FromQuery] int fouls)
        {
            await _service.UpdateFoulsAsync(id, fouls);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

}
