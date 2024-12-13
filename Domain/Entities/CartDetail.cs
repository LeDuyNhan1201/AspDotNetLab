using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("cart_details")]
    public class CartDetail
    {
        [Column("cart_id")]
        [Required]
        public Guid CartId { get; set; }

        [Column("book_id")]
        [Required]
        public Guid BookId { get; set; }

        [Column("quantity")]
        [Required]
        public int Quantity { get; set; } = 1;

        [Column("price", TypeName = "decimal(18,2)")]
        [Required]
        public decimal Price { get; set; }

        [ForeignKey(nameof(CartId))]
        public virtual Cart Cart { get; set; }

        [ForeignKey(nameof(BookId))]
        public virtual Book Book { get; set; }
    }

}
