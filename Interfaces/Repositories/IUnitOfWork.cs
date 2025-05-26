namespace ApiEntregasMentoria.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        public IRepositoryUser _UserRepository { get; }
        public IRepositoryRoleToken _RoleTokenRepository { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
