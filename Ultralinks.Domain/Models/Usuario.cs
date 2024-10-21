using System.ComponentModel.DataAnnotations.Schema;

namespace Ultralinks.Domain.Models
{
    [Table("usuario")]
    public class Usuario : BaseDomain
    {
        [Column("nome_completo")]
        public string NomeCompleto { get; set; }

        [Column("data_nascimento")]
        public DateTime DataNascimento { get; set; }

        [Column("cpf")]
        public string Cpf { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("senha")]
        public string Senha { get; set; }

        public Endereco EnderecoCobranca { get; set; }

        public ICollection<Transacao> TransacoesOrigem { get; set; } = new List<Transacao>();
        public ICollection<Transacao> TransacoesDestino { get; set; } = new List<Transacao>();
    }
}
