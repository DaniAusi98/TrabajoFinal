using Application.ActividadMuseo.Repositories;
using Application.ApplicationMuseo.Repositories;
using Application.Repositories;
using Application.VisitaGrupal.Repositories;

using Domain.Common.Others.Utils;

using Infrastructure.Constants;
using Infrastructure.Repositories.Sql.VisitaGrupal;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Bson.Serialization.Conventions;

using static Domain.Common.Enums.Enums;

namespace Infrastructure.Factories
{
    internal static class DatabaseFactory
    {
        public static void CreateDataBase(this IServiceCollection services, string dbType, IConfiguration configuration)
        {
            switch (dbType.ToEnum<DatabaseType>())
            {
                case DatabaseType.MYSQL:
                    services.AddMySqlRepositories(configuration);
                    break;
                case DatabaseType.MARIADB:
                case DatabaseType.SQLSERVER:
                    services.AddSqlServerRepositories(configuration);
                    break;
                case DatabaseType.MONGODB:
                    services.AddMongoDbRepositories(configuration);
                    break;
                default:
                    throw new NotSupportedException(InfrastructureConstants.DATABASE_TYPE_NOT_SUPPORTED);
            }
        }

        private static IServiceCollection AddSqlServerRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Repositories.Sql.MuseoDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
            }, ServiceLifetime.Scoped);

            //Habilitar para trabajar con Migrations
            var context = services.BuildServiceProvider().GetRequiredService<Repositories.Sql.MuseoDbContext>();
            context.Database.Migrate();

            /* Sql Repositories */
            services.AddTransient<IDummyEntityRepository, Repositories.Sql.DummyEntityRepository>();

            //services.AddTransient<IRepositorioUsuarioVisitante,RepositorioUsuario>();
        


            return services;
        }

        private static IServiceCollection AddMongoDbRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            ConventionRegistry.Register("Camel Case", new ConventionPack { new CamelCaseElementNameConvention() }, _ => true);

            Repositories.Mongo.StoreDbContext db = new(configuration.GetConnectionString("MongoConnection") ?? throw new NullReferenceException());
            services.AddSingleton(typeof(Repositories.Mongo.StoreDbContext), db);

            /* MongoDb Repositories */
            services.AddTransient<IDummyEntityRepository, Repositories.Mongo.DummyEntityRepository>();

            return services;
        }

        private static IServiceCollection AddMySqlRepositories(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<Repositories.Sql.MuseoDbContext>(options =>
            {
                options.UseMySql(
                    configuration.GetConnectionString("MySqlConnection"),
                    ServerVersion.AutoDetect(
                        configuration.GetConnectionString("MySqlConnection")
                    )
                );
            });

            /* Repositories */
            services.AddTransient<IDummyEntityRepository, Repositories.Sql.DummyEntityRepository>();
            // services.AddTransient<IRepositorioUsuarioVisitante, RepositorioUsuario>();
            services.AddTransient<IRepositorioVisitaGuiada, Repositories.Sql.VisitaGrupal.RepositorioVisitaGuiada>();
            services.AddTransient<IRepositorioTematicas, Repositories.Sql.VisitaGrupal.RepositorioTematicasVisita>();
            services.AddTransient<IRepositorioGuia,Repositories.Sql.VisitaGrupal.RepositorioGuia>();
            services.AddTransient<IRepositorioDiaCierreMuseo, Repositories.Sql.DisponibilidadActividades.RepositorioDiaCierreMuseo>();
            services.AddTransient<IRepositorioActividadMuseo,Repositories.Sql.DisponibilidadActividades.RepositorioActividadMuseo>();
            services.AddTransient<IRepositorioVisitaGrupalAutoguiada, RepositorioVisitaGrupalAutoguiada>();
            // 🔥 Migraciones automáticas al levantar la app (infra pura)
            var context = services.BuildServiceProvider()
               .GetRequiredService<Repositories.Sql.MuseoDbContext>();

            context.Database.Migrate();


            return services;
        }

    }
}




/* services.AddTransient<IRepositorioVisitaGuiada, Repositories.Sql.VisitaGrupal.RepositorioVisitaGuiada>();
            services.AddTransient<IRepositorioTematicas, Repositories.Sql.VisitaGrupal.RepositorioTematicasVisita>();
            services.AddTransient<IRepositorioGuia,Repositories.Sql.VisitaGrupal.RepositorioGuia>();
            services.AddTransient<IRepositorioDiaCierreMuseo, Repositories.Sql.DisponibilidadActividades.RepositorioDiaCierreMuseo>();*/
