using DAL.EF;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ConceptRepository : Repository<Concept>, IRepository<Concept>, IConceptRepository
    {
        public ConceptRepository(ParserDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> GetCountAsync()
        {
            return await GetAll().CountAsync();
        }

        public async Task<Concept> GetAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(concept => concept.Id == id);
        }
        public async Task<Concept> GetAsync(string name)
        {
            return await GetAll().FirstOrDefaultAsync(concept => concept.Name.Equals(name));
        }

        //public async Task<List<Organization>> GetFieldsOfResearchAsync(FieldOfResearchFilter? filter = null)
        //{
        //    return new List<Organization>();

        //    //return await GetAll().Where(direction => filter == null ||
        //    //(string.IsNullOrEmpty(filter.Title) || (filter.Title.Contains(direction.Title) || direction.Title.Contains(filter.Title)))
        //    //).ToListAsync();
        //}
    }
}
