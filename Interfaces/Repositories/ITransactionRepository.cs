using ApiEntregasMentoria.Data.Entities;

namespace ApiEntregasMentoria.Interfaces.Repositories
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId);
        Task<Transaction?> GetByIdAndUserAsync(int transactionId, int userId);
    }
}
