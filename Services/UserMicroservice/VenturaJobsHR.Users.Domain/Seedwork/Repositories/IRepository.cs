using System.Linq.Expressions;
using VenturaJobsHR.Users.CrossCutting.Pagination;

namespace VenturaJobsHR.Users.Domain.Seedwork.Repositories;

public interface IRepository<T> where T : class
{
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string id);
    Task BulkInsertAsync(List<T> entities);
    Task<T> GetByIdAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter);
    Task<Pagination<T>> FindByFilterAsync(Expression<Func<T, bool>> filter, Page pagination);
}
