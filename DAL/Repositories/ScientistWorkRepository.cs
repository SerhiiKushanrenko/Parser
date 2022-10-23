using DAL.AdditionalModels;
using DAL.EF;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ScientistWorkRepository : Repository<ScientistWork>, IRepository<ScientistWork>, IScientistWorkRepository
    {
        public ScientistWorkRepository(ParserDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> GetCountAsync()
        {
            return await GetAll().CountAsync();
        }

        public async Task<ScientistWork> GetAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(direction => direction.Id == id);
        }

        public async Task<List<ScientistWork>> GetScientistsFieldsOfResearchAsync(ScientistWorkFilter? filter = null)
        {
            return new List<ScientistWork>();

            //return await GetAll().Where(scientistFieldOfResearch => filter == null || 
            //(!filter.ScientistId.HasValue || (filter.ScientistId == scientistFieldOfResearch.ScientistId)) &&
            //(!filter.FieldOfResearchId.HasValue || (filter.FieldOfResearchId == scientistFieldOfResearch.FieldOfResearchId))
            //).ToListAsync();
        }
    }
}
