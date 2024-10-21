using AutoMapper;
using Ultralinks.Application.Interfaces;
using Ultralinks.Application.ViewModels;
using Ultralinks.Domain.DTOs;
using Ultralinks.Domain.Interfaces;
using Ultralinks.Domain.Models;

namespace Ultralinks.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly IViaCepService _viaCepService;

        public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper, IAutenticacaoService autenticacaoService, IViaCepService viaCepService)
        {
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _autenticacaoService = autenticacaoService ?? throw new ArgumentNullException(nameof(autenticacaoService));
            _viaCepService = viaCepService ?? throw new ArgumentNullException(nameof(viaCepService));
        }

        public async Task<UsuarioDTO> CreateAsync(CreateUsuarioViewModel usuarioVM)
        {
            try
            {
                if (!ValidarCpf(usuarioVM.Cpf))
                    throw new Exception($"CPF '{usuarioVM.Cpf}' inválido.");

                await VerificarUsuarioExistente(usuarioVM.Cpf, usuarioVM.Email);
                usuarioVM.Senha = _autenticacaoService.CriptografarSenha(usuarioVM.Senha);

                var usuario = _mapper.Map<Usuario>(usuarioVM);
                await CompletarDadosEnderecoAsync(usuario);
                await _usuarioRepository.CreateAsync(usuario);

                return _mapper.Map<UsuarioDTO>(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar o usuário. {ex.Message}");
            }
        }

        public async Task<UsuarioDTO> GetByIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);

            return _mapper.Map<UsuarioDTO>(usuario);
        }

        public async Task<UsuarioDTO> UpdateAsync(UpdateUsuarioViewModel usuarioVM)
        {
            try
            {
                await VerificarUsuarioExistente(usuarioVM.Cpf, usuarioVM.Email, usuarioVM.Id);

                var entity = await _usuarioRepository.GetByIdAsync(usuarioVM.Id);
                var usuario = _mapper.Map<Usuario>(usuarioVM);

                // Persistência de dados
                usuario.Senha = entity.Senha;                
                usuario.EnderecoCobranca.Id = entity.EnderecoCobranca.Id;

                await CompletarDadosEnderecoAsync(usuario);
                await _usuarioRepository.UpdateAsync(usuario);

                return _mapper.Map<UsuarioDTO>(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar o usuário. {ex.Message}");
            }
        }

        public async Task<List<UsuarioDTO>> GetAllAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return _mapper.Map<List<UsuarioDTO>>(usuarios);
        }

        public async Task DeleteAsync(int id)
        {            
            await _usuarioRepository.DeleteAsync(id);
        }

        public async Task PutSenhaAsync(int idUsuario, string senha)
        {
            if (idUsuario <= 0)
                throw new Exception("Necessário informar um idUsuario válido.");
            if (string.IsNullOrEmpty(senha))
                throw new Exception("Necessário informar uma senha.");

            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(idUsuario);
                usuario.Senha = _autenticacaoService.CriptografarSenha(senha);

                await _usuarioRepository.UpdateAsync(usuario);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task CompletarDadosEnderecoAsync(Usuario usuario)
        {
            if (usuario == null || usuario.EnderecoCobranca == null)
                throw new Exception("Usuário ou endereço de cobrança não podem ser nulos.");

            try
            {
                var viaCepResponse = await _viaCepService.GetAddressByCepAsync(usuario.EnderecoCobranca.Cep);
                if (viaCepResponse == null)
                    throw new Exception("Resposta da API ViaCep veio nula.");

                usuario.EnderecoCobranca.Logradouro = viaCepResponse.Logradouro;
                usuario.EnderecoCobranca.Bairro = viaCepResponse.Bairro;
                usuario.EnderecoCobranca.Localidade = viaCepResponse.Localidade;
                usuario.EnderecoCobranca.Uf = viaCepResponse.Uf;
                usuario.EnderecoCobranca.Ibge = viaCepResponse.Ibge;
                usuario.EnderecoCobranca.Gia = viaCepResponse.Gia;
                usuario.EnderecoCobranca.Ddd = viaCepResponse.Ddd;
                usuario.EnderecoCobranca.Siafi = viaCepResponse.Siafi;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao completar dados do endereço de cobrança. {ex.Message}");
            }
        }

        private async Task VerificarUsuarioExistente(string cpf, string email, int id = 0)
        {
            if (string.IsNullOrEmpty(cpf) || string.IsNullOrEmpty(email))
                throw new Exception("Necessário informar CPF e Email.");

            var usuario = await _usuarioRepository.GetByCpfOrEmailAsync(cpf, email, id);
            if (usuario != null)
            {
                if (usuario.Cpf == cpf)
                    throw new Exception($"Usuário com este CPF '{cpf}' já está cadastrado.");

                if (usuario.Email == email)
                    throw new Exception($"Usuário com este Email '{email}' já está cadastrado.");
            }
        }

        private string RemoverCaracteresNaoNumericos(string str)
        {
            return str.Replace(".", "").Replace("-", "").Trim();
        }

        private bool ValidarCpf(string cpf)
        {
            // Verifica se o CPF tem 11 dígitos
            if (cpf.Length != 11 || !long.TryParse(cpf, out _))
                return false;

            // Verifica se todos os dígitos são iguais (ex: 111.111.111-11)
            if (cpf.All(c => c == cpf[0]))
                return false;

            // Calcula o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += (10 - i) * (cpf[i] - '0');
            }

            int primeiroDigitoVerificador = 11 - (soma % 11);
            if (primeiroDigitoVerificador >= 10)
                primeiroDigitoVerificador = 0;

            // Verifica se o primeiro dígito está correto
            if (primeiroDigitoVerificador != (cpf[9] - '0'))
                return false;

            // Calcula o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += (11 - i) * (cpf[i] - '0');
            }

            int segundoDigitoVerificador = 11 - (soma % 11);
            if (segundoDigitoVerificador >= 10)
                segundoDigitoVerificador = 0;

            // Verifica se o segundo dígito está correto
            return segundoDigitoVerificador == (cpf[10] - '0');
        }
    }
}
