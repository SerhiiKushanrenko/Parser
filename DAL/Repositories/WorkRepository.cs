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
            return await GetAll().FirstOrDefaultAsync(direction => direction.Id == id);
        }
        public async Task<Work> GetAsync(string name)
        {
            return await GetAll().FirstOrDefaultAsync(direction => direction.Name.Equals(name));
        }

        //public async Task<List<Work>> GetFieldsOfResearchAsync(FieldOfResearchFilter? filter = null)
        //{
        //    return new List<Work>();

        //    //return await GetAll().Where(direction => filter == null ||
        //    //(string.IsNullOrEmpty(filter.Title) || (filter.Title.Contains(direction.Title) || direction.Title.Contains(filter.Title)))
        //    //).ToListAsync();
        //}
    }
}
