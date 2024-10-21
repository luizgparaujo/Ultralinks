using System.ComponentModel.DataAnnotations.Schema;

namespace Ultralinks.Domain.Models
{
    [Table("endereco")]
    public class Endereco : BaseDomain
    {
        [Column("cep")]
        public string Cep { get; set; }

        [Column("complemento")]
        public string Complemento { get; set; }

        [Column("numero")]
        public string Numero { get; set; }

        [Column("logradouro")]
        public string Logradouro { get; set; }

        [Column("bairro")]
        public string Bairro { get; set; }

        [Column("localidade")]
        public string Localidade { get; set; }

        [Column("uf")]
        public string Uf { get; set; }

        [Column("ibge")]
        public string Ibge { get; set; }

        [Column("gia")]
        public string Gia { get; set; }

        [Column("ddd")]
        public string Ddd { get; set; }

        [Column("siafi")]
        public string Siafi { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; } 
    }
}
