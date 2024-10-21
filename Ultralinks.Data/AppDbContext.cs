using Microsoft.EntityFrameworkCore;
using Ultralinks.Domain.Models;

namespace Ultralinks.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.EnderecoCobranca)
                .WithOne(e => e.Usuario)
                .HasForeignKey<Endereco>(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transacao>()
            .HasOne(t => t.UsuarioOrigem)
            .WithMany(u => u.TransacoesOrigem)
            .HasForeignKey(t => t.UsuarioIdOrigem)
            .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Transacao>()
                .HasOne(t => t.UsuarioDestino)
                .WithMany(u => u.TransacoesDestino)
                .HasForeignKey(t => t.UsuarioIdDestino)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
