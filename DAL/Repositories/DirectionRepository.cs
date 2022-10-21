using DAL.AdditionalModels;
using DAL.EF;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class DirectionRepository : Repository<Direction>, IRepository<Direction>, IDirectionRepository
    {
        public DirectionRepository(ParserDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> GetCountAsync()
        {
            return await GetAll().CountAsync();
        }

        public async Task<Direction> GetAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(direction => direction.Id == id);
        }
        public async Task<Direction> GetAsync(string name)
        {
            return await GetAll().FirstOrDefaultAsync(direction => direction.Name.Equals(name));
        }

        public async Task<List<Direction>> GetDirectionsAsync(DirectionFilter? filter = null)
        {
            return await GetAll().Where(direction => filter == null || 
            (string.IsNullOrEmpty(filter.Name) || (filter.Name.Contains(direction.Name) || direction.Name.Contains(filter.Name)))
            ).ToListAsync();
        }
    }
}
