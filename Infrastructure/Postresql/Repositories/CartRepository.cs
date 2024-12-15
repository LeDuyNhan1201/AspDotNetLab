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
    public class CartRepository(PostreSqlDbContext context) : SqlRepository<Cart, Guid>(context), ICartRepository
    {
        public async Task<ICollection<Cart>> GetAllByUserId(Guid userId)
            => await DbSet.Where(c => c.UserId == userId).ToListAsync();

        public async Task<int> CountByUserId(Guid userId)
            => await DbSet.CountAsync(c => c.UserId == userId);
    }
}
