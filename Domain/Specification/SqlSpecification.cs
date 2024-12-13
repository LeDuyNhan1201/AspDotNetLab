using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace Domain.Specification
{
    public class SqlSpecification<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity, bool>> Criteria { get; set; }

        public IList<OrderField<TEntity>> OrderFields { get; set; } = [];

        public int Take { get; set; } = 10;

        public int Skip { get; set; } = 0;

        public IList<SearchField<TEntity>> SearchFields { get; set; } = [];

    }
}
