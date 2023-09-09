using MercadoIT.Web.DataAccess.Interfaces;
using MercadoIT.Web.Entities;
using MercadoIT.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MercadoIT.Web.DataAccess.Servicios
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        private readonly NorthwindContext context;

        public RepositoryAsync(NorthwindContext context)
        {
            this.context = context;
        }
        //public RepositoryAsync(NegocioContext context)
        //{
        //    this.context = context;
        //}

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

        public async Task<T> GetByID(int? id)
        {
            return await EntitySet.FindAsync(id);
        }

        public async Task<T> Insert(T entity)
        {
            EntitySet.Add(entity);
            await Save();
            return entity;
        }

        public async Task<T> Delete(int id)
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


    }
}
