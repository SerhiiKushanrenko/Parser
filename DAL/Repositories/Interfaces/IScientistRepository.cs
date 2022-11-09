using DAL.AdditionalModels;
using DAL.Models;
using System.Collections.Concurrent;

namespace DAL.Repositories.Interfaces
{
    public interface IScientistRepository : IRepository<Scientist>
    {
        Task<Scientist> GetAsync(int id);

        Task<Scientist> GetAsync(string name);

        Task<List<Scientist>> GetScientistsListAsync(ScientistFilter? filter = null);

        Task<int> GetScientistsCountAsync(ScientistFilter? filter = null);
        Task<BlockingCollection<Scientist>> GetAllFromFieldOfResearch(List<ScientistFieldOfResearch> listOfScientistFieldOfResearches);
        Task<BlockingCollection<Scientist>> GetAllFromWork(List<ScientistWork> getScientistWork);
    }
}
