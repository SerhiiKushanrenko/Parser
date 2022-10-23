using DAL.AdditionalModels;
using DAL.Models;

namespace DAL.Repositories
{
    public interface IScientistWorkRepository : IRepository<ScientistWork>
    {
        Task<int> GetCountAsync();
        Task<ScientistWork> GetAsync(int id);

        Task<List<ScientistWork>> GetScientistsFieldsOfResearchAsync(ScientistWorkFilter? filter = null);
    }
}
