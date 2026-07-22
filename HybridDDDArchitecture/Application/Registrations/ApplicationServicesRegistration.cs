using System.Reflection;

using Application.ApplicationMuseo.ApplicationServices;
using Application.ApplicationMuseo.Integrations.Events;
using Application.ApplicationMuseo.Integrations.Handlers.Publishers;
using Application.ApplicationMuseo.Integrations.Handlers.Subscribers;
using Application.Usuario.ApplicationServices;
using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.UseCases.Commands.UpdateUsuario;

using Core.Application;

using Domain.ActividadMuseo.Entities;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Registrations
{
    /// <summary>
    /// Aqui se deben registrar todas las dependencias de la capa de aplicacion
    /// </summary>
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            /* Automapper */
            services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = configuration["LuckyPennySoftware:LicenseKey"];
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            services.AddMediatR(cfg =>
            {
                cfg.LicenseKey = configuration["LuckyPennySoftware:LicenseKey"];
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddScoped<ICommandQueryBus, MediatrCommandQueryBus>();
            services.AddScoped<IConfirmEmailService, ConfirmEmailServiceResponse>();
            /* EventBus */
            services.AddPublishers();
            services.AddSubscribers();

            /* MediatR*/
            

            /* Application Services */
            services.AddScoped<IDummyEntityApplicationService, DummyEntityApplicationService>();
            //services.AddScoped<IUsuarioApplicationService,UsuarioVisitanteApplicationService>();

            services.AddScoped<ICalendarioMuseo, CalendarioMuseo>();



            services.AddValidatorsFromAssembly(typeof(ApplicationServicesRegistration).Assembly);

           // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


            return services;
        }

        private static IServiceCollection AddPublishers(this IServiceCollection services)
        {
            //Aqui se registran los handlers que publican en el bus de eventos
            services.AddTransient<IIntegrationEventHandler<DummyEntityCreatedIntegrationEvent>, DummyEntityCreatedIntegrationEventHandlerPub>();
            return services;
        }

        private static IServiceCollection AddSubscribers(this IServiceCollection services)
        {
            //Aqui se registran los handlers que se suscriben al bus de eventos
            services.AddTransient<DummyEntityCreatedIntegrationEventHandlerSub>();
            return services;
        }
    }
}
