using System;
using System.Collections.Generic;

namespace Domain.IRepositories
{
    public class PagedResult<TItem>
    {

        public List<TItem> Items { get; set; } = [];

        public int TotalItems { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public bool HasPrevious => PageNumber > 1;

        public bool HasNext => PageNumber < TotalPages;

    }

}
