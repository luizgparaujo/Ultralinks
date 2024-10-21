using AutoMapper;
using Ultralinks.Application.Interfaces;
using Ultralinks.Application.ViewModels;
using Ultralinks.Domain.DTOs;
using Ultralinks.Domain.Enums;
using Ultralinks.Domain.Interfaces;
using Ultralinks.Domain.Models;

namespace Ultralinks.Application.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public TransacaoService(ITransacaoRepository transacaoRepository, IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _transacaoRepository = transacaoRepository ?? throw new ArgumentNullException(nameof(transacaoRepository));
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TransacaoDTO> DepositarAsync(CreateTransacaoViewModel transacaoVM, int usuarioIdOrigem)
        {
            try
            {
                // Validações iniciais
                if (usuarioIdOrigem <= 0)
                    throw new Exception("Necessário informar usuário origem.");

                var usuarios = await ObterUsuariosParaTransacaoAsync(usuarioIdOrigem, transacaoVM.UsuarioIdDestino);
                var usuarioDestino = usuarios.FirstOrDefault(x => x.Id == transacaoVM.UsuarioIdDestino);
                if (usuarioDestino == null)
                    throw new Exception("Usuário destino não localizado.");

                var transacao = await MapearDepositoAsync(transacaoVM, usuarioIdOrigem);
                await _transacaoRepository.CreateAsync(transacao);

                return MapearTransacaoDTO(transacao, usuarios);

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao realizar depósito. {ex.Message}");
            }
        }

        public async Task<TransacaoDTO> TransferirAsync(CreateTransacaoViewModel transacaoVM, int usuarioIdOrigem)
        {
            try
            {
                ValidarTransferencia(usuarioIdOrigem, transacaoVM.UsuarioIdDestino);

                var usuarios = await ObterUsuariosParaTransacaoAsync(usuarioIdOrigem, transacaoVM.UsuarioIdDestino);
                var usuarioDestino = usuarios.FirstOrDefault(x => x.Id == transacaoVM.UsuarioIdDestino);
                if (usuarioDestino == null)
                    throw new Exception("Usuário destino não localizado.");

                var transacao = await MapearTransferenciaAsync(transacaoVM, usuarioIdOrigem);

                await _transacaoRepository.CreateAsync(transacao);

                return MapearTransacaoDTO(transacao, usuarios);

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao realizar transferência. {ex.Message}");
            }
        }

        private async Task<Transacao> MapearTransferenciaAsync(CreateTransacaoViewModel transacaoVM, int usuarioIdOrigem)
        {
            var transacao = _mapper.Map<Transacao>(transacaoVM);
            transacao.UsuarioIdOrigem = usuarioIdOrigem;
            transacao.TipoTransacao = TipoTransacao.Transferencia;
            transacao.CodigoAutorizacao = $"TRANSF{await GerarCodigoAutorizacaoAsync(TipoTransacao.Transferencia)}";

            return transacao;
        }

        private async Task<Transacao> MapearDepositoAsync(CreateTransacaoViewModel transacaoVM, int usuarioIdOrigem)
        {
            var transacao = _mapper.Map<Transacao>(transacaoVM);
            transacao.UsuarioIdOrigem = usuarioIdOrigem;
            transacao.TipoTransacao = TipoTransacao.Deposito;
            transacao.CodigoAutorizacao = $"DEP{await GerarCodigoAutorizacaoAsync(TipoTransacao.Deposito)}";

            return transacao;
        }

        private TransacaoDTO MapearTransacaoDTO(Transacao transacao, List<Usuario> usuarios)
        {
            var transacaoDTO = _mapper.Map<TransacaoDTO>(transacao);
            transacaoDTO.NomeUsuarioOrigem = usuarios.FirstOrDefault(x => x.Id == transacao.UsuarioIdOrigem).NomeCompleto;
            transacaoDTO.NomeUsuarioDestino = usuarios.FirstOrDefault(x => x.Id == transacao.UsuarioIdDestino).NomeCompleto;

            return transacaoDTO;
        }

        private void ValidarTransferencia(int usuarioIdOrigem, int usuarioIdDestino)
        {
            if (usuarioIdOrigem <= 0)
                throw new Exception("Necessário informar usuário origem.");

            if (usuarioIdOrigem == usuarioIdDestino)
                throw new Exception("Não é possível transferir para sua própria conta.");
        }

        private async Task<List<Usuario>> ObterUsuariosParaTransacaoAsync(int usuarioIdOrigem, int usuarioIdDestino)
        {
            var idsUsuarios = new List<int> { usuarioIdOrigem, usuarioIdDestino };
            return await _usuarioRepository.GetByIdsAsync(idsUsuarios);
        }

        public async Task<List<TransacaoDTO>> GetAllAsync()
        {
            var transacoes = await _transacaoRepository.GetAllAsync();
            var idsUsuarios = transacoes.SelectMany(x => new[] { x.UsuarioIdOrigem, x.UsuarioIdDestino }).Where(id => id > 0).Distinct().ToList();
            var usuarios = await _usuarioRepository.GetByIdsAsync(idsUsuarios);

            var transacoesDTO = new List<TransacaoDTO>();
            transacoes.ForEach(x => transacoesDTO.Add(MapearTransacaoDTO(x, usuarios)));

            return transacoesDTO;
        }

        public async Task<TransacaoDTO> GetByIdAsync(int id)
        {
            var transacao = await _transacaoRepository.GetByIdAsync(id);
            var idsUsuarios = transacao.UsuarioIdOrigem != transacao.UsuarioIdDestino ? new List<int> { transacao.UsuarioIdOrigem, transacao.UsuarioIdDestino } : new List<int> { transacao.UsuarioIdOrigem };
            var usuarios = await _usuarioRepository.GetByIdsAsync(idsUsuarios);

            return MapearTransacaoDTO(transacao, usuarios);
        }

        private async Task<string> GerarCodigoAutorizacaoAsync(TipoTransacao tipoTransacao)
        {
            var ultimoCodigo = await _transacaoRepository.ObterUltimoCodigoAutorizacaoAsync(tipoTransacao);

            // Extrai o número do último código
            int numero = 0;
            if (!string.IsNullOrEmpty(ultimoCodigo))
                int.TryParse(new string(ultimoCodigo.Where(char.IsDigit).ToArray()), out numero);

            // Incrementa o número para o novo código
            numero++;

            // Retorna apenas o número como string
            return numero.ToString("D4");
        }
    }
}
