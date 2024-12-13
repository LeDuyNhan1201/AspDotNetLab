﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("catalogues")]
    public class Catalogue
    {

        [Column("catalogue_id")]
        public Guid CatalogueId { get; set; } = Guid.NewGuid();

        [Column("name")]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<BookCatalogue> BookCatalogues { get; set; } = [];

    }

}