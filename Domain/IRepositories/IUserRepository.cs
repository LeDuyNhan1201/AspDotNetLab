using Domain.Entities;
using System;

namespace Domain.IRepositories
{
    public interface IUserRepository : ISqlRepository<User, Guid>
    {



    }
}
