using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ScientistWorkRepository : Repository<ScientistWork>, IScientistWorkRepository
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
    }
}
