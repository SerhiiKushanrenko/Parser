using DAL.AdditionalModels;
using DAL.EF;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ScientistFieldOfResearchRepository : Repository<ScientistFieldOfResearch>, IRepository<ScientistFieldOfResearch>, IScientistFieldOfResearchRepository
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
            return await GetAll().FirstOrDefaultAsync(direction => direction.Id == id);
        }

        public async Task<List<ScientistFieldOfResearch>> GetScientistsFieldsOfResearchAsync(ScientistFieldOfResearchFilter? filter = null)
        {
            return await GetAll().Where(scientistFieldOfResearch => filter == null || 
            (!filter.ScientistId.HasValue || (filter.ScientistId == scientistFieldOfResearch.ScientistId)) &&
            (!filter.FieldOfResearchId.HasValue || (filter.FieldOfResearchId == scientistFieldOfResearch.FieldOfResearchId))
            ).ToListAsync();
        }
    }
}
