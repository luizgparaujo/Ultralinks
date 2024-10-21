using System.ComponentModel.DataAnnotations;

namespace Ultralinks.Application.ViewModels
{
    public class CreateTransacaoViewModel
    {
        [Required]
        public int UsuarioIdDestino { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor da transação deve ser maior que zero.")]
        public decimal Valor { get; set; }

        [StringLength(500, ErrorMessage = "A descrição não pode ter mais que 500 caracteres.")]
        public string Descricao { get; set; }
    }
}
