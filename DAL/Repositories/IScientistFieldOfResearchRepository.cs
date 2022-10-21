using DAL.AdditionalModels;
using DAL.Models;

namespace DAL.Repositories
{
    public interface IScientistFieldOfResearchRepository : IRepository<ScientistFieldOfResearch>
    {
        Task<int> GetCountAsync();
        Task<ScientistFieldOfResearch> GetAsync(int id);

        Task<List<ScientistFieldOfResearch>> GetScientistsFieldsOfResearchAsync(ScientistFieldOfResearchFilter? filter = null);
    }
}
