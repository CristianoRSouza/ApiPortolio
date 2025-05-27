using ApiEntregasMentoria.Data.Dto;

namespace ApiEntregasMentoria.Interfaces.Services
{
    public interface IMatchService
    {
        Task<IEnumerable<MatchDto>> GetAllAsync();
        Task<MatchDto> GetByIdAsync(int id);
        Task<MatchDto> CreateAsync(MatchCreateDto dto);
        Task UpdateAsync(int id, MatchUpdateDto dto);
        Task UpdateCornersAsync(int id, int corners);
        Task UpdateFoulsAsync(int id, int fouls);
        Task DeleteAsync(int id);
    }

}
