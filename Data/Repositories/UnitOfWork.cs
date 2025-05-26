using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace ApiEntregasMentoria.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IRepositoryUser _UserRepository {  get;}
        public IRepositoryRoleToken _RoleTokenRepository {  get;}
        public MyContext _myContext { get; set; }
        private IDbContextTransaction? _transaction;

        public UnitOfWork(MyContext myContext, IRepositoryUser userRepository, IRepositoryRoleToken repositoryRoleToken)
        {
            _UserRepository = userRepository;
            _RoleTokenRepository = repositoryRoleToken;
            _myContext = myContext;
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _myContext.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitAsync()
        {
            await _myContext.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
