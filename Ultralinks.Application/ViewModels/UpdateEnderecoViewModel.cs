using System.ComponentModel.DataAnnotations;

namespace Ultralinks.Application.ViewModels
{
    public class UpdateEnderecoViewModel
    {
        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "O CEP deve ter 8 caracteres.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "O CEP deve conter apenas números.")]
        public string Cep { get; set; }

        public string Complemento { get; set; }

        [Required(ErrorMessage = "O número é obrigatório.")]
        public string Numero { get; set; }        
    }
}
