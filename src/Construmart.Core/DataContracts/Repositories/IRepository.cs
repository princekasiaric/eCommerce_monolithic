using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Construmart.Core.Commons;

namespace Construmart.Core.DataContracts.Repositories
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        Task<TResult> SingleOrDefaultAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true) where TResult : class;

        Task<T> SingleOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true);

        Task<IEnumerable<T>> AllAsync(Expression<Func<T, object>>[] includes = null);
        Task<IEnumerable<T>> WhereAsync(
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true);

        Task<IEnumerable<TResult>> WhereAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true) where TResult : class;

        Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true);

        Task<TResult> FirstOrDefaultAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true) where TResult : class;

        Task<T> LastOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true);

        Task<TResult> LastOrDefault<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true) where TResult : class;

        Task<PagedList<T>> PaginateAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true);

        Task<PagedList<TResult>> PaginateAsync<TResult>(
            int pageNumber,
            int pageSize,
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, object>>[] includes = null,
            bool withTracking = false,
            Expression<Func<T, object>> orderBy = null,
            bool isOrderAscending = true) where TResult : class;

        Task<T> FindAsync(params long[] ids);
        Task AddAsync(T model);
        Task AddRangeAsync(IReadOnlyList<T> models);
        void Update(T model);
        void Remove(T model);
        Task RemoveRangeAsync(Expression<Func<T, bool>> predicate);
        Task RemoveAsync(params long[] id);
        Task RemoveAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        void UpdateRange(IEnumerable<T> model);
    }
}