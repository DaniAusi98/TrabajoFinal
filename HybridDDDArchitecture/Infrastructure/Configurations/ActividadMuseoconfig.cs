using Domain.ActividadMuseo.Entities;
using Domain.RecursoMuseo.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ActividadMuseoConfiguration : IEntityTypeConfiguration<Actividad>
    {
        public void Configure(EntityTypeBuilder<Actividad> builder)
        {
            builder.ToTable("ActividadesMuseo");

            builder.HasKey(x => x.Id);

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

            builder.OwnsMany(x => x.TimeSlots, ts =>
            {
                ts.ToTable("ActividadTimeSlots");

                ts.WithOwner()
                  .HasForeignKey("ActividadMuseoId");

                ts.Property<int>("Id");

                ts.HasKey("Id");

                ts.Property(x => x.Inicio)
                  .HasColumnType("datetime");

                ts.Property(x => x.Fin)
                  .HasColumnType("datetime");
            });

         

            builder.HasMany(x => x.Salas)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "ActividadSala",
                    j => j
                        .HasOne<Sala>()
                        .WithMany()
                        .HasForeignKey("SalaId"),
                    j => j
                        .HasOne<Actividad>()
                        .WithMany()
                        .HasForeignKey("ActividadMuseoId"),
                    j =>
                    {
                        j.ToTable("ActividadSalas");
                        j.HasKey("ActividadMuseoId", "SalaId");
                    });


            builder.HasMany(x => x.Recursos)
                   .WithOne(x => x.Actividad)
                   .HasForeignKey(x => x.ActividadId)
                   .OnDelete(DeleteBehavior.Cascade); // Si se borra la actividad, se desasignan sus recursos

     
        }
    }
}
