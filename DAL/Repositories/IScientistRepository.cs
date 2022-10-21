using DAL.AdditionalModels;
using DAL.Models;

namespace DAL.Repositories
{
    public interface IScientistRepository : IRepository<Scientist>
    {
        Task<Scientist> GetAsync(int id);

        Task<Scientist> GetAsync(string name);

        Task<List<Scientist>> GetScientistsListAsync(ScientistFilter? filter = null);

        Task<int> GetScientistsCountAsync(ScientistFilter? filter = null);
    }
}
