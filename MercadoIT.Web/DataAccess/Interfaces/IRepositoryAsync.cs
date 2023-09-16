using System.Linq.Expressions;

namespace MercadoIT.Web.DataAccess.Interfaces
{
    public interface IRepositoryAsync<T> : IDisposable where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includes);
        Task<T> GetByID(int? id);
        Task<T> GetByID(int? id, params Expression<Func<T, object>>[] includes);
        Task<T> Insert(T entity);
        Task<T> Delete(int id);
        Task Update(T entity);
        Task<T> Find(Expression<Func<T, bool>> expr);
        Task<IEnumerable<T>> GetPaged(Expression<Func<T, bool>> filter = null,
                                 Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                 int? skip = null,
                                 int? take = null,
                                 params Expression<Func<T, object>>[] includes);
        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
    }
}
