﻿using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Postresql.Data;
using System;

namespace Infrastructure.Postresql.Repositories
{
    public class BookRepository(PostreSqlDbContext context) : SqlRepository<Book, Guid>(context), IBookRepository
    {



    }
}
