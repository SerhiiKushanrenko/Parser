using BLL.AdditionalModels;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IScientistService
    {
        public Task<Scientist> GetAsync(int id);
        public Task<List<Scientist>> GetScientistsAsync(ScientistFilter filter);
    }
}
