using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Interfaces.Repositories;
using ApiEntregasMentoria.Interfaces.Services;
using ApiEntregasMentoria.Shared.Exceptions;
using AutoMapper;

namespace ApiEntregasMentoria.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MatchService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MatchDto>> GetAllAsync()
        {
            var matches = await _unitOfWork._MatchRepository.GetAll();
            return _mapper.Map<IEnumerable<MatchDto>>(matches);
        }

        public async Task<MatchDto> GetByIdAsync(int id)
        {
            var match = await _unitOfWork._MatchRepository.Get(id);
            if (match == null)
                throw new MatchNotFound("Partida não encontrada.");

            return _mapper.Map<MatchDto>(match);
        }

        public async Task<MatchDto> CreateAsync(MatchCreateDto dto)
        {
            var match = _mapper.Map<Match>(dto);

            var homeTeam = await _unitOfWork._TeamRepository.Get(dto.HomeTeamId);
            var awayTeam = await _unitOfWork._TeamRepository.Get(dto.AwayTeamId);

            if (homeTeam == null || awayTeam == null)
                throw new Exception("Time não encontrado.");

            match.HomeTeam = homeTeam;
            match.AwayTeam = awayTeam;
            match.MatchDateTime = DateTime.Now;
            match.HomeGoals = 0;
            match.AwayGoals = 0;


            await _unitOfWork._MatchRepository.Add(match);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<MatchDto>(match);
        }

        public async Task UpdateAsync(int id, MatchUpdateDto dto)
        {
            var match = await _unitOfWork._MatchRepository.Get(id);
            if (match == null)
                throw new MatchNotFound("Partida não encontrada.");

            _mapper.Map(dto, match);

            await _unitOfWork._MatchRepository.UpdateCustomAsync(match);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork._MatchRepository.Delete(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateCornersAsync(int id, int corners)
        {
            var match = await _unitOfWork._MatchRepository.Get(id);
            if (match == null)
                throw new MatchNotFound("Partida não encontrada.");

            match.MatchStats.Corners = corners;

            await _unitOfWork._MatchRepository.Update(match);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateFoulsAsync(int id, int fouls)
        {
            var match = await _unitOfWork._MatchRepository.Get(id);
            if (match == null)
                throw new MatchNotFound("Partida não encontrada.");

            match.MatchStats.Fouls = fouls;

            await _unitOfWork._MatchRepository.Update(match);
            await _unitOfWork.CommitAsync();
        }
    }
}
