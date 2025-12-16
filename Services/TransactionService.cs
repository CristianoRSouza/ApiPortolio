using ApiEntregasMentoria.Data.Dto;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace ApiEntregasMentoria.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly MyContext _context;
        private readonly PixService _pixService;

        public TransactionService(MyContext context, PixService pixService)
        {
            _context = context;
            _pixService = pixService;
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByUserAsync(int userId)
        {
            return await _context.Set<Transaction>()
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    Type = t.Type,
                    Amount = t.Amount,
                    Status = t.Status,
                    Description = t.Description,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<PixPaymentDto> CreateDepositAsync(int userId, DepositRequestDto request)
        {
            var user = await _context.Set<User>().FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            var pixKey = GeneratePixKey();
            var qrCodePayload = GenerateQrCode();
            var qrCodeBase64 = _pixService.GeneratePixQrCode(request.Amount, request.Description ?? "Depósito SoccerBet");

            var transaction = new Transaction
            {
                UserId = userId,
                Type = "deposit",
                Amount = request.Amount,
                Status = "pending",
                Description = request.Description,
                QrCode = qrCodePayload,
                PixKey = pixKey,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Set<Transaction>().Add(transaction);
            await _context.SaveChangesAsync();

            return new PixPaymentDto
            {
                TransactionId = transaction.Id,
                QrCode = transaction.QrCode!,
                QrCodeBase64 = qrCodeBase64,
                PixKey = transaction.PixKey!,
                Amount = transaction.Amount,
                ExpiresAt = transaction.ExpiresAt!.Value
            };
        }

        public async Task<decimal> ConfirmDepositAsync(int userId, int transactionId)
        {
            var transaction = await _context.Set<Transaction>()
                .FirstOrDefaultAsync(t => t.Id == transactionId && t.UserId == userId);

            if (transaction == null) throw new KeyNotFoundException("Transaction not found");
            if (transaction.Status != "pending") throw new InvalidOperationException("Transaction already processed");

            var user = await _context.Set<User>().FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            transaction.Status = "completed";
            user.Balance += transaction.Amount;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return user.Balance;
        }

        public async Task CreateWithdrawAsync(int userId, WithdrawRequestDto request)
        {
            var user = await _context.Set<User>().FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            if (user.Balance < request.Amount) throw new InvalidOperationException("Insufficient balance");

            var transaction = new Transaction
            {
                UserId = userId,
                Type = "withdraw",
                Amount = request.Amount,
                Status = "pending",
                Description = request.Description,
                PixKey = request.PixKey,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            user.Balance -= request.Amount;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Set<Transaction>().Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetUserBalanceAsync(int userId)
        {
            var user = await _context.Set<User>().FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user.Balance;
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionHistoryAsync(int userId, string? type, int page, int pageSize)
        {
            var query = _context.Set<Transaction>()
                .Where(t => t.UserId == userId);

            if (!string.IsNullOrEmpty(type))
                query = query.Where(t => t.Type == type);

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    Type = t.Type,
                    Amount = t.Amount,
                    Status = t.Status,
                    Description = t.Description,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();
        }

        private string GenerateQrCode() => Guid.NewGuid().ToString("N")[..20];
        private string GeneratePixKey() => $"pix-{Guid.NewGuid().ToString("N")[..10]}";
    }
}
