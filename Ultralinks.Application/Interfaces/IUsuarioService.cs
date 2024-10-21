using Ultralinks.Application.ViewModels;
using Ultralinks.Domain.DTOs;

namespace Ultralinks.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDTO> CreateAsync(CreateUsuarioViewModel usuarioVM);
        Task<UsuarioDTO> GetByIdAsync(int id);
        Task<UsuarioDTO> UpdateAsync(UpdateUsuarioViewModel usuarioVM);
        Task<List<UsuarioDTO>> GetAllAsync();
        Task DeleteAsync(int id);
        Task PutSenhaAsync(int idUsuario, string senha);
    }
}
