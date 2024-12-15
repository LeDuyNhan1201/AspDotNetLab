using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Postresql.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Postresql.Repositories
{
    public class CartDetailRepository(PostreSqlDbContext context) : ICartDetailRepository
    {

        protected readonly DbSet<CartDetail> DbSet = context.Set<CartDetail>();

        public async Task<List<CartDetail>> GetAllAsync() => await DbSet.ToListAsync();

        public async Task<CartDetail> GetByIdAsync(Guid cartId, Guid bookId) 
            => await DbSet.Where(cartDetail => cartDetail.CartId.Equals(cartId) && cartDetail.BookId.Equals(bookId)).FirstOrDefaultAsync();

        public async Task CommitChangesAsync() => await context.SaveChangesAsync();

        public virtual async Task CreateAsync(IEnumerable<CartDetail> items)
        {
            ArgumentNullException.ThrowIfNull(items);
            await DbSet.AddRangeAsync(items);
        }

        public async Task<CartDetail> CreateAsync(CartDetail item)
        {
            ArgumentNullException.ThrowIfNull(item);
            await DbSet.AddAsync(item);
            return item;
        }

        public CartDetail UpdateAsync(CartDetail item)
        {
            ArgumentNullException.ThrowIfNull(item);
            DbSet.Attach(item);
            context.Entry(item).State = EntityState.Modified;
            return item;
        }

        public async Task<CartDetail> DeleteAsync(Guid cartId, Guid bookId)
        {
            var entity = await GetByIdAsync(cartId, bookId) 
                ?? throw new KeyNotFoundException($"Entity with cart id {cartId} & book id {bookId} not found.");
            DbSet.Remove(entity);
            return entity;
        }

        public async Task<ICollection<CartDetail>> FindAsync(Func<CartDetail, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            return await Task.FromResult(DbSet.AsEnumerable().Where(predicate).ToList());
        }

        public async Task<bool> ExistsAsync(Guid cartId, Guid bookId)
        {
            var entity = await GetByIdAsync(cartId, bookId);
            return entity != null;
        }

        public async Task<int> CountAsync() => await DbSet.CountAsync();

        public async Task<ICollection<CartDetail>> GetAllByCartIdAsync(Guid cartId)
            => await DbSet.Where(cd => cd.CartId == cartId).ToListAsync();

    }
}
