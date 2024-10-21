using Ultralinks.Domain.Enums;
using Ultralinks.Domain.Models;

namespace Ultralinks.Domain.Interfaces
{
    public interface ITransacaoRepository : IBaseRepository<Transacao>
    {
        Task<string> ObterUltimoCodigoAutorizacaoAsync(TipoTransacao tipoTransacao);
    }
}
