using DAL.AdditionalModels;
using DAL.Models;

namespace DAL.Repositories
{
    public interface IFieldOfResearchRepository : IRepository<FieldOfResearch>
    {
        Task<int> GetCountAsync();
        Task<FieldOfResearch> GetAsync(int id);

        Task<FieldOfResearch> GetAsync(string title);

        Task<List<FieldOfResearch>> GetFieldsOfResearchAsync(FieldOfResearchFilter? filter = null);
    }
}
