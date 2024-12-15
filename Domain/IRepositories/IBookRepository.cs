using Domain.Entities;
using System;

namespace Domain.IRepositories
{
    public interface IBookRepository : ISqlRepository<Book, Guid>
    {



    }
}
