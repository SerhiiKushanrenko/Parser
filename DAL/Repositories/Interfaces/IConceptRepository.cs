using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IConceptRepository : IRepository<Concept>
    {
        Task<int> GetCountAsync();
        Task<Concept> GetAsync(int id);

        Task<Concept> GetAsync(string title);

        // Task<List<Work>> GetFieldsOfResearchAsync(FieldOfResearchFilter? filter = null);
    }
}
