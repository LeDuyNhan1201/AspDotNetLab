using Domain.Specification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface ISqlRepository<TEntity, TId> where TEntity : class
    {

        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(TId id);

        Task CreateAsync(IEnumerable<TEntity> items);

        Task<TEntity> CreateAsync(TEntity item);

        Task UpdateAsync(TEntity item);

        Task DeleteAsync(TId id);

        Task<List<TEntity>> FindAsync(Func<TEntity, bool> predicate);

        Task<bool> ExistsAsync(TId id);

        Task<int> CountAsync();

        Task<PagedResult<TEntity>> GetPagedAsync(SqlSpecification<TEntity> spec);

    }

}
