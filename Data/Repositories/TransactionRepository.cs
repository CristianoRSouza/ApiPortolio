using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApiEntregasMentoria.Data.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(MyContext context) : base(context) { }

        public async Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId)
        {
            return await _meuContexto.Set<Transaction>()
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Transaction?> GetByIdAndUserAsync(int transactionId, int userId)
        {
            return await _meuContexto.Set<Transaction>()
                .FirstOrDefaultAsync(t => t.Id == transactionId && t.UserId == userId);
        }
    }
}
