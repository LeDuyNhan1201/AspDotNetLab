using System;

namespace Application.Contracts
{
    public sealed record BookResponse
    {

        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public DateTime PublishDate { get; set; }

        public string[] Catalogues { get; set; }

    }
}
