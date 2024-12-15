using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    [Table("users")]
    public class User : IdentityUser<Guid>
    {
        [Column("address", TypeName = "text")]
        public string Address { get; set; }
    }
}
