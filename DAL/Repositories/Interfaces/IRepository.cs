namespace DAL.Repositories.Interfaces
{
    public interface IRepository<EntType>
    {
        IQueryable<EntType> GetAll();
        Task<int> CreateAsync(EntType entity);
        Task<int> CreateAsync(IEnumerable<EntType> entities);
        Task<int> UpdateAsync(EntType entity);
        Task<int> UpdateAsync(IEnumerable<EntType> entities);
        Task<int> DeleteAsync(EntType entity);
        Task<int> DeleteAsync(IEnumerable<EntType> entities);
    }
}
