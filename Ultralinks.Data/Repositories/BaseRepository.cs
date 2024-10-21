using Microsoft.EntityFrameworkCore;
using Ultralinks.Domain.Interfaces;
using Ultralinks.Domain.Models;

namespace Ultralinks.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseDomain
    {
        protected readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T> CreateAsync(T entity)
        {
            try
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar a entidade. {ex.Message}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity == null)
                    throw new Exception($"Entidade com id '{id}' não encontrada.");

                _context.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir a entidade. {ex.Message}");
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null)
                    throw new Exception($"Entidade com o ID {id} não foi encontrada.");

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar a entidade. {ex.Message}");
            }
        }

        public async Task<List<T>> GetByIdsAsync(IEnumerable<int> ids)
        {
            try
            {
                // Verifica se a lista de IDs não está vazia
                if (ids == null || !ids.Any())
                    throw new ArgumentException("A lista de IDs não pode ser vazia.");

                // Busca as entidades que possuem IDs correspondentes
                var entities = await _context.Set<T>()
                    .Where(x => ids.Contains(x.Id))
                    .ToListAsync();

                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar as entidades. {ex.Message}");
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar todas as entidades. {ex.Message}");
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                var exists = await _context.Set<T>().FindAsync(entity.Id);
                if (exists == null)
                    throw new Exception($"Entidade com ID '{entity.Id}' não encontrada.");

                _context.Entry(exists).CurrentValues.SetValues(entity);

                exists.DataAlteracao = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return exists;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar a entidade. {ex.Message}");
            }
        }
    }
}
