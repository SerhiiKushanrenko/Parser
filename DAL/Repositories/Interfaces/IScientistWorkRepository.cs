using DAL.AdditionalModels;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IScientistWorkRepository : IRepository<ScientistWork>
    {
        Task<int> GetCountAsync();
        Task<ScientistWork> GetAsync(int id);

        bool CheckScientistWorkAsync(ScientistWorkFilter? filter = null);
        Task<List<ScientistWork>> GetScientistsFieldsOfResearchAsync(ScientistWorkFilter? filter = null);
    }
}
