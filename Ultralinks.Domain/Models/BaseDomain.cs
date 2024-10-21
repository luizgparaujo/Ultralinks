using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultralinks.Domain.Models
{
    public abstract class BaseDomain
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("data_cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        [Column("data_alteracao")]
        public DateTime? DataAlteracao { get; set; }
    }
}
