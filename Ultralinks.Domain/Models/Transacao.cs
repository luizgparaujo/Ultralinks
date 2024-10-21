using System.ComponentModel.DataAnnotations.Schema;
using Ultralinks.Domain.Enums;

namespace Ultralinks.Domain.Models
{
    [Table("transacao")]
    public class Transacao : BaseDomain
    {
        [Column("usuario_id_origem")]
        public int UsuarioIdOrigem { get; set; }

        [Column("usuario_id_destino")]
        public int UsuarioIdDestino { get; set; }

        [Column("valor")]
        public decimal Valor { get; set; }

        [Column("tipo")]
        public TipoTransacao TipoTransacao { get; set; }

        [Column("descricao")]
        public string Descricao { get; set; }

        [Column("codigo_autorizacao")]
        public string CodigoAutorizacao { get; set; }

        public Usuario UsuarioOrigem { get; set; }
        public Usuario UsuarioDestino { get; set; }
    }
}
