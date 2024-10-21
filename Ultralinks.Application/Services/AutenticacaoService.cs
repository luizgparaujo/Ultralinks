using Microsoft.AspNetCore.Identity;
using Ultralinks.Application.Interfaces;
using Ultralinks.Domain.Interfaces;

namespace Ultralinks.Application.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly PasswordHasher<object> _passwordHasher;
        private readonly IUsuarioRepository _usuariosRepository;

        public AutenticacaoService(IUsuarioRepository usuarioRepository)
        {
            _passwordHasher = new PasswordHasher<object>();
            _usuariosRepository = usuarioRepository;
        }

        public string CriptografarSenha(string senha)
        {
            return _passwordHasher.HashPassword(new object(), senha);
        }

        private bool VerificarSenha(string senha, string senhaHash)
        {
            if (string.IsNullOrEmpty(senha) || string.IsNullOrEmpty(senhaHash))
                throw new Exception("Necessário informar Senha e SenhaHash.");

            var result = _passwordHasher.VerifyHashedPassword(new object(), senhaHash, senha);
            return result == PasswordVerificationResult.Success;
        }

        public async Task<object> AutenticarAsync(string email, string senha)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
                throw new Exception("Necessário informar Email e Senha.");

            var usuario = await _usuariosRepository.GetByEmailAsync(email);
            if (usuario == null)
                return false;

            if (VerificarSenha(senha, usuario.Senha))
                return TokenService.GenerateToken(usuario);
            else
                throw new Exception("Senha incorreta, verifique!");
        }
    }
}
