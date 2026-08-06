using Application.ActividadMuseo.Repositories;
using Application.VisitaGrupal.Repositories;
using Domain.VisitasGrupales.Entities;
using Domain.VisitasGrupales.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Globalization;


namespace Infrastructure.Data.Seeders
{
    public static class ConfiguracionHorarioAutoguiadasSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var repositorio = scope.ServiceProvider.GetRequiredService<IRepositorioConfiguracionHorarioAutoguiada>();
            var repositorioCalendario = scope.ServiceProvider.GetRequiredService<IRepositorioCalendarioMuseo>();
            var logger = scope.ServiceProvider.GetService<ILogger<IRepositorioConfiguracionHorarioAutoguiada>>();
            try
            {
                logger?.LogInformation(" Verificando configuración de visitas grupales autoguiadas...");

                // Verificar si ya existe configuración
                var existente = await repositorio.ObtenerConfiguracionActivaAsync();

                if (existente != null)
                {
                    logger?.LogInformation(" Configuración de visitas grupales autoguiadas ya existe (ID: {Id}). Seed omitido.", existente.Id);
                    return;
                }

                // Obtener el calendario del museo
                var calendario = await repositorioCalendario.ObtenerCalendarioActivoAsync();

                if (calendario == null)
                {
                    logger?.LogWarning("No se encontró el calendario del museo.");
                    return;
                }

                logger?.LogInformation(
                    "Horario calendario: {Inicio} - {Fin}",
                    calendario.HorarioApertura.HoraInicio,
                    calendario.HorarioApertura.HoraFin);

                var configuracionInicial =
                    ConfiguracionVisitasAutoguiadasFactory.CrearConfiguracionPorDefecto(calendario);

                await repositorio.AddAsync(configuracionInicial);

                logger?.LogInformation("  Configuración de visitas grupales autoguiadas creada correctamente:");
                logger?.LogInformation("   - Capacidad maxima por grupo: {CapacidadMaximaPorGrupo}", configuracionInicial.CapacidadMaximaPorGrupo);
                logger?.LogInformation("   - Días disponibles: {DiasCount} de {DiasMuseo}",
                    configuracionInicial.DiasDisponibles.Dias.Count,
                    calendario.DiasApertura.Dias.Count);

                if (calendario == null)
                {
                    logger?.LogWarning(" No se encontró el calendario del museo. No se puede crear configuración de visitas guiadas.");
                    return;
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, " Error al sembrar la configuración de visitas grupales autoguiadas.");
            }


        }
    }
}
