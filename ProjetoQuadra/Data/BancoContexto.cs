
using Microsoft.EntityFrameworkCore;
using ProjetoQuadra.Data.Mapeamento;
using ProjetoQuadra.Models;


namespace ProjetoQuadra.Data
{
    public class BancoContexto : DbContext
    {
        public BancoContexto(DbContextOptions<BancoContexto> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuariosMapeamento());
            // Definindo valor padrão para a coluna data_cadastro
            modelBuilder.Entity<Usuarios>()
                      .Property(u => u.data_cadastro)
                      .HasDefaultValueSql("GETDATE()");

            modelBuilder.ApplyConfiguration(new QuadrasMapeamento());
            modelBuilder.ApplyConfiguration(new CardapioMapeamento());
            modelBuilder.ApplyConfiguration(new ReservasMapeamento());
            modelBuilder.Entity<Reservas>()
                        .Property(r => r.data_criacao)
                        .HasDefaultValueSql("GETDATE()");
        }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Quadras> Quadras { get; set; }
        public DbSet<Cardapio> Cardapio { get; set; }
        public DbSet<Reservas> Reservas { get; set; }
    }
}
