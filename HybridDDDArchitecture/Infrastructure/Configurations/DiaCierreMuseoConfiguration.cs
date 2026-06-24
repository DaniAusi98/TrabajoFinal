using Domain.Entities.DisponibilidadMuseo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class DiaCierreMuseoConfiguration : IEntityTypeConfiguration<DiaCierreMuseo>
    {
        public void Configure(EntityTypeBuilder<DiaCierreMuseo> builder)
        {
            builder.ToTable("diascierremuseo");

            builder.Property(a => a.Fecha)
               .IsRequired();

            builder.Property(a => a.FechaHasta)
                .IsRequired();
           

            builder.Property(a => a.Motivo)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(a => a.Observaciones)
                .HasMaxLength(500)
                .IsRequired(false);



        }
    }
}
