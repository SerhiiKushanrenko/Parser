using DAL.AdditionalModels;
using DAL.EF;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ScientistSocialNetworkRepository : Repository<ScientistSocialNetwork>, IRepository<ScientistSocialNetwork>, IScientistSocialNetworkRepository
    {
        public ScientistSocialNetworkRepository(ParserDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> GetCountAsync()
        {
            return await GetAll().CountAsync();
        }

        public async Task<ScientistSocialNetwork> GetAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(scientistSocialNetwork => scientistSocialNetwork.Id == id);
        }

        public async Task<List<ScientistSocialNetwork>> GetScientistsSocialNetworksAsync(ScientistSocialNetworkFilter? filter = null)
        {
            return await GetAll().Where(scientistSocialNetwork => filter == null ||
            (!filter.Type.HasValue || filter.Type == scientistSocialNetwork.Type) &&
            (!filter.ScientistId.HasValue || filter.ScientistId == scientistSocialNetwork.ScientistId) &&
            (string.IsNullOrEmpty(filter.SocialNetworkScientistId) || filter.SocialNetworkScientistId.Equals(scientistSocialNetwork.SocialNetworkScientistId))
            ).ToListAsync();
        }
    }
}
