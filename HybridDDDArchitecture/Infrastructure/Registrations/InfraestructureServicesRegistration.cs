using Application.ApplicationMuseo.ApplicationServices;
using Application.Common.ApplicationServices;
using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;

using Core.Application.Adapters.Http;
using Core.Infraestructure;
using Core.Infraestructure.Adapters.Http;

using Infrastructure.Adapters;
using Infrastructure.Adapters.EmailSender.ResendEmailService;
using Infrastructure.Adapters.EmailSender.ResendEmailService.User;
using Infrastructure.Constants;
using Infrastructure.Factories;
using Infrastructure.Identity;

using Microsoft.AspNetCore.Builder;


using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;

using Resend;

namespace Infrastructure.Registrations
{
    /// <summary>
    /// Aqui se deben registrar todas las dependencias de la capa de infraestructura
    /// </summary>
    public static class InfraestructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            /* Database Context */
            services.AddRepositories(configuration);

            /* EventBus */
            services.AddEventBus(configuration);

            services.Configure<ResendClientOptions>(options =>
            {
                options.ApiToken = configuration["Resend:ApiKey"]!;
            });

            services.AddHttpClient<ResendClient>();
            services.AddTransient<IResend, ResendClient>();
            services.AddScoped<IConfirmUserUrl, ConfirmUserUrl>();

            services.AddScoped<IEmailService, ResendEmailService>();

            /* Adapters */
            services.AddSingleton<IExternalApiClient, ExternalApiHttpAdapter>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<JwtTokenService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IClock, ArgentinaClock>();
            services.AddHttpClient<IConsultarProvinciasArgetina, ObtenerProvinciasArgentinaGeoRef>(client =>
            {
                // Aquí es donde va la base del Curl. Tiene que terminar siempre con una barra diagonal '/'
                client.BaseAddress = new Uri("https://apis.datos.gob.ar/");
            });
            services.AddHttpClient<IConsultaDepartamentos, ObtenerDepartamentosArg>(client =>
            {
                // Aquí es donde va la base del Curl. Tiene que terminar siempre con una barra diagonal '/'
                client.BaseAddress = new Uri("https://apis.datos.gob.ar/");
            });
            services.AddHttpClient<IConsultarLocalidades, ObtenerLocalidadesArg>(client =>
            {
                // Aquí es donde va la base del Curl. Tiene que terminar siempre con una barra diagonal '/'
                client.BaseAddress = new Uri("https://apis.datos.gob.ar/");
            });

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            string dbType = configuration["Configurations:UseDatabase"] ?? throw new NullReferenceException(InfrastructureConstants.DATABASE_TYPE_NOT_CONFIGURED);

            services.CreateDataBase(dbType, configuration);

            return services;
        }
        // 🔽 AGREGAR ESTO - Método para seedear roles
        public static void SeedIdentityRoles(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                IdentityDataInitializer.SeedRolesAsync(roleManager).GetAwaiter().GetResult();
            }
        }
        public static void SeedCalendarioMuseo(this IApplicationBuilder app)
        {
            Infrastructure.Data.Seeders.CalendarioMuseoSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        // 🔼
        /// <summary>
        /// Seedea la configuración inicial de visitas grupales guiadas al iniciar la aplicación
        /// </summary>
        public static void SeedConfiguracionVisitas(this IApplicationBuilder app)
        {
            Infrastructure.Data.Seeders.ConfiguracionVisitasSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedConfiguracionHorarioAutoguiadas(this IApplicationBuilder app)
        {
            Infrastructure.Data.Seeders.ConfiguracionHorarioAutoguiadasSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedProvinciasArgentina(this IApplicationBuilder app)
        {
            Data.Seeders.Ubicacion.ProvinciaSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedDepartamentosArgentina(this IApplicationBuilder app)
        {
            Data.Seeders.Ubicacion.SeederDepartamentos
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();

        }
        public static void SeedLocalidadesArgentina(this IApplicationBuilder app)
        {
            Data.Seeders.Ubicacion.SeederLocalidades
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedSalasMuseo(this IApplicationBuilder app)
        {
            Data.Seeders.SalaSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedTematicasVisitas(this IApplicationBuilder app)
        {
            Data.Seeders.TematicaVisitaSeeder
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
        public static void SeedGuiasMuseo(this IApplicationBuilder app)
        {
            Data.Seeders.Ubicacion.SeederGuias
                .SeedAsync(app.ApplicationServices)
                .GetAwaiter()
                .GetResult();
        }
    }
}