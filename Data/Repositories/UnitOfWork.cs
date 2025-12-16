using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Interfaces.Repositories;

namespace ApiEntregasMentoria.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyContext _context;
        public IRepositoryUser _UserRepository { get; }
        public IRepositoryTeam _TeamRepository { get; }
        public IRepositoryMatch _MatchRepository { get; }
        public ITransactionRepository _TransactionRepository { get; }

        public UnitOfWork(MyContext context, 
                         IRepositoryUser userRepository,
                         IRepositoryTeam teamRepository,
                         IRepositoryMatch matchRepository,
                         ITransactionRepository transactionRepository)
        {
            _context = context;
            _UserRepository = userRepository;
            _TeamRepository = teamRepository;
            _MatchRepository = matchRepository;
            _TransactionRepository = transactionRepository;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
