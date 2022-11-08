using DAL.AdditionalModels;
using DAL.EF;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ScientistWorkRepository : Repository<ScientistWork>, IRepository<ScientistWork>,
                                           IScientistWorkRepository
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


        public bool CheckScientistWorkAsync(ScientistWorkFilter? filter = null)
        {
            return GetAll().Any(e => e.ScientistId.Equals(filter.ScientistId) & e.WorkId.Equals(filter.WorkId));

        }

        public async Task<List<ScientistWork>> GetScientistWorkAsync(ScientistWorkFilter? filter = null)
        {
            return await GetAll()
                .Where(scientistFieldOfResearch => filter == null ||
                                                   (!filter.ScientistId.HasValue || (filter.ScientistId ==
                                                       scientistFieldOfResearch.ScientistId)) &&
                                                   (!filter.WorkId.HasValue ||
                                                    (filter.WorkId == scientistFieldOfResearch.WorkId))
                )
                .ToListAsync();
        }
    }
}
