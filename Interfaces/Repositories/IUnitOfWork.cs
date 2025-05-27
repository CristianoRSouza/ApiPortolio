namespace ApiEntregasMentoria.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        public IRepositoryUser _UserRepository { get; }
        public IRepositoryRoleToken _RoleTokenRepository { get; }
        public IRepositoryMatch _MatchRepository { get; }
        public IRepositoryTeam _TeamRepository { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
