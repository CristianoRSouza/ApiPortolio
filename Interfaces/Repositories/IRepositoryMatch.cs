using ApiEntregasMentoria.Data.Entities;

namespace ApiEntregasMentoria.Interfaces.Repositories
{
    public interface IRepositoryMatch:IBaseRepository<Match>
    {
        public Task<Match> GetMatchIncludeAsync(int id);

        public Task<IEnumerable<Match>> GetAllIncludeAsync();
        public  Task UpdateCustomAsync(Match entidade);
    }
}
