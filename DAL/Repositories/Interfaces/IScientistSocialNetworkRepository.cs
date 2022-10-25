using DAL.AdditionalModels;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IScientistSocialNetworkRepository : IRepository<ScientistSocialNetwork>
    {
        Task<int> GetCountAsync();
        Task<ScientistSocialNetwork> GetAsync(int id);

        Task<List<ScientistSocialNetwork>> GetScientistsSocialNetworksAsync(ScientistSocialNetworkFilter? filter = null);
    }
}
