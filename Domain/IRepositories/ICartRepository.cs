using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface ICartRepository : ISqlRepository<Cart, Guid>
    {

        public Task<ICollection<Cart>> GetAllByUserId(Guid userId);

        public Task<int> CountByUserId(Guid userId);

    }
}
