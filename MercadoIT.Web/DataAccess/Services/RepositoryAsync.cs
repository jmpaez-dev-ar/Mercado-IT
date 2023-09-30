using MercadoIT.Web.DataAccess.Interfaces;
using MercadoIT.Web.Entities;
using MercadoIT.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MercadoIT.Web.DataAccess.Services
{
	public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        private readonly NorthwindContext context;

        public RepositoryAsync(NorthwindContext context)
        {
            this.context = context;
        }

        protected DbSet<T> EntitySet
        {
            get
            {
                return context.Set<T>();
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await EntitySet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = EntitySet;
            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<T> GetById(object? id)
        {
            return await EntitySet.FindAsync(id);
        }

        public async Task<bool> ExistById(object? id)
        {
            var entity = await GetById(id);
            return entity != null;
        }

        public async Task<T> GetById(int? id, params Expression<Func<T, object>>[] includes)
        {
            if (id == null)
                return null;

            if (includes == null || !includes.Any())
            {
                return await EntitySet.FindAsync(id);
            }
            else
            {
                IQueryable<T> query = EntitySet;

                foreach (var include in includes)
                    query = query.Include(include);

                return await query.SingleOrDefaultAsync(e => e == EntitySet.Find(id));
            }
        }

        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> whereCondition = null,
                                  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                  string includeProperties = "")
        {
            IQueryable<T> query = EntitySet;

            if (whereCondition != null)
            {
                query = query.Where(whereCondition);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<T> Insert(T entity)
        {
            EntitySet.Add(entity);
            await Save();
            return entity;
        }

        public async Task<T> Delete(object id)
        {
            T entity = await EntitySet.FindAsync(id);
            EntitySet.Remove(entity);
            await Save();
            return entity;
        }

        public async Task Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await Save();
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task<T> Find(Expression<Func<T, bool>> expr)
        {
            return await EntitySet.AsNoTracking().FirstOrDefaultAsync(expr);
        }

        public async Task<IEnumerable<T>> GetPaged(Expression<Func<T, bool>> filter = null,
                                                   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                   int? skip = null,
                                                   int? take = null,
                                                   params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = EntitySet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var include in includes)
                query = query.Include(include);

            if (orderBy != null)
                query = orderBy(query);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return await query.ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = EntitySet;

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }


        public async Task<PagedResult<T>> GetPagedDataAsync(Expression<Func<T, bool>> filter, int start, int length, string orderBy, bool ascending)
        {
            var query = ApplyFilter(context.Set<T>(), filter);
            query = ApplySorting(query, orderBy, ascending);
            var data = await ApplyPagination(query, start, length);
            var totalRecords = await GetTotalRecords(query);
            return CreatePagedResult(data, totalRecords);
        }

        private IQueryable<T> ApplyFilter(IQueryable<T> query, Expression<Func<T, bool>> filter)
        {
            return query.Where(filter);
        }

        private IQueryable<T> ApplySorting(IQueryable<T> query, string orderBy, bool ascending)
        {
            var type = typeof(T);
            var property = type.GetProperty(orderBy);
            if (property != null)
            {
                var parameter = Expression.Parameter(type, "p");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);

                var method = ascending ? "OrderBy" : "OrderByDescending";
                query = CallGenericMethod(query, method, new Type[] { type, property.PropertyType }, orderByExp);
            }
            return query;
        }

        private async Task<IEnumerable<T>> ApplyPagination(IQueryable<T> query, int start, int length)
        {
            return await query.Skip(start).Take(length).ToListAsync();
        }

        private async Task<int> GetTotalRecords(IQueryable<T> query)
        {
            return await query.CountAsync();
        }

        private PagedResult<T> CreatePagedResult(IEnumerable<T> data, int totalRecords)
        {
            return new PagedResult<T>
            {
                Data = data,
                TotalRecords = totalRecords
            };
        }

        private IQueryable<T> CallGenericMethod(IQueryable<T> query, string methodName, Type[] typeArgs, LambdaExpression lambda)
        {
            var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == 2);
            if (method == null) return null;

            var genericMethod = method.MakeGenericMethod(typeArgs);
            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, lambda });
        }


        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
