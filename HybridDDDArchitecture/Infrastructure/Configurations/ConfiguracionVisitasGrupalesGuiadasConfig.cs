using Domain.VisitasGrupales.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ConfiguracionVisitasGrupalesGuiadasConfig : IEntityTypeConfiguration<ConfiguracionVisitasGrupalesGuiadas>
    {
        public void Configure(EntityTypeBuilder<ConfiguracionVisitasGrupalesGuiadas> builder)
        {
            builder.ToTable("ConfiguracionVisitasGrupalesGuiadas");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.MinGuiasParaCapacidadCompleta).IsRequired();
            builder.Property(x => x.CapacidadPorGuia).IsRequired();
            builder.Property(x => x.CapacidadMaximaPorTurno).IsRequired();

            // Seed default row if needed (provide explicit non-zero Id to satisfy EF Core validation)
            builder.HasData(new { Id = -1, MinGuiasParaCapacidadCompleta = 2, CapacidadPorGuia = 25, CapacidadMaximaPorTurno = 50 });
        }
    }
}
