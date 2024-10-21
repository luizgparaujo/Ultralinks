namespace Ultralinks.Domain.DTOs
{
    public class TransacaoDTO
    {
        public int Id { get; set; }
        public DateTime DataCadastro { get; set; }
        public string NomeUsuarioOrigem { get; set; }
        public string NomeUsuarioDestino { get; set; }
        public decimal Valor { get; set; }
        public string TipoTransacao { get; set; }
        public string Descricao { get; set; }
        public string CodigoAutorizacao { get; set; }
    }
}
