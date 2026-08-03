using System.Reflection;

using Application.ApplicationMuseo.ApplicationServices;
using Application.ApplicationMuseo.Integrations.Events;
using Application.ApplicationMuseo.Integrations.Handlers.Publishers;
using Application.ApplicationMuseo.Integrations.Handlers.Subscribers;
using Application.Behaviors;
using Application.Usuario.ApplicationServices.ApplicationServiceInterfaces;
using Application.Usuario.UseCases.Commands.UpdateUsuario;
using Core.Application;
using Domain.ActividadMuseo.Entities;
using FluentValidation;

using MediatR;
using Scrutor;

using Domain.VisitasGrupales.Options;
using Domain.VisitasGrupales.DomainServices;
using Microsoft.Extensions.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Availability;

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

            // Bind TurnosVisitas options from configuration and register service
            services.Configure<TurnosVisitasOptions>(configuration.GetSection("TurnosVisitas"));
            // Add options validator
            services.AddSingleton<Microsoft.Extensions.Options.IValidateOptions<TurnosVisitasOptions>, Domain.VisitasGrupales.Options.TurnosVisitasOptionsValidator>();
            // Register application services for managing persisted configuracion visitas grupales guiadas
            services.AddScoped<Application.VisitaGrupal.ApplicationServices.IConfiguracionVisitasGrupalesGuiadasService, Application.VisitaGrupal.ApplicationServices.ConfiguracionVisitasGrupalesGuiadasService>();
            services.AddScoped<Domain.VisitasGrupales.Options.IConfiguracionVisitasOptionsProvider, Application.VisitaGrupal.ApplicationServices.ConfiguracionVisitasOptionsProvider>();

            // Register availability service by interface (domain service will obtain options via provider)
            services.AddScoped<Domain.VisitasGrupales.DomainServices.IServicioDisponibilidadTurnosVisitasGuiadas, ServicioDisponibilidadTurnosVisitasGuiadas>();

            // Register availability engine, rule factory and providers (moved from Application.Availability.ServiceCollectionExtensions)
            services.AddScoped<Application.Availability.AvailabilityEngine>();
            services.AddScoped<Application.Availability.IRuleFactory, Application.Availability.CompositeRuleFactory>();

            // Register providers - these will be created by DI and can create rule instances
            services.Scan(scan => scan
                .FromAssemblyOf<AvailabilityEngine>()
                .AddClasses(classes => classes.InNamespaces("Application.Availability.Providers"))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            // Register rules that do not require extra dependencies
            services.AddScoped<Application.Availability.IAvailabilityRule, Application.Availability.Rules.NoConcurrentGuidedIfGroupRule>();

            // Register guided availability producer service
            services.AddScoped<Application.Availability.Producers.GuidedAvailabilityProducerService>();
            // Register group availability producer service
           // services.AddScoped<Application.Availability.Producers.GroupAvailabilityProducerService>();
            // Register in-memory recurrence repo for testing (optional)
            services.AddSingleton<Application.Availability.Recurrence.IRecurrenceRuleRepository, Application.Availability.Recurrence.InMemoryRecurrenceRuleRepository>();

            services.AddValidatorsFromAssemblyContaining<CrearVisitaGuiadaCommandValidator>();

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));


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
