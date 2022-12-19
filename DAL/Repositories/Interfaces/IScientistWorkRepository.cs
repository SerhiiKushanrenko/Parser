using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IScientistWorkRepository : IRepository<ScientistWork>
    {
        Task<int> GetCountAsync();
        Task<ScientistWork> GetAsync(int id);

    }
}
