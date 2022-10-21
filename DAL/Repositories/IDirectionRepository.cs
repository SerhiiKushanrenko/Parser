using DAL.AdditionalModels;
using DAL.Models;

namespace DAL.Repositories
{
    public interface IDirectionRepository : IRepository<Direction>
    {
        Task<int> GetCountAsync();
        Task<Direction> GetAsync(int id);

        Task<Direction> GetAsync(string name);

        Task<List<Direction>> GetDirectionsAsync(DirectionFilter? filter = null);
    }
}
