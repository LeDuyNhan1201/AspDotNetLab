using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class AbstractEntity<TId>
    {

        [Key]
        [Column("id")]
        public TId Id { get; set; }

    }
}
