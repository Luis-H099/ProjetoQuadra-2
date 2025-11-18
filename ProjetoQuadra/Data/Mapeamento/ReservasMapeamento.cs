using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjetoQuadra.Data.Mapeamento
{
        public class ReservasMapeamento : IEntityTypeConfiguration<Models.Reservas>
        {
            public void Configure(EntityTypeBuilder<Models.Reservas> entity)
            {
                entity.ToTable("reservas");
                entity.HasKey(r => r.id_reserva);

                entity.Property(r => r.id_usuario).HasColumnType("int");
                entity.Property(r => r.id_quadra).HasColumnType("int");
                entity.Property(r => r.data_reserva).HasColumnType("date");
                entity.Property(r => r.hora_inicio).HasColumnType("time");
                entity.Property(r => r.hora_fim).HasColumnType("time");
                entity.Property(r => r.status).HasColumnType("varchar(20)");
                entity.Property(r => r.valor_total).HasColumnType("decimal(10,2)");
                entity.Property(r => r.data_criacao).HasColumnType("datetime");
            }
        }
}

