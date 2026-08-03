using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.VisitaGrupal.Repositories;
using Application.Availability.Recurrence;
using Microsoft.Extensions.DependencyInjection;
using System;
using Domain.VisitasGrupales.DomainServices;
using Domain.VisitasGrupales.Entities;
using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;

namespace Application.Availability.Producers
{
    // Orquestador que obtiene los turnos del servicio de guiadas,
    // construye candidatas mínimas y las valida una por una con el AvailabilityEngine
    public class GuidedAvailabilityProducerService
    {
        private readonly IServicioDisponibilidadTurnosVisitasGuiadas _servicioGuiadas;
        private readonly IRepositorioGuia _repositorioGuia;
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada;
        private readonly Application.Repositories.IRepositorioDiaCierreMuseo _repositorioDiasCierre;
        private readonly Application.ActividadMuseo.Repositories.IRepositorioActividadMuseo _repositorioActividadMuseo;
        private readonly AvailabilityEngine _engine;
        private readonly IServiceProvider _serviceProvider;

        public GuidedAvailabilityProducerService(
            IServicioDisponibilidadTurnosVisitasGuiadas servicioGuiadas,
            IRepositorioGuia repositorioGuia,
            IRepositorioVisitaGuiada repositorioVisitaGuiada,
            Application.Repositories.IRepositorioDiaCierreMuseo repositorioDiasCierre,
            Application.ActividadMuseo.Repositories.IRepositorioActividadMuseo repositorioActividadMuseo,
            AvailabilityEngine engine,
            IServiceProvider serviceProvider)
        {
            _servicioGuiadas = servicioGuiadas;
            _repositorioGuia = repositorioGuia;
            _repositorioVisitaGuiada = repositorioVisitaGuiada;
            _repositorioDiasCierre = repositorioDiasCierre;
            _repositorioActividadMuseo = repositorioActividadMuseo;
            _engine = engine;
            _serviceProvider = serviceProvider;
        }

        // Devuelve la lista de turnos aprobados por las reglas de disponibilidad
        // blockedWeekDays: optional list of weekdays where guided visits are not allowed (e.g. Tue/Wed)
        public async Task<List<TurnoDisponible>> GetAvailableTurnsAsync(System.DateTime desde, System.DateTime hasta, IEnumerable<System.DayOfWeek>? blockedWeekDays = null)
        {
            // 1) Prefetch datos que el servicio de guiadas necesita
            var guias = await _repositorioGuia.ObtenerGuiasConDisponibilidadAsync();
            var visitasGuiadasExistentes = await _repositorioVisitaGuiada.GetAllGroupVisitAsync(desde, hasta);
            var diasCierre = await _repositorioDiasCierre.FindAllAsync();

            // 2) Llamar al servicio de dominio de guiadas que calcula los turnos candidatos
            var candidatosTurnos = await _servicioGuiadas.CalcularDisponibilidad(desde, hasta, guias, visitasGuiadasExistentes);

            // Prefilter by museum closed days (diasCierre) and optional blocked weekdays to avoid creating candidates
            var candidatosFiltrados = candidatosTurnos
                .Where(turno => !diasCierre.Any(dc => dc.SolapaConFechas(turno.HorarioTurno.Inicio, turno.HorarioTurno.Fin)));

           /* if (blockedWeekDays != null && blockedWeekDays.Any())
            {
                candidatosFiltrados = candidatosFiltrados
                    .Where(turno => !blockedWeekDays.Contains(turno.HorarioTurno.Inicio.DayOfWeek));
            }
           */
            candidatosTurnos = [.. candidatosFiltrados];

            // 3) Prefetch actividades del rango (para todas las salas) and other common data
            var actividadesEnRango = await _repositorioActividadMuseo.FindAllAsync(desde, hasta);

            var aprobados = new List<TurnoDisponible>();

            // 3.1) Optionally prefetch recurrence rules (blocking rules) and expand into blocked slots
            var blockedSlots = new List<Domain.Common.ValueObjets.TimeSlot>();
            try
            {
                var recurrenceRepo = _serviceProvider.GetService<IRecurrenceRuleRepository>();
                if (recurrenceRepo != null)
                {
                    var rules = await recurrenceRepo.GetRulesAsync(desde, hasta, true);
                    foreach (var r in rules ?? new List<RecurrenceRule>())
                    {
                        blockedSlots.AddRange(RecurrenceExpander.Expand(r, desde, hasta));
                    }
                }
            }
            catch
            {
                // ignore if no repository available or expansion fails; engine rules still apply
            }

            // If we have blocked slots, filter candidatosTurnos to skip ones that overlap
            if (blockedSlots.Any())
            {
                candidatosTurnos = candidatosTurnos.Where(turno => !blockedSlots.Any(bs => turno.HorarioTurno.SeSolapaCon(bs))).ToList();
            }

            // Build candidate entries list and keep mapping to original turno using stable Ids
            var entries = new List<Application.Availability.Models.CandidateEntry>();
            var candidates = new List<Domain.ActividadMuseo.Entities.ActividadMuseo>();

            // Email and phone once
            var email = new Domain.Common.ValueObjets.Email("sistema@localhost.com");
            var telefono = new Domain.Common.ValueObjets.Telefono("1123456789");

            foreach (var turno in candidatosTurnos)
            {
                var timeSlot = new TimeSlot(turno.HorarioTurno.Inicio, turno.HorarioTurno.Fin);

                // If you prefetched exhibits, you could filter out candidates that conflict here to avoid
                // sending them to the engine. Example (commented because exhibits repo may not exist):
                // if (baseCtx.Metadata.TryGetValue(AvailabilityMetadataKeys.Exhibits, out var exObj) && exObj is IEnumerable<ActividadMuseo> exhibits) {
                //     if (exhibits.Any(e => e.TimeSlots.Any(ts => ts.SeSolapaCon(new TimeSlot(timeSlot.Inicio, timeSlot.Fin))))) continue; // skip this turno
                // }

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
                    tematicas: new Domain.VisitasGrupales.Entities.TematicaVisita[] { },
                    salas: null
                );

                var entry = new Models.CandidateEntry(Guid.NewGuid(), candidate, turno, "GuidedService");
                entries.Add(entry);
                candidates.Add(candidate);
            }

            // Build a base context that contains prefetched data for reuse
            var baseCtx = new AvailabilityContext
            {
                Start = desde,
                End = hasta,
                ExistingActivities = actividadesEnRango,
                DiasCierre = diasCierre,
                Metadata = new Dictionary<string, object>
                {
                    { AvailabilityMetadataKeys.DiasCierre, diasCierre },
                    { AvailabilityMetadataKeys.ExistingActivities, actividadesEnRango },
                    { AvailabilityMetadataKeys.GuidesPrefetched, guias },
                    { AvailabilityMetadataKeys.BlockedRecurrenceSlots, blockedSlots }
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
