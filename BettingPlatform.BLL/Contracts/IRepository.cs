using BettingPlatform.DAL.Entities;

namespace BettingPlatform.BLL.Contracts
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetById(string id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task AddRangeAsync(List<T> entity);
        void Delete(T entity);
        Task DeleteAll();
        Task SaveChangesAsync();
        Task<List<Match>> GetMatchesAsync();
    }
}
