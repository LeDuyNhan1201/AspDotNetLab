using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("book_catalogues")]
    public class BookCatalogue
    {

        [Column("book_id")]
        [Required]
        public Guid BookId { get; set; }

        [Column("catalogue_id")]
        [Required]
        public Guid CatalogueId { get; set; }

        [ForeignKey(nameof(BookId))]
        public virtual Book Book { get; set; }

        [ForeignKey(nameof(CatalogueId))]
        public virtual Catalogue Catalogue { get; set; }

    }

}
