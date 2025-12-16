using ApiEntregasMentoria.Data.Dto;

namespace ApiEntregasMentoria.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetTransactionsByUserAsync(int userId);
        Task<PixPaymentDto> CreateDepositAsync(int userId, DepositRequestDto request);
        Task<decimal> ConfirmDepositAsync(int userId, int transactionId);
        Task CreateWithdrawAsync(int userId, WithdrawRequestDto request);
        Task<decimal> GetUserBalanceAsync(int userId);
        Task<IEnumerable<TransactionDto>> GetTransactionHistoryAsync(int userId, string? type, int page, int pageSize);
    }
}
