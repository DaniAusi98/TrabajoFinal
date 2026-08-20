using Application.VisitaGrupal.Repositories;
using Domain.VisitasGrupales.DomainServices;
using Domain.VisitasGrupales.Entities;
using Domain.Common.ValueObjets;
using static Domain.VisitasGrupales.Enums.Enums;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Microsoft.Extensions.Logging;

namespace Application.Availability.Producers
{
    // Orquestador que obtiene los turnos del servicio de guiadas,
    // construye candidatas mínimas y las valida una por una con el AvailabilityEngine
    public class GuidedAvailabilityProducerService
    {
        private readonly IServicioDisponibilidadTurnosVisitasGuiadas _servicioGuiadas;
        private readonly IRepositorioGuia _repositorioGuia;
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada;
        private readonly ActividadMuseo.Repositories.IRepositorioActividadMuseo _repositorioActividadMuseo;
        private readonly IRepositorioConfiguracionVisitasGrupalesGuiadas _repositorioConfiguracion;
        private readonly ActividadMuseo.Repositories.IRepositorioCalendarioMuseo _repositorioCalendario;
        private readonly AvailabilityEngine _engine;
        private readonly ILogger<GuidedAvailabilityProducerService> _logger;

        public GuidedAvailabilityProducerService(
            IServicioDisponibilidadTurnosVisitasGuiadas servicioGuiadas,
            IRepositorioGuia repositorioGuia,
            IRepositorioVisitaGuiada repositorioVisitaGuiada,
            Repositories.IRepositorioDiaCierreMuseo repositorioDiasCierre,
            ActividadMuseo.Repositories.IRepositorioActividadMuseo repositorioActividadMuseo,
            IRepositorioConfiguracionVisitasGrupalesGuiadas repositorioConfiguracion,
            ActividadMuseo.Repositories.IRepositorioCalendarioMuseo repositorioCalendario,
            AvailabilityEngine engine,
            IServiceProvider serviceProvider,
            ILogger<GuidedAvailabilityProducerService> logger)
        {
            _servicioGuiadas = servicioGuiadas;
            _repositorioGuia = repositorioGuia;
            _repositorioVisitaGuiada = repositorioVisitaGuiada;
            _repositorioActividadMuseo = repositorioActividadMuseo;
            _repositorioConfiguracion = repositorioConfiguracion;
            _repositorioCalendario = repositorioCalendario;
            _engine = engine;
            _logger = logger;
        }

        // Devuelve la lista de turnos aprobados por las reglas de disponibilidad
        public async Task<List<TurnoDisponible>> GetAvailableTurnsAsync(
            DateTime desde,
            DateTime hasta)
        {
            _logger.LogInformation(
                "[GuidedAvailabilityProducer] ===== INICIO =====");

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Rango solicitado: Desde {Desde} - Hasta {Hasta}",
                desde,
                hasta);

            // ============================================================
            // 1) PREFETCH DATOS
            // ============================================================

            var guias = await _repositorioGuia.ObtenerGuiasConDisponibilidadAsync();

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Guías obtenidos: {Cantidad}",
                guias.Count);

