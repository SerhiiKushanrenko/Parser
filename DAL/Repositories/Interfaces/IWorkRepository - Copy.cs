using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        Task<int> GetCountAsync();
        Task<Organization> GetAsync(int id);

        Task<Organization> GetAsync(string title);

        // Task<List<Work>> GetFieldsOfResearchAsync(FieldOfResearchFilter? filter = null);
    }
}
