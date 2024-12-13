using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("users")]
    public class User : IdentityUser<Guid>
    {

        [Column("address", TypeName = "text")]
        public string Address { get; set; }

        public virtual ICollection<Cart> Carts { get; set; } = [];

    }
}
