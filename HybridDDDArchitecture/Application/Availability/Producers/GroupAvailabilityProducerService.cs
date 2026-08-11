using Application.Availability.Models;
using Application.Exceptions;
using Application.MuseumResources.Repositories;
using Application.VisitaGrupal.Repositories;
using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.DomainServices;
using Domain.VisitasGrupales.Entities;

using static Domain.VisitasGrupales.Enums.Enums;

/*DateTime fechaDesde,
            DateTime fechaHasta,
            IReadOnlyCollection<VisitaGrupalAutoguiada> visitasAutoguiadas,
            ConfiguracionHorarioAutoguiada configuracion,
            CalendarioMuseo calendario*/

namespace Application.Availability.Producers
{
    /// <summary>
    /// Producer que genera bloques horarios (por ejemplo 1 hora) para visitas grupales (autoguiadas)
    /// y valida las candidatas contra el AvailabilityEngine.
    /// </summary>
    public class GroupAvailabilityProducerService
    {
        private readonly IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada;
        private readonly ActividadMuseo.Repositories.IRepositorioActividadMuseo _repositorioActividadMuseo;
        private readonly IRepositorioConfiguracionHorarioAutoguiada repositorioConfiguracionHorarioAutoguiada;
        private readonly ActividadMuseo.Repositories.IRepositorioCalendarioMuseo _repositorioCalendario;
        private readonly IRepositorioSala repositorioSala;
        private readonly IServicioDisponibilidadSlotsAutoguiadas servicioDisponibilidadSlotsAutoguiadas;
        private readonly AvailabilityEngine _engine;
        private readonly IRepositorioTematicas _tematicaRepository;

        public GroupAvailabilityProducerService(
            IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada,
            IRepositorioConfiguracionHorarioAutoguiada repositorioConfiguracionHorarioAutoguiada,
            ActividadMuseo.Repositories.IRepositorioCalendarioMuseo repositorioCalendario,
            Application.ActividadMuseo.Repositories.IRepositorioActividadMuseo repositorioActividadMuseo,
            IServicioDisponibilidadSlotsAutoguiadas servicioDisponibilidadSlotsAutoguiadas,
            IRepositorioSala repositorioSala,
            IRepositorioTematicas tematicaRepository,
            AvailabilityEngine engine)
        {
            _repositorioActividadMuseo = repositorioActividadMuseo;
            this.repositorioVisitaGrupalAutoguiada = repositorioVisitaGrupalAutoguiada;
            this.servicioDisponibilidadSlotsAutoguiadas = servicioDisponibilidadSlotsAutoguiadas;
            this.repositorioConfiguracionHorarioAutoguiada = repositorioConfiguracionHorarioAutoguiada;
            this.repositorioSala = repositorioSala;
            _repositorioCalendario = repositorioCalendario;
            _tematicaRepository = tematicaRepository;
            _engine = engine;
        }

        /// <summary>
        /// Genera bloques horarios entre "desde" y "hasta" (inclusive dates) para las salas indicadas.
        /// Por defecto genera bloques de 1 hora entre startHour (incl) y endHour (excl).
        /// </summary>
        /// 



        public async Task<List<SlotDisponibleVisitaAutoguiada>> GetHourlyBlocksAsync(DateTime desde, DateTime hasta, List<int>? tematicasSeleccionadasIds = null)
        {
            /*DateTime fechaDesde,
            DateTime fechaHasta,
            IReadOnlyCollection<VisitaGrupalAutoguiada> visitasAutoguiadas,
            ConfiguracionHorarioAutoguiada configuracion,
            CalendarioMuseo calendario*/
            var VisitasAutoguiadasExistentes= await repositorioVisitaGrupalAutoguiada.GetAllGroupVisitAuAsync(desde, hasta);
            var configuracion = await repositorioConfiguracionHorarioAutoguiada.ObtenerConfiguracionActivaAsync();
            var calendario = await _repositorioCalendario.ObtenerCalendarioActivoAsync();

            if (configuracion == null)
                throw new InvalidOperationException("No hay configuración activa para visitas guiadas");

            if (calendario == null)
                throw new InvalidOperationException("No hay calendario activo del museo");
            // Llamar al servicio de dominio de guiadas que calcula los turnos candidatos
            // Este servicio ya valida: calendario, configuración, capacidad contra visitasAutoguiadasExistentes, y devuelve los bloques horarios candidatos.
            var candidatosTurnos = await servicioDisponibilidadSlotsAutoguiadas.CalcularDisponibilidad(desde, hasta, VisitasAutoguiadasExistentes, configuracion, calendario);

            var candidatosFiltrados = candidatosTurnos
                .Where(turno => turno.EstadoSlot == EstadoTurno.Disponible);

            candidatosTurnos = [.. candidatosFiltrados];

            // Prefetch common data

            var actividadesExistentes = await _repositorioActividadMuseo.FindAllAsync(desde, hasta);

            // Load salas and filter if salaIds provided
            var tematicas = await _tematicaRepository
                .GetByIdsAsync(tematicasSeleccionadasIds);
            if (tematicas.Count != tematicasSeleccionadasIds.Count)
            {
                throw new BussinessException(
                    "Una o más temáticas no existen.");
            }
            var salas = tematicas
                .SelectMany(t => t.Salas)
                .DistinctBy(s => s.Id)
                .ToList();

            // Build entries
            var entries = new List<CandidateEntry>();
            var candidates = new List<Domain.ActividadMuseo.Entities.ActividadMuseo>();


            var email = new Email("sistema@localhost.com");
            var telefono = new Telefono("1123456789");

            foreach (var turno in candidatosTurnos)
            {
                
                
                var timeSlot = new TimeSlot(turno.HorarioSlot.Inicio, turno.HorarioSlot.Fin);

                var candidate = new VisitaGrupalAutoguiada(

                    usuarioVisitanteId: "system",
                    cantidadPersonas: turno.cuposDisponibles,
                    institucion: "Sistema",
                    emailInstitucion: email,
                    provinciaInstitucion: "Sistema",
                    departamentoInstitucion: "Sistema",
                    ciudadInstitucion: "Sistema",
                    descripcionDiversidad: string.Empty,
                    observaciones: string.Empty,
                    timeSlots: new[] { timeSlot },
                    salas: salas,
                    tematicas:tematicas


                );
                var entry = new CandidateEntry(Guid.NewGuid(), candidate, turno, "self-guidedservice");
                entries.Add(entry);
                candidates.Add(candidate);
            }


            // Build base context
            var baseCtx = new AvailabilityContext
            {
                Start = desde,
                End = hasta,
                ExistingActivities = actividadesExistentes,
                Metadata = new Dictionary<string, object>
                {
                    { AvailabilityMetadataKeys.ExistingActivities, actividadesExistentes }
                }
            };

            // Optionally prefetch exhibits here if repository exists (commented)
            // var exhibits = await _repositorioMuestra.FindAllAsync(desde, hasta);
            // baseCtx.Metadata[AvailabilityMetadataKeys.Exhibits] = exhibits;

            // Validate all entries
            var results = await _engine.CheckManyAsync(baseCtx, entries);

            // Return originals for approved entries
            var approved = new List<SlotDisponibleVisitaAutoguiada>();
            foreach (var entry in entries)
            {
                if (results.TryGetValue(entry.Id, out var res) && res.IsOk)
                {
                    if (entry.Original is SlotDisponibleVisitaAutoguiada hb)
                        approved.Add(hb);
                }
            }

            return approved;
        }
    }
}
