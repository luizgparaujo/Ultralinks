using Microsoft.EntityFrameworkCore;
using Ultralinks.Domain.Interfaces;
using Ultralinks.Domain.Models;

namespace Ultralinks.Data.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context)
        { }

        public async Task<Usuario> GetByCpfAsync(string cpf)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Cpf == cpf);
                if (usuario == null)
                    throw new Exception($"Usuário com o CPF '{cpf}' não foi encontrado.");

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar usuário com o CPF '{cpf}'.", ex);
            }
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
                if (usuario == null)
                    throw new Exception($"Usuário com o email '{email}' não foi encontrado.");

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar usuário com o email '{email}'.", ex);
            }
        }

        public async Task<Usuario> GetByCpfOrEmailAsync(string cpf, string email, int id = 0)
        {
            try
            {
                return await _context.Usuarios.FirstOrDefaultAsync(x => (x.Cpf == cpf || x.Email == email) && (id == 0 || x.Id != id));
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar usuário com o email '{email}'.", ex);
            }
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _context.Usuarios.AsNoTracking().Include(u => u.EnderecoCobranca).FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null)
                    throw new Exception($"Entidade com o ID {id} não foi encontrada.");

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar a entidade com o ID {id}.", ex);
            }
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            try
            {
                return await _context.Usuarios.AsNoTracking().Include(u => u.EnderecoCobranca).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar todas as entidades.", ex);
            }
        }

        public async Task<Usuario> UpdateAsync(Usuario usuario)
        {
            try
            {
                var entity = await _context.Usuarios.Include(u => u.EnderecoCobranca).FirstOrDefaultAsync(x => x.Id == usuario.Id);
                if (entity == null)
                    throw new Exception($"Usuário com ID '{usuario.Id}' não encontrado.");

                if (usuario.EnderecoCobranca != null)
                {
                    usuario.EnderecoCobranca.DataAlteracao = DateTime.UtcNow;
                    usuario.EnderecoCobranca.UsuarioId = usuario.Id;
                    _context.Entry(entity.EnderecoCobranca).CurrentValues.SetValues(usuario.EnderecoCobranca);
                }

                usuario.DataAlteracao = DateTime.UtcNow;
                _context.Entry(entity).CurrentValues.SetValues(usuario);

                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar a entidade. {ex.Message}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Usuarios.Include(u => u.EnderecoCobranca).FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null)
                    throw new Exception($"Entidade com id '{id}' não encontrada.");

                _context.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var mensagem = "Erro ao excluir usuário.";

                if (ex.InnerException.Message.Contains("FK_transacao_usuario_usuario_id_destino"))
                    throw new Exception($"{mensagem} Não é permitido excluir usuários que possuem transações.");
                else
                    throw new Exception($"{mensagem}. {ex.Message}");
            }
        }
    }
}
