using Application.VisitaGrupal.Repositories;
using Domain.VisitasGrupales.DomainServices;
using Domain.VisitasGrupales.Entities;
using Domain.Common.ValueObjets;
using static Domain.VisitasGrupales.Enums.Enums;
using Domain.VisitasGrupales.Entities.GrupalGuiada;

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

        public GuidedAvailabilityProducerService(
            IServicioDisponibilidadTurnosVisitasGuiadas servicioGuiadas,
            IRepositorioGuia repositorioGuia,
            IRepositorioVisitaGuiada repositorioVisitaGuiada,
            Repositories.IRepositorioDiaCierreMuseo repositorioDiasCierre,
            ActividadMuseo.Repositories.IRepositorioActividadMuseo repositorioActividadMuseo,
            IRepositorioConfiguracionVisitasGrupalesGuiadas repositorioConfiguracion,
            ActividadMuseo.Repositories.IRepositorioCalendarioMuseo repositorioCalendario,
            AvailabilityEngine engine,
            IServiceProvider serviceProvider)
        {
            _servicioGuiadas = servicioGuiadas;
            _repositorioGuia = repositorioGuia;
            _repositorioVisitaGuiada = repositorioVisitaGuiada;
            _repositorioActividadMuseo = repositorioActividadMuseo;
            _repositorioConfiguracion = repositorioConfiguracion;
            _repositorioCalendario = repositorioCalendario;
            _engine = engine;
        }


        // Devuelve la lista de turnos aprobados por las reglas de disponibilidad
        public async Task<List<TurnoDisponible>> GetAvailableTurnsAsync(DateTime desde, DateTime hasta)
        {
            // 1) Prefetch datos que el servicio de guiadas necesita
            var guias = await _repositorioGuia.ObtenerGuiasConDisponibilidadAsync();
            var visitasGuiadasExistentes = await _repositorioVisitaGuiada.GetAllGroupVisitAsync(desde, hasta);
            var configuracion = await _repositorioConfiguracion.ObtenerConfiguracionActivaAsync();
            var calendario = await _repositorioCalendario.ObtenerCalendarioActivoAsync();

            if (configuracion == null)
                throw new InvalidOperationException("No hay configuración activa para visitas guiadas");

            if (calendario == null)
                throw new InvalidOperationException("No hay calendario activo del museo");

            // 2) Llamar al servicio de dominio de guiadas que calcula los turnos candidatos
            // Este servicio ya valida: calendario, configuración, guías y capacidad
            var candidatosTurnos = await _servicioGuiadas.CalcularDisponibilidad(
                desde, hasta, guias, visitasGuiadasExistentes, configuracion, calendario);

            // Filtrar turnos que el servicio de dominio ya marcó como no disponibles
            // (el servicio ya valida calendario, configuración, guías y capacidad)
            var candidatosFiltrados = candidatosTurnos
                .Where(turno => turno.EstadoTurno == EstadoTurno.Disponible);

            candidatosTurnos = [.. candidatosFiltrados];

            // 3) Prefetch actividades del rango (para todas las salas) and other common data
            var actividadesEnRango = await _repositorioActividadMuseo.FindAllAsync(desde, hasta);

            var aprobados = new List<TurnoDisponible>();

            // 3.1) Optionally prefetch recurrence rules (blocking rules) and expand into blocked slots
            

            // Build candidate entries list and keep mapping to original turno using stable Ids
            var entries = new List<Models.CandidateEntry>();
            var candidates = new List<Domain.ActividadMuseo.Entities.ActividadMuseo>();

            // Email and phone once
            var email = new Email("sistema@localhost.com");
            var telefono = new Telefono("1123456789");

            foreach (var turno in candidatosTurnos)
            {
                var timeSlot = new TimeSlot(turno.HorarioTurno.Inicio, turno.HorarioTurno.Fin);

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

                var entry = new Models.CandidateEntry(Guid.NewGuid(), candidate, turno, "GuidedService");
                entries.Add(entry);
                candidates.Add(candidate);
            }

            // Build a base context that contains prefetched data for reuse
            // Nota: No incluimos DiasCierre porque el servicio de dominio ya validó el calendario
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

            // If you have a repository for exhibits (muestras), prefetch them here and put in metadata
            // Example (commented because repository may not exist yet):
            // var exhibits = await _repositorioMuestra.FindAllAsync(desde, hasta);
            // baseCtx.Metadata[AvailabilityMetadataKeys.Exhibits] = exhibits;

            // Ask engine to check all entries (returns results keyed by entry.Id)
            var results = await _engine.CheckManyAsync(baseCtx, entries);

            foreach (var entry in entries)
            {
                if (results.TryGetValue(entry.Id, out var res) && res.IsOk)
                {
                    if (entry.Original is TurnoDisponible turno)
                    {
                        aprobados.Add(turno);
                    }
                }
            }

            return aprobados;
        }
    }
}
