using System;
using System.Linq.Expressions;

namespace Domain.Specification
{
    public class OrderField<TEntity>
    {

        public Expression<Func<TEntity, object>> Field { get; set; }

        public SortDirection Direction { get; set; }

    }
}
