using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoQuadra.Models;

namespace ProjetoQuadra.Data.Mapeamento
{
    public class UsuariosMapeamento : IEntityTypeConfiguration<Usuarios>
    {
        public void Configure(EntityTypeBuilder<Usuarios> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(u => u.id_usuario);

            builder.Property(u => u.nome).HasColumnType("VARCHAR(100)");
            builder.Property(u => u.cpf).HasColumnType("VARCHAR(11)");
            builder.Property(u => u.senha).HasColumnType("VARCHAR(50)");
            builder.Property(u => u.data_cadastro).HasColumnType("DATETIME");
        }
    }
}
