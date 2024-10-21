using Ultralinks.Application.ViewModels;
using Ultralinks.Domain.DTOs;

namespace Ultralinks.Application.Interfaces
{
    public interface ITransacaoService
    {
        Task<TransacaoDTO> DepositarAsync(CreateTransacaoViewModel transacaoVM, int usuarioIdOrigem);
        Task<TransacaoDTO> TransferirAsync(CreateTransacaoViewModel transacaoVM, int usuarioIdOrigem);
        Task<TransacaoDTO> GetByIdAsync(int id);      
        Task<List<TransacaoDTO>> GetAllAsync();       
    }
}
