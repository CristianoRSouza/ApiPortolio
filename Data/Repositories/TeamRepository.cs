using ApiEntregasMentoria.Data.ContextEntity;
using ApiEntregasMentoria.Data.Entities;
using ApiEntregasMentoria.Interfaces.Repositories;

namespace ApiEntregasMentoria.Data.Repositories
{
    public class TeamRepository : BaseRepository<Team>,IRepositoryTeam
    {
        public TeamRepository(MyContext myContext) : base(myContext)
        {
        }
    }
}
