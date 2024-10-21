using Ultralinks.Domain.Models;

namespace Ultralinks.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : BaseDomain
    {
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetByIdsAsync(IEnumerable<int> ids);
        Task<List<T>> GetAllAsync();
    }
}
