using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class OrganizationRepository : Repository<Organization>, IRepository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(ParserDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> GetCountAsync()
        {
            return await GetAll().CountAsync();
        }

        public async Task<Organization> GetAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(organization => organization.Id == id);
        }
        public async Task<Organization> GetAsync(string name)
        {
            return await GetAll().FirstOrDefaultAsync(organization => organization.Name.Equals(name));
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
