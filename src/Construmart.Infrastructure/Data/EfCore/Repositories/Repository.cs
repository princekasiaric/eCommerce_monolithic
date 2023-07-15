using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.Repositories
{
    public class Repository<T> : IRepository<T> where T : ModelBase, IAggregateRoot
    {
        protected internal readonly RepositoryContext context;
        private IQueryable<T> query;

        public Repository(RepositoryContext dbContext)
        {
            context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            query = context.Set<T>().AsQueryable();
        }

        public async Task AddAsync(T model) => await context.Set<T>().AddAsync(model);

        public async Task AddRangeAsync(IReadOnlyList<T> models) => await context.Set<T>().AddRangeAsync(models);

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) => await context.Set<T>().AnyAsync(predicate);

        public async Task<IEnumerable<T>> AllAsync(Expression<Func<T, object>>[] includes = null)
        {
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> WhereAsync(
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true)
        {
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (withTracking)
            {
                query = query.AsTracking();
            }
            if (orderBy != null)
            {
                if (isOrderAscending)
                {
                    query = query.OrderBy(orderBy);
                }
                else
                {
                    query = query.OrderByDescending(orderBy);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TResult>> WhereAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true) where TResult : class
        {
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (withTracking)
            {
                query = query.AsTracking();
            }
            if (orderBy != null)
            {
                if (isOrderAscending)
                {
                    query = query.OrderBy(orderBy);
                }
                else
                {
                    query = query.OrderByDescending(orderBy);
                }
            }
            return await query.Select(selector).ToListAsync();
        }

        public async Task<T> FindAsync(params long[] ids) => await context.Set<T>().FindAsync(ids);

        public async Task<T> SingleOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true)
        {
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (withTracking)
            {
                query = query.AsTracking();
            }
            if (orderBy != null)
            {
                if (isOrderAscending)
                {
                    query = query.OrderBy(orderBy);
                }
                else
                {
                    query = query.OrderByDescending(orderBy);
                }
            }
            return await query.SingleOrDefaultAsync(predicate);
        }

        public async Task<TResult> SingleOrDefaultAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true) where TResult : class
        {
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (withTracking)
            {
                query = query.AsTracking();
            }
            if (orderBy != null)
            {
                if (isOrderAscending)
                {
                    query = query.OrderBy(orderBy);
                }
                else
                {
                    query = query.OrderByDescending(orderBy);
                }
            }
            return await query.Where(predicate).Select(selector).SingleOrDefaultAsync();
        }

        public async Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true)
        {
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (withTracking)
            {
                query = query.AsTracking();
            }
            if (orderBy != null)
            {
                if (isOrderAscending)
                {
                    query = query.OrderBy(orderBy);
                }
                else
                {
                    query = query.OrderByDescending(orderBy);
                }
            }
            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<TResult> FirstOrDefaultAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true) where TResult : class
        {
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (withTracking)
            {
                query = query.AsTracking();
            }
            if (orderBy != null)
            {
                if (isOrderAscending)
                {
                    query = query.OrderBy(orderBy);
                }
                else
                {
                    query = query.OrderByDescending(orderBy);
                }
            }
            return await query.Where(predicate).Select(selector).FirstOrDefaultAsync();
        }

        public async Task<T> LastOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true)
        {
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (withTracking)
            {
                query = query.AsTracking();
            }
            if (orderBy != null)
            {
                if (isOrderAscending)
                {
                    query = query.OrderBy(orderBy);
                }
                else
                {
                    query.OrderByDescending(orderBy);
                }
            }
            return await query.LastOrDefaultAsync(predicate);
        }

        public async Task<TResult> LastOrDefault<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true) where TResult : class
        {
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (withTracking)
            {
                query = query.AsTracking();
            }
            if (orderBy != null)
            {
                if (isOrderAscending)
                {
                    query = query.OrderBy(orderBy);
                }
                else
                {
                    query = query.OrderByDescending(orderBy);
                }
            }
            return await query.Where(predicate).Select(selector).LastOrDefaultAsync();
        }

        public Task<PagedList<T>> PaginateAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true)
        => Task.Run(() =>
        {
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (withTracking)
            {
                query = query.AsTracking();
            }
            if (orderBy != null)
            {
                if (isOrderAscending)
                {
                    query = query.OrderBy(orderBy);
                }
                else
                {
                    query = query.OrderByDescending(orderBy);
                }
            }
            return PagedList<T>.Paginate(query, pageNumber, pageSize);
        });

        public Task<PagedList<TResult>> PaginateAsync<TResult>(
            int pageNumber,
            int pageSize,
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true) where TResult : class
        => Task.Run(() =>
        {
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (withTracking)
            {
                query = query.AsTracking();
            }
            if (orderBy != null)
            {
                if (isOrderAscending)
                {
                    query = query.OrderBy(orderBy);
                }
                else
                {
                    query = query.OrderByDescending(orderBy);
                }
            }
            var selectedQuery = query.Select(selector);
            return PagedList<TResult>.Paginate(selectedQuery, pageNumber, pageSize);
        });

        public void Remove(T model) => context.Set<T>().Remove(model);

        public async Task RemoveAsync(params long[] id) => Remove(await context.Set<T>().FindAsync(id));

        public async Task RemoveAsync(Expression<Func<T, bool>> predicate) => context.Set<T>().Remove(await context.Set<T>().SingleOrDefaultAsync(predicate));

        public async Task RemoveRangeAsync(Expression<Func<T, bool>> predicate) => context.Set<T>().RemoveRange(await context.Set<T>().Where(predicate).ToListAsync());

        public void Update(T model) => context.Update(model);

        public void UpdateRange(IEnumerable<T> model) => context.UpdateRange(model);
    }
}