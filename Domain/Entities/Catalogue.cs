using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("catalogues")]
    public class Catalogue : AbstractEntity<Guid>
    {
        public Catalogue() => Id = Guid.NewGuid();

        [Column("name")]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}
