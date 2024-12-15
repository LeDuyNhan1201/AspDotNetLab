using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface ICartDetailRepository
    {

        public Task<List<CartDetail>> GetAllAsync();

        public Task<CartDetail> GetByIdAsync(Guid cartId, Guid bookId);

        public Task CommitChangesAsync();

        public Task CreateAsync(IEnumerable<CartDetail> items);

        public Task<CartDetail> CreateAsync(CartDetail item);

        public CartDetail UpdateAsync(CartDetail item);

        public Task<CartDetail> DeleteAsync(Guid cartId, Guid bookId);

        public Task<ICollection<CartDetail>> FindAsync(Func<CartDetail, bool> predicate);

        public Task<bool> ExistsAsync(Guid cartId, Guid bookId);

        public Task<int> CountAsync();

        public Task<ICollection<CartDetail>> GetAllByCartIdAsync(Guid cartId);

    }
}
