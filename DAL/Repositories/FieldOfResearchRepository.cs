using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class FieldOfResearchRepository : Repository<FieldOfResearch>, IFieldOfResearchRepository
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
            return await GetAll().FirstOrDefaultAsync(fieldOfResearch => fieldOfResearch.Id == id);
        }
        public async Task<FieldOfResearch> GetAsync(string title)
        {
            return await GetAll().FirstOrDefaultAsync(fieldOfResearch => fieldOfResearch.Title.Equals(title));
        }
    }
}
