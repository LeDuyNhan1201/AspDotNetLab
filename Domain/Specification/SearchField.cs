using System;
using System.Linq.Expressions;

namespace Domain.Specification
{
    public class SearchField<TEntity>
    {

        public Expression<Func<TEntity, object>> Field { get; set; }

        public string SearchTerm { get; set; }

    }

}
