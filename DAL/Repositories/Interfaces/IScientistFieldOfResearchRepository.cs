using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IScientistFieldOfResearchRepository : IRepository<ScientistFieldOfResearch>
    {
        Task<int> GetCountAsync();
        Task<ScientistFieldOfResearch> GetAsync(int id);
    }
}
