using Application.ApplicationMuseo.ApplicationServices;
using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Core.Application.Adapters.Http;
using Core.Infraestructure;
using Core.Infraestructure.Adapters.Http;
using Infrastructure.Adapters;
using Infrastructure.Constants;
using Infrastructure.Factories;
using Infrastructure.Identity;

using Microsoft.AspNetCore.Builder;  


using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
                    
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

            /* Adapters */
            services.AddSingleton<IExternalApiClient, ExternalApiHttpAdapter>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddSingleton<IUsuarioRegistradoEmailSender, UsuarioRegistradoEmailSender>();
            services.AddScoped<JwtTokenService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IClock,ArgentinaClock>();

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
        // 🔼
    }
}
