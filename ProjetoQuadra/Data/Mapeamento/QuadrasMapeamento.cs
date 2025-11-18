using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoQuadra.Models;

namespace ProjetoQuadra.Data.Mapeamento
{
    public class QuadrasMapeamento : IEntityTypeConfiguration<Quadras>
    {
        public void Configure(EntityTypeBuilder<Quadras> builder)
        {
            builder.ToTable("Quadras");
            builder.HasKey(q => q.id_quadra);
            builder.Property(q => q.nome).HasColumnType("VARCHAR(50)");
            builder.Property(q => q.tipo).HasColumnType("VARCHAR(20)");
        }
    }
}
