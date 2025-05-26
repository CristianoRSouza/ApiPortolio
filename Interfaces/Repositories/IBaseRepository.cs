namespace ApiEntregasMentoria.Interfaces.Repositories
{
    public interface IBaseRepository<Tentity>
    {
        Task<Tentity> Get(int id);
        Task<IEnumerable<Tentity>> GetAll();
        Task Add(Tentity entidade);
        Task Delete(int Id);
        Task Update(Tentity entidade);
    }
}
