using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IFieldOfResearchRepository : IRepository<FieldOfResearch>
    {
        Task<int> GetCountAsync();
        Task<FieldOfResearch> GetAsync(int id);

        Task<FieldOfResearch> GetAsync(string title);
    }
}