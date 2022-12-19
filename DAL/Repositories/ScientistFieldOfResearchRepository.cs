using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ScientistFieldOfResearchRepository : Repository<ScientistFieldOfResearch>, IScientistFieldOfResearchRepository
    {
        public ScientistFieldOfResearchRepository(ParserDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> GetCountAsync()
        {
            return await GetAll().CountAsync();
        }

        public async Task<ScientistFieldOfResearch> GetAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(fieldOfResearch => fieldOfResearch.Id == id);
        }
    }
}
