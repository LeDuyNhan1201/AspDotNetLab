using Domain.Entities;
using Domain.Specification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface ISqlRepository<TEntity, TId> where TEntity : AbstractEntity<TId>
    {

        public Task<ICollection<TEntity>> GetAllAsync();

        public Task<TEntity> GetByIdAsync(TId id);

        public Task CommitChangesAsync();

        public Task CreateAsync(IEnumerable<TEntity> items);

        public Task<TEntity> CreateAsync(TEntity item);

        public TEntity UpdateAsync(TEntity item);

        public Task<TEntity> DeleteAsync(TId id);

        public Task<ICollection<TEntity>> FindAsync(Func<TEntity, bool> predicate);

        public Task<bool> ExistsAsync(TId id);

        public Task<int> CountAsync();

        public Task<PagedResult<TEntity>> GetPagedAsync(SqlSpecification<TEntity> spec);

    }

}
