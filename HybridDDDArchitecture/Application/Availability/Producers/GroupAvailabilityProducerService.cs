/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Availability.Models;
using Application.MuseumResources.Repositories;
using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;
using Domain.VisitasGrupales.Entities;

namespace Application.Availability.Producers
{
    /// <summary>
    /// Producer que genera bloques horarios (por ejemplo 1 hora) para visitas grupales (autoguiadas)
    /// y valida las candidatas contra el AvailabilityEngine.
    /// </summary>
    public class GroupAvailabilityProducerService
    {
        private readonly Application.ActividadMuseo.Repositories.IRepositorioActividadMuseo _repositorioActividadMuseo;
        private readonly Application.Repositories.IRepositorioDiaCierreMuseo _repositorioDiasCierre;
        private readonly IRepositorioSala _repositorioSala;
        private readonly AvailabilityEngine _engine;

        public GroupAvailabilityProducerService(
            Application.ActividadMuseo.Repositories.IRepositorioActividadMuseo repositorioActividadMuseo,
            Application.Repositories.IRepositorioDiaCierreMuseo repositorioDiasCierre,
            IRepositorioSala repositorioSala,
            AvailabilityEngine engine)
        {
            _repositorioActividadMuseo = repositorioActividadMuseo;
            _repositorioDiasCierre = repositorioDiasCierre;
            _repositorioSala = repositorioSala;
            _engine = engine;
        }

        /// <summary>
        /// Genera bloques horarios entre "desde" y "hasta" (inclusive dates) para las salas indicadas.
        /// Por defecto genera bloques de 1 hora entre startHour (incl) y endHour (excl).
        /// </summary>
        public async Task<List<HourBlockDto>> GetHourlyBlocksAsync(DateTime desde, DateTime hasta, IEnumerable<int>? salaIds = null, int startHour = 9, int endHour = 17)
        {
            // Prefetch common data
            var actividadesEnRango = await _repositorioActividadMuseo.FindAllAsync(desde, hasta);
            var diasCierre = await _repositorioDiasCierre.FindAllAsync();

            // Load salas and filter if salaIds provided
            var salasAll = await _repositorioSala.FindAllAsync();
            var salas = salaIds != null ? salasAll.Where(s => salaIds.Contains(s.Id)).ToList() : salasAll;

            // Build entries
            var entries = new List<Application.Availability.Models.CandidateEntry>();

            var email = new Email("sistema@localhost.com");
            var telefono = new Telefono("1123456789");

            for (var day = desde.Date; day <= hasta.Date; day = day.AddDays(1))
            {
                for (int h = startHour; h < endHour; h++)
                {
                    var inicio = day.AddHours(h);
                    var fin = inicio.AddHours(1);

                    // If no specific salas requested, create a candidate with salas = null
                    if (salas == null || !salas.Any())
                    {
                        var candidate = new VisitaGrupalAutoguiada(
                            usuarioVisitanteId: "system",
                            cantidadPersonas: 1,
                            institucion: "Sistema",
                            emailInstitucion: email,
                            provinciaInstitucion: string.Empty,
                            departamentoInstitucion: string.Empty,
                            ciudadInstitucion: string.Empty,
                            descripcionDiversidad: string.Empty,
                            observaciones: string.Empty,
                            timeSlots: new[] { new Domain.Common.ValueObjets.TimeSlot(inicio, fin) },
                            salas: null
                        );

                        var original = new HourBlockDto(null, inicio, fin);
                        var entry = new Application.Availability.Models.CandidateEntry(Guid.NewGuid(), candidate, original, "GroupProducer");
                        entries.Add(entry);
                    }
                    else
                    {
                        foreach (var sala in salas)
                        {
                            var candidate = new VisitaGrupalAutoguiada(
                                usuarioVisitanteId: "system",
                                cantidadPersonas: 1,
                                institucion: "Sistema",
                                emailInstitucion: email,
                                provinciaInstitucion: string.Empty,
                                departamentoInstitucion: string.Empty,
                                ciudadInstitucion: string.Empty,
                                descripcionDiversidad: string.Empty,
                                observaciones: string.Empty,
                                timeSlots: new[] { new Domain.Common.ValueObjets.TimeSlot(inicio, fin) },
                                salas: new[] { sala }
                            );

                            var original = new HourBlockDto(sala.Id, inicio, fin);
                            var entry = new Application.Availability.Models.CandidateEntry(Guid.NewGuid(), candidate, original, "GroupProducer");
                            entries.Add(entry);
                        }
                    }
                }
            }

            // Build base context
            var baseCtx = new AvailabilityContext
            {
                Start = desde,
                End = hasta,
                ExistingActivities = actividadesEnRango,
                DiasCierre = diasCierre,
                Metadata = new Dictionary<string, object>
                {
                    { AvailabilityMetadataKeys.DiasCierre, diasCierre },
                    { AvailabilityMetadataKeys.ExistingActivities, actividadesEnRango }
                }
            };

            // Optionally prefetch exhibits here if repository exists (commented)
            // var exhibits = await _repositorioMuestra.FindAllAsync(desde, hasta);
            // baseCtx.Metadata[AvailabilityMetadataKeys.Exhibits] = exhibits;

            // Validate all entries
            var results = await _engine.CheckManyAsync(baseCtx, entries);

            // Return originals for approved entries
            var approved = new List<HourBlockDto>();
            foreach (var entry in entries)
            {
                if (results.TryGetValue(entry.Id, out var res) && res.IsOk)
                {
                    if (entry.Original is HourBlockDto hb)
                        approved.Add(hb);
                }
            }

            return approved;
        }
    }
}
*/