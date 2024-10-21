using System.ComponentModel.DataAnnotations;

namespace Ultralinks.Application.ViewModels
{
    public class UpdateUsuarioViewModel
    {
        [Required(ErrorMessage = "O id é obrigatório.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "A data nascimento é obrigatória.")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O cpf é obrigatório.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter exatamente 11 dígitos numéricos.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public string Email { get; set; }

        public UpdateEnderecoViewModel EnderecoCobranca { get; set; }
    }
}
