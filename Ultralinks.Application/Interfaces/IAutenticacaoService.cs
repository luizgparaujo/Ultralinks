namespace Ultralinks.Application.Interfaces
{
    public interface IAutenticacaoService
    {
        string CriptografarSenha(string senha);
        Task<object> AutenticarAsync(string email, string senha);
    }
}
