using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Interfaces.Services;
using ApiEntregasMentoria.Interfaces.Repositories;

namespace ApiEntregasMentoria.Services
{
    public class MatchService : IMatchService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MatchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MatchDto>> GetAllAsync()
        {
            var matches = await _unitOfWork._MatchRepository.GetAllIncludeAsync();
            return matches.Select(m => new MatchDto
            {
                Id = m.Id,
                Team1Id = m.Team1Id,
                Team2Id = m.Team2Id,
                Team1Name = m.Team1?.Name ?? "",
                Team2Name = m.Team2?.Name ?? "",
                MatchDate = m.MatchDate,
                Status = m.Status,
                Team1Score = m.Team1Score ?? 0,
                Team2Score = m.Team2Score ?? 0,
                Team1Odd = m.Team1Odd,
                DrawOdd = m.DrawOdd,
                Team2Odd = m.Team2Odd
            });
        }

        public async Task<MatchDto> GetByIdAsync(int id)
        {
            var match = await _unitOfWork._MatchRepository.GetMatchIncludeAsync(id);
            if (match == null) throw new KeyNotFoundException("Match not found");

            return new MatchDto
            {
                Id = match.Id,
                Team1Id = match.Team1Id,
                Team2Id = match.Team2Id,
                Team1Name = match.Team1?.Name ?? "",
                Team2Name = match.Team2?.Name ?? "",
                MatchDate = match.MatchDate,
                Status = match.Status,
                Team1Score = match.Team1Score ?? 0,
                Team2Score = match.Team2Score ?? 0,
                Team1Odd = match.Team1Odd,
                DrawOdd = match.DrawOdd,
                Team2Odd = match.Team2Odd
            };
        }

        public async Task<MatchDto> CreateAsync(MatchCreateDto matchDto)
        {
            var match = new Match
            {
                Team1Id = matchDto.Team1Id,
                Team2Id = matchDto.Team2Id,
                MatchDate = matchDto.MatchDate,
                Status = "scheduled",
                Team1Odd = 2.0m,
                DrawOdd = 3.0m,
                Team2Odd = 2.5m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork._MatchRepository.Add(match);
            await _unitOfWork.SaveAsync();

            return new MatchDto
            {
                Id = match.Id,
                Team1Id = match.Team1Id,
                Team2Id = match.Team2Id,
                MatchDate = match.MatchDate,
                Status = match.Status,
                Team1Odd = match.Team1Odd,
                DrawOdd = match.DrawOdd,
                Team2Odd = match.Team2Odd
            };
        }

        public async Task UpdateAsync(int id, MatchUpdateDto matchDto)
        {
            var match = await _unitOfWork._MatchRepository.Get(id);
            if (match == null) throw new KeyNotFoundException("Match not found");

            match.Team1Score = matchDto.Team1Score;
            match.Team2Score = matchDto.Team2Score;
            match.Status = matchDto.Status;
            match.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork._MatchRepository.UpdateCustomAsync(match);
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork._MatchRepository.Delete(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCornersAsync(int matchId, int corners)
        {
            var match = await _unitOfWork._MatchRepository.Get(matchId);
            if (match == null) throw new KeyNotFoundException("Match not found");

            await _unitOfWork._MatchRepository.UpdateCustomAsync(match);
        }

        public async Task UpdateFoulsAsync(int matchId, int fouls)
        {
            var match = await _unitOfWork._MatchRepository.Get(matchId);
            if (match == null) throw new KeyNotFoundException("Match not found");

            await _unitOfWork._MatchRepository.UpdateCustomAsync(match);
        }
    }
}