            var visitasGuiadasExistentes =
                await _repositorioVisitaGuiada.GetAllGroupVisitAsync(desde, hasta);

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Visitas guiadas existentes obtenidas: {Cantidad}",
                visitasGuiadasExistentes.Count);

            var configuracion =
                await _repositorioConfiguracion.ObtenerConfiguracionActivaAsync();

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Configuración encontrada: {Existe}",
                configuracion is not null);

            var calendario =
                await _repositorioCalendario.ObtenerCalendarioActivoAsync();

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Calendario encontrado: {Existe}",
                calendario is not null);

            if (configuracion == null)
            {
                _logger.LogError(
                    "[GuidedAvailabilityProducer] No existe configuración activa");

                throw new InvalidOperationException(
                    "No hay configuración activa para visitas guiadas");
            }

            if (calendario == null)
            {
                _logger.LogError(
                    "[GuidedAvailabilityProducer] No existe calendario activo");

                throw new InvalidOperationException(
                    "No hay calendario activo del museo");
            }

            // ============================================================
            // 2) SERVICIO DE DISPONIBILIDAD DE VISITAS GUIADAS
            // ============================================================

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Llamando a CalcularDisponibilidad...");

            var candidatosTurnos =
                await _servicioGuiadas.CalcularDisponibilidad(
                    desde,
                    hasta,
                    guias,
                    visitasGuiadasExistentes,
                    configuracion,
                    calendario);

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Servicio de guiadas devolvió {Cantidad} turnos",
                candidatosTurnos.Count);

            // Log por estado para saber qué está devolviendo el servicio
            var disponiblesAntesFiltro = candidatosTurnos.Count(
                x => x.EstadoTurno == EstadoTurno.Disponible);

            var noDisponiblesAntesFiltro =
                candidatosTurnos.Count - disponiblesAntesFiltro;

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Turnos con estado Disponible: {Disponibles}",
                disponiblesAntesFiltro);

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Turnos NO disponibles: {NoDisponibles}",
                noDisponiblesAntesFiltro);

            // ============================================================
            // FILTRO POR ESTADO
            // ============================================================

            var candidatosFiltrados = candidatosTurnos
                .Where(turno => turno.EstadoTurno == EstadoTurno.Disponible)
                .ToList();

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Después de filtrar por estado Disponible: {Cantidad}",
                candidatosFiltrados.Count);

            candidatosTurnos = candidatosFiltrados;

            // ============================================================
            // 3) ACTIVIDADES EXISTENTES
            // ============================================================

            var actividadesEnRango =
                await _repositorioActividadMuseo.FindAllAsync(desde, hasta);

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Actividades existentes en rango: {Cantidad}",
                actividadesEnRango.Count);

            var aprobados = new List<TurnoDisponible>();

            // ============================================================
            // 4) CREACIÓN DE CANDIDATOS
            // ============================================================

            var entries = new List<Models.CandidateEntry>();

            // Email and phone once
            var email = new Email("sistema@localhost.com");
            var telefono = new Telefono("1123456789");

            foreach (var turno in candidatosTurnos)
            {
                var timeSlot = new TimeSlot(
                    turno.HorarioTurno.Inicio,
                    turno.HorarioTurno.Fin);

                var candidate = new VisitaGrupalGuiada(
                    usuarioVisitanteId: "system",
                    nivelEducativo: null,
                    anioGrado: null,
                    cantidadPersonas: 1,
                    institucion: "Sistema",
                    emailInstitucion: email,
                    telefonoInstitucion: telefono,
                    provinciaInstitucion: "",
                    departamentoInstitucion: "",
                    ciudadInstitucion: "",
                    descripcionDiversidad: string.Empty,
                    motivoVisita: string.Empty,
                    observaciones: string.Empty,
                    timeSlots: new[] { timeSlot },
                    tematicas: new TematicaVisita[] { },
                    salas: null
                );

                var entry = new Models.CandidateEntry(
                    Guid.NewGuid(),
                    candidate,
                    turno,
                    "GuidedService");

                entries.Add(entry);
            }

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] CandidateEntry creados: {Cantidad}",
                entries.Count);

            // Si llegamos acá con 0, el problema está ANTES del AvailabilityEngine
            if (entries.Count == 0)
            {
                _logger.LogWarning(
                    "[GuidedAvailabilityProducer] No hay entries para enviar al AvailabilityEngine. " +
                    "El problema está antes del engine.");

                return aprobados;
            }

            // ============================================================
            // 5) CONTEXTO PARA AVAILABILITY ENGINE
            // ============================================================

            var baseCtx = new AvailabilityContext
            {
                Start = desde,
                End = hasta,
                ExistingActivities = actividadesEnRango,
                Metadata = new Dictionary<string, object>
                {
                    { AvailabilityMetadataKeys.ExistingActivities, actividadesEnRango },
                    { AvailabilityMetadataKeys.GuidesPrefetched, guias },
                }
            };

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Enviando {Cantidad} candidatos al AvailabilityEngine",
                entries.Count);

            // ============================================================
            // 6) VALIDACIÓN CON AVAILABILITY ENGINE
            // ============================================================

            var results =
                await _engine.CheckManyAsync(baseCtx, entries);

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] AvailabilityEngine devolvió {Cantidad} resultados",
                results.Count);

            var resultadosOk =
                results.Count(x => x.Value.IsOk);

            var resultadosError =
                results.Count - resultadosOk;

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Resultados OK: {Cantidad}",
                resultadosOk);

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Resultados rechazados: {Cantidad}",
                resultadosError);

            // ============================================================
            // 7) RECUPERAR TURNOS APROBADOS
            // ============================================================

            foreach (var entry in entries)
            {
                if (results.TryGetValue(entry.Id, out var res))
                {
                    if (res.IsOk)
                    {
                        if (entry.Original is TurnoDisponible turno)
                        {
                            aprobados.Add(turno);
                        }
                    }
                    else
                    {
                        _logger.LogDebug(
                            "[GuidedAvailabilityProducer] Candidato {Id} rechazado por AvailabilityEngine",
                            entry.Id);
                    }
                }
                else
                {
                    _logger.LogWarning(
                        "[GuidedAvailabilityProducer] No se encontró resultado para CandidateEntry {Id}",
                        entry.Id);
                }
            }

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] ===== FIN =====");

            _logger.LogInformation(
                "[GuidedAvailabilityProducer] Total de turnos aprobados: {Cantidad}",
                aprobados.Count);

            return aprobados;
        }
    }
}