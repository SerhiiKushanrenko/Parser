using DAL.EF;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class WorkRepository : Repository<Work>, IRepository<Work>, IWorkRepository
    {
        public WorkRepository(ParserDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> GetCountAsync()
        {
            return await GetAll().CountAsync();
        }

        public async Task<Work> GetAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(work => work.Id == id);
        }
        public async Task<Work> GetAsync(string name)
        {
            return await GetAll().FirstOrDefaultAsync(work => work.Name.Equals(name));
        }


    }
}
