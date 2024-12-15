using Domain.Entities;
using Domain.Specification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IUserRepository
    {

        public Task<ICollection<User>> GetAllAsync();

        public Task<User> GetByIdAsync(Guid id);

        public Task CommitChangesAsync();

        public Task CreateAsync(IEnumerable<User> items);

        public Task<User> CreateAsync(User item);

        public User UpdateAsync(User item);

        public Task<User> DeleteAsync(Guid id);

        public Task<ICollection<User>> FindAsync(Func<User, bool> predicate);

        public Task<bool> ExistsAsync(Guid id);

        public Task<int> CountAsync();

        public Task<PagedResult<User>> GetPagedAsync(SqlSpecification<User> spec);

    }
}
