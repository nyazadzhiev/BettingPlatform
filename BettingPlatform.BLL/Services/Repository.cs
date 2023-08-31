using BettingPlatform.BLL.Contracts;
using BettingPlatform.DAL;
using BettingPlatform.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BettingPlatform.BLL.Services
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly SportsDbContext _dbContext;
        public Repository(SportsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(string id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task DeleteAll()
        {
            _dbContext.Set<T>().RemoveRange(await _dbContext.Set<T>().ToListAsync());
        }

        public async Task AddRangeAsync(List<T> entity)
        {
            await _dbContext.Set<T>().AddRangeAsync(entity);
        }

        public async Task<List<Match>> GetMatchesAsync()
        {
            return await _dbContext.Matches
                .Where(match => match.StartDate >= DateTime.Now && match.StartDate <= DateTime.Now.AddHours(24))
                .ToListAsync();
        }

    }
}
