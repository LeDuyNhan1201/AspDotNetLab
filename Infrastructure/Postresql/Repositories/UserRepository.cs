using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Postresql.Data;
using System;

namespace Infrastructure.Postresql.Repositories
{
    public class UserRepository(PostreSqlDbContext context) : SqlRepository<User, Guid>(context), IUserRepository
    {



    }
}
