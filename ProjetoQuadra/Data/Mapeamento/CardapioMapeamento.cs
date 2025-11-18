using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoQuadra.Models;

namespace ProjetoQuadra.Data.Mapeamento
{
    public class CardapioMapeamento : IEntityTypeConfiguration<Cardapio>
    {
        public void Configure(EntityTypeBuilder<Cardapio> builder)
        {
            builder.ToTable("Cardapio");

            builder.HasKey(c => c.id_item);

            builder.Property(c => c.nome_item).HasColumnType("VARCHAR(100)");
            builder.Property(c => c.preco).HasColumnType("DECIMAL(10,2)");
            builder.Property(c => c.categoria).HasColumnType("VARCHAR(50)");
            builder.Property(c => c.disponibilidade).HasColumnType("BIT");
        }
    }
}
