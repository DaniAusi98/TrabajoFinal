using Infrastructure.Configurations;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.VisitasGrupales.Entities;
using Domain.ActividadMuseo.Entities;
using Domain.RecursoMuseo.Entities;
using Domain.Common.Entities;
using Domain.RecursoMuseo.Entities.Guia;

namespace Infrastructure.Repositories.Sql
{
    /// <summary>
    /// Contexto de almacenamiento en base de datos. Aca se definen los nombres de 
    /// las tablas, y los mapeos entre los objetos
    /// </summary>
    public class MuseoDbContext
    : IdentityDbContext<UsuarioSistema, IdentityRole, string>
    {
        public MuseoDbContext(DbContextOptions<MuseoDbContext> options)
            : base(options)
        {
        }
       // public DbSet<DummyEntity> DummyEntity { get; set; }


        public DbSet<VisitaGrupalGuiada> VisitaGuiada { get; set; }

       public DbSet<VisitaGrupalAutoguiada> VisitaGrupalAutoguiada { get; set; }

        public DbSet<ActividadMuseo> ActividadAgendaMuseo { get; set; }
        public DbSet<RecursoAsignado> RecursoAsignado { get; set; }
        public DbSet<Recurso> RecursoMuseo { get; set; }
        public DbSet<Sala> SalaMuseo { get; set; }
        public DbSet<Guia> GuiaMuseo { get; set; }
        public DbSet<AusenciaGuia> AusenciasGuia { get; set; }
        public DbSet<DiaCierreMuseo> DiaCierreMuseo { get; set; }
        public DbSet<HorarioGuia> HorarioGuia { get; set; }
        public DbSet<TematicaVisita> TematicaVisita { get; set; }
        public DbSet<Domain.VisitasGrupales.Entities.ConfiguracionVisitasGrupalesGuiadas> ConfiguracionVisitasGrupalesGuiadas { get; set; }
        public DbSet<Domain.Common.Entities.Provincia> Provincias { get; set; }
        public DbSet<Domain.Common.Entities.Departamento> Departamentos { get; set; }
        public DbSet<Domain.Common.Entities.Localidad> Localidades { get; set; }



        /*public DbSet<PersonalInterno> PersonalInterno { get; set; }

        public DbSet<Area> Area { get; set; }

        public DbSet<AreaPuesto> AreaPuesto { get; set; }

        public DbSet<Puesto> Puesto { get; set; }*/









        protected MuseoDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DummyEntity>().ToTable("DummyEntity");
            modelBuilder.ApplyConfiguration(new VisitaGrupalGuiadaConfiguration());
            modelBuilder.ApplyConfiguration(new VisitaGrupalAutoguiadaConfiguration());
            modelBuilder.ApplyConfiguration(new ActividadMuseoConfiguration());
            modelBuilder.ApplyConfiguration(new RecursoConfiguration());
            modelBuilder.ApplyConfiguration(new SalaConfiguration());
            modelBuilder.ApplyConfiguration(new RecursoAsignadoConfiguration());
            modelBuilder.ApplyConfiguration(new GuiaConfiguration());
            modelBuilder.ApplyConfiguration(new HorarioGuiaConfiguration());
            modelBuilder.ApplyConfiguration(new AusenciaGuiaConfiguration());
            modelBuilder.ApplyConfiguration(new DiaCierreMuseoConfiguration());
            modelBuilder.ApplyConfiguration(new BloqueoSalaConfiguration());
            modelBuilder.ApplyConfiguration(new TematicaVisitaConfiguration());
            modelBuilder.ApplyConfiguration(new ProvinciaConfiguration());
            modelBuilder.ApplyConfiguration(new DepartamentoConfiguration());
            modelBuilder.ApplyConfiguration(new LocalidadConfiguration());
            modelBuilder.ApplyConfiguration(new ConfiguracionVisitasGrupalesGuiadasConfig());

        }
    }
}
