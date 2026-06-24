using Domain.Entities.VisitasGrupalesMuseo;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class VisitaGrupalAutoguiadaConfiguration : IEntityTypeConfiguration<VisitaGrupalAutoguiada>
    {
        public void Configure(EntityTypeBuilder<VisitaGrupalAutoguiada> builder)
        {
            builder.ToTable("VisitasGrupalesAutoguiadas");

            builder.Property(x => x.UsuarioVisitanteId)
                .IsRequired();

            builder.Property(x => x.Institucion)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.ProvinciaInstitucion)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.DepartamentoInstitucion)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.CiudadInstitucion)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.DiversidadFuncional)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.Observaciones)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(x => x.EstadoConfirmacion)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.OwnsOne(x => x.EmailInstitucion, email =>
            {
                email.Property(e => e.Valor)
                    .HasColumnName("EmailInstitucion")
                    .HasMaxLength(200)
                    .IsRequired();
            });
        }
    }
}