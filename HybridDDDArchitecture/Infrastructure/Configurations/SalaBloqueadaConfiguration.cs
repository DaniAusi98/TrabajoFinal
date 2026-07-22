
using Domain.ActividadMuseo.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class SalaBloqueadaConfiguration : IEntityTypeConfiguration<SalaBloqueadaMuseo>
    {
        public void Configure(EntityTypeBuilder<SalaBloqueadaMuseo> builder)
        {
            builder.ToTable("salabloqueada");
            builder.Property(a => a.FechaDesde)
               .IsRequired();
            builder.Property(a => a.FechaHasta)
                .IsRequired();
            builder.Property(a => a.Motivo)
                .HasConversion<string>()
                .IsRequired();
            builder.Property(a => a.Observaciones)
                .HasMaxLength(500)
                .IsRequired(false);
             builder.HasMany(x => x.SalasBloqueadas)
                 .WithMany()
                 .UsingEntity(j => j.ToTable("SalaBloqueada_Sala"));
            

        }
    }
}
