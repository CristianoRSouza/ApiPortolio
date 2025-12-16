using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Data.ContextEntity;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace ApiEntregasMentoria.Services
{
    public class ProfileService
    {
        private readonly MyContext _context;

        public ProfileService(MyContext context)
        {
            _context = context;
        }

        public async Task<UserDto?> GetProfileAsync(int userId)
        {
            var user = await _context.Set<User>().FindAsync(userId);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Nickname = user.Nickname,
                FullName = user.FullName,
                Phone = user.Phone,
                Cpf = user.Cpf,
                Balance = user.Balance,
                IsVerified = user.IsVerified,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin
            };
        }

        public async Task<bool> UpdateProfileAsync(int userId, UpdateUserDto request)
        {
            var user = await _context.Set<User>().FindAsync(userId);
            if (user == null) return false;

            if (!string.IsNullOrEmpty(request.FullName))
                user.FullName = request.FullName;

            if (!string.IsNullOrEmpty(request.Phone))
                user.Phone = request.Phone;

            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto request)
        {
            var user = await _context.Set<User>().FindAsync(userId);
            if (user == null) return false;

            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
                throw new InvalidOperationException("Current password is incorrect");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserStatisticsDto> GetStatisticsAsync(int userId)
        {
            var bets = await _context.Set<Bet>()
                .Where(b => b.UserId == userId)
                .ToListAsync();

            var totalBets = bets.Count;
            var wonBets = bets.Count(b => b.Status == "won");
            var lostBets = bets.Count(b => b.Status == "lost");
            var pendingBets = bets.Count(b => b.Status == "pending");
            var totalBetAmount = bets.Sum(b => b.BetAmount);
            var totalWinnings = bets.Where(b => b.Status == "won").Sum(b => b.ResultAmount);
            var winRate = totalBets > 0 ? (decimal)wonBets / totalBets * 100 : 0;
            var roi = totalBetAmount > 0 ? (totalWinnings - totalBetAmount) / totalBetAmount * 100 : 0;

            return new UserStatisticsDto
            {
                TotalBets = totalBets,
                WonBets = wonBets,
                LostBets = lostBets,
                PendingBets = pendingBets,
                TotalBetAmount = totalBetAmount,
                TotalWinnings = totalWinnings,
                WinRate = winRate,
                Roi = roi
            };
        }
    }
}
