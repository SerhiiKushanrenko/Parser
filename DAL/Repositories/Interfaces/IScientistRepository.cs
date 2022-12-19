using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IScientistRepository : IRepository<Scientist>
    {
        Task<Scientist> GetAsync(int id);
        Task<Scientist> GetAsync(string name);
    }
}
