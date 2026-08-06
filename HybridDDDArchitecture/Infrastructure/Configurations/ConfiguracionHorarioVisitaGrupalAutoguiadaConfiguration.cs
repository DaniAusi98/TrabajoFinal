using Domain.VisitasGrupales.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Infrastructure.Configurations
{
    internal sealed class ConfiguracionHorarioVisitaGrupalAutoguiadaConfiguration : IEntityTypeConfiguration<ConfiguracionHorarioAutoguiada>
    {
        public void Configure(EntityTypeBuilder<ConfiguracionHorarioAutoguiada> builder)
        {
            builder.ToTable("ConfiguracionVisitaAutoguiada");

            builder.HasKey(c => c.Id);


            builder.Property(c => c.CapacidadMaximaPorGrupo)
                .IsRequired();


            builder.OwnsOne(x => x.HorarioDisponibleVisitaAutoguiadas, horario =>
            {
                horario.Property(h => h.HoraInicio)
                    .HasColumnName("HoraInicio")
                    .IsRequired();

                horario.Property(h => h.HoraFin)
                    .HasColumnName("HoraFin")
                    .IsRequired();
            });
            builder.OwnsOne(c => c.DiasDisponibles, diasNav =>
            {
                diasNav.Property(d => d.Dias)
                    .HasColumnName("DiasDisponibles")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<DayOfWeek>>(v, (JsonSerializerOptions)null) ?? new List<DayOfWeek>()
                    )
                    .IsRequired();
            });
        }
    }
}
