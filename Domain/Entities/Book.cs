using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("books")]
    public class Book : AbstractEntity<Guid>
    {

        public Book() => Id = Guid.NewGuid();

        [Column("title")]
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }

        [Column("description", TypeName = "text")]
        public string Description { get; set; }

        [Column("price", TypeName = "decimal(18,2)")]
        [Required]
        public decimal Price { get; set; }

        [Column("discount_price", TypeName = "decimal(18,2)")]
        public decimal? DiscountPrice { get; set; }

        [Column("author")]
        [MaxLength(100)]
        [Required]
        public string Author { get; set; }

        [Column("publish_date")]
        [Required]
        public DateTime PublishDate { get; set; }

        public virtual ICollection<CartDetail> CartDetails { get; set; } = [];

        public virtual ICollection<BookCatalogue> BookCatalogues { get; set; } = [];

    }

}
