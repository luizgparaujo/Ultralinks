using Microsoft.EntityFrameworkCore;
using Ultralinks.Domain.Enums;
using Ultralinks.Domain.Interfaces;
using Ultralinks.Domain.Models;

namespace Ultralinks.Data.Repositories
{
    public class TransacaoRepository : BaseRepository<Transacao>, ITransacaoRepository
    {
        public TransacaoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<string> ObterUltimoCodigoAutorizacaoAsync(TipoTransacao tipoTransacao)
        {            
            var ultimoCodigo = await _context.Transacoes
                .Where(t => t.TipoTransacao == tipoTransacao) 
                .OrderByDescending(t => t.CodigoAutorizacao)
                .Select(t => t.CodigoAutorizacao)
                .FirstOrDefaultAsync();

            return ultimoCodigo;
        }
    }
}
