using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities.VisitasGrupalesMuseo;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TematicasConfig : IEntityTypeConfiguration<TematicaVisita>
    {
       
        public void Configure(EntityTypeBuilder<TematicaVisita> builder)
        {
            // 1. Nombre de la tabla y clave primaria (heredada de DomainEntity)
            builder.ToTable("TematicasVisita");
            builder.HasKey(x => x.Id);
            // 2. Configuración de textos obligatorios
            builder.Property(x => x.Nombre)
                .HasMaxLength(100)
                .IsRequired();
             builder.Property(x => x.Descripcion)
                .HasMaxLength(500)
                .IsRequired();
             builder.Property(x => x.Disponible)
                .IsRequired();
        }
    }
}
