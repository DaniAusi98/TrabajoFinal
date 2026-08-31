using Domain.ActividadMuseo.Entities;
using Domain.RecursoMuseo.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ActividadMuseoConfiguration : IEntityTypeConfiguration<ActividadMuseo>
    {
        public void Configure(EntityTypeBuilder<ActividadMuseo> builder)
        {
            builder.ToTable("ActividadesMuseo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CategoriaActividad)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.TipoActividad)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Estado)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CantidadPersonas)
                .IsRequired(false);


            // =========================
            // HORARIO
            // =========================

            builder.OwnsOne(x => x.Horario, h =>
            {
                h.ToTable("ActividadTimeSlots");

                h.WithOwner()
                    .HasForeignKey("ActividadMuseoId");

                h.Property<int>("Id");

                h.HasKey("Id");

                h.Property(x => x.Inicio)
                    .HasColumnType("datetime");

                h.Property(x => x.Fin)
                    .HasColumnType("datetime");
            });


            // =========================
            // RECURRENCIA
            // =========================

            builder.OwnsOne(x => x.Recurrence, r =>
            {
                r.ToTable("ActividadRecurrencias");

                r.WithOwner()
                    .HasForeignKey("ActividadMuseoId");

                r.Property(x => x.StartDate)
                    .HasColumnType("date")
                    .IsRequired();

                r.Property(x => x.EndDate)
                    .HasColumnType("date")
                    .IsRequired(false);

                r.Property(x => x.Frequency)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                r.Property(x => x.Interval)
                    .IsRequired();

                r.Property(x => x.ByDays)
                    .HasConversion(
                        v => v == null
                            ? null
                            : System.Text.Json.JsonSerializer.Serialize(
                                v,
                                (System.Text.Json.JsonSerializerOptions?)null),

                        v => string.IsNullOrEmpty(v)
                            ? null
                            : System.Text.Json.JsonSerializer.Deserialize<DayOfWeek[]>(
                                v,
                                (System.Text.Json.JsonSerializerOptions?)null)
                    )
                    .HasColumnType("json")
                    .IsRequired(false);

                r.Property(x => x.MonthDay)
                    .IsRequired(false);

                r.Property(x => x.WeekOfMonth)
                    .IsRequired(false);
            });


            // =========================
            // EXCEPCIONES
            // =========================

            builder.HasMany(x => x.Exceptions)
                .WithOne()
                .HasForeignKey(x => x.ActividadMuseoId)
                .OnDelete(DeleteBehavior.Cascade);


            // =========================
            // SALAS
            // =========================

            builder.HasMany(x => x.Salas)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "ActividadSala",
                    j => j
                        .HasOne<Sala>()
                        .WithMany()
                        .HasForeignKey("SalaId"),
                    j => j
                        .HasOne<ActividadMuseo>()
                        .WithMany()
                        .HasForeignKey("ActividadMuseoId"),
                    j =>
                    {
                        j.ToTable("ActividadSalas");
                        j.HasKey("ActividadMuseoId", "SalaId");
                    });


            // =========================
            // RECURSOS
            // =========================

            builder.HasMany(x => x.Recursos)
                .WithOne(x => x.Actividad)
                .HasForeignKey(x => x.ActividadId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}