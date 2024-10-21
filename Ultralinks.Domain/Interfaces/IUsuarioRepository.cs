using Ultralinks.Domain.Models;

namespace Ultralinks.Domain.Interfaces
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<Usuario> GetByEmailAsync(string email);
        Task<Usuario> GetByCpfAsync(string cpf);
        Task<Usuario> GetByCpfOrEmailAsync(string cpf, string email, int id = 0);
        Task<Usuario> GetByIdAsync(int id);
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario> UpdateAsync(Usuario usuario);
        Task DeleteAsync(int id);        
    }
}
