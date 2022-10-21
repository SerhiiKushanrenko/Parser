using DAL.AdditionalModels;
using DAL.EF;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class FieldOfResearchRepository : Repository<FieldOfResearch>, IRepository<FieldOfResearch>, IFieldOfResearchRepository
    {
        public FieldOfResearchRepository(ParserDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> GetCountAsync()
        {
            return await GetAll().CountAsync();
        }

        public async Task<FieldOfResearch> GetAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(direction => direction.Id == id);
        }
        public async Task<FieldOfResearch> GetAsync(string name)
        {
            return await GetAll().FirstOrDefaultAsync(direction => direction.Title.Equals(name));
        }

        public async Task<List<FieldOfResearch>> GetFieldsOfResearchAsync(FieldOfResearchFilter? filter = null)
        {
            return await GetAll().Where(direction => filter == null || 
            (string.IsNullOrEmpty(filter.Title) || (filter.Title.Contains(direction.Title) || direction.Title.Contains(filter.Title)))
            ).ToListAsync();
        }
    }
}
