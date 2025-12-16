namespace ApiEntregasMentoria.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryUser _UserRepository { get; }
        IRepositoryTeam _TeamRepository { get; }
        IRepositoryMatch _MatchRepository { get; }
        ITransactionRepository _TransactionRepository { get; }
        Task SaveAsync();
    }
}
