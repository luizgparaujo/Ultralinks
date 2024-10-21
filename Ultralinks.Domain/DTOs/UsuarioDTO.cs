namespace Ultralinks.Domain.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public EnderecoDTO EnderecoCobranca { get; set; }
    }
}
