using Domain.VisitasGrupales.Entities.Guia;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Configurations
{
    public class HorarioGuiaConfiguration
        : IEntityTypeConfiguration<HorarioGuia>
    {
        public void Configure(EntityTypeBuilder<HorarioGuia> builder)
        {
            builder.ToTable("HorariosGuia");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.GuiaId)
                .IsRequired();

            builder.Property(h => h.HoraInicio)
                .IsRequired();

            builder.Property(h => h.HoraFin)
                .IsRequired();

            var jsonOptions = new JsonSerializerOptions();

            jsonOptions.Converters.Add(
                new JsonStringEnumConverter());

            var converter = new ValueConverter<List<DayOfWeek>, string>(
                v => JsonSerializer.Serialize(v, jsonOptions),

                v => JsonSerializer.Deserialize<List<DayOfWeek>>(
                    v,
                    jsonOptions)!);

            builder.Property(h => h.DiasLaborales)
                .HasConversion(converter)
                .HasColumnType("longtext");
        }
    }
}
