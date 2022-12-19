using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ScientistRepository : Repository<Scientist>, IScientistRepository
    {
        public ScientistRepository(ParserDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Scientist> GetAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(scientist => scientist.Id == id);
        }

        public async Task<Scientist> GetAsync(string name)
        {
            return await GetAll().FirstOrDefaultAsync(scientist => scientist.Name.Equals(name));
        }
    }
}

