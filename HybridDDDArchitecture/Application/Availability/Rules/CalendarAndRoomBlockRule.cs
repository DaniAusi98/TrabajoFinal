using System.Linq;
using System.Threading.Tasks;
using Application.Availability;
using Domain.ActividadMuseo.Entities;

namespace Application.Availability.Rules
{
    /// <summary>
    /// Valida cierres del museo y bloqueos de sala. También puede usarse como defensa final
    /// frente a candidatos que deberían haber sido prefiltados por los producers.
    /// </summary>
    public class CalendarAndRoomBlockRule : IAvailabilityRule
    {
        public Task<AvailabilityResult> CheckAsync(AvailabilityContext ctx, ActividadMuseo candidate)
        {
            // Comprobar días de cierre prefetechados
            if (ctx.DiasCierre != null && ctx.DiasCierre.Any(dc => dc.SolapaConFechas(GetCandidateStart(candidate), GetCandidateEnd(candidate))))
            {
                return Task.FromResult(AvailabilityResult.Fail("El museo está cerrado en la fecha solicitada."));
            }

            // Comprobar bloqueos de sala si se pasaron prefetechados
            if (ctx.BloqueosSala != null && candidate.Salas != null && candidate.Salas.Any())
            {
                foreach (var bloqueo in ctx.BloqueosSala)
                {
                    foreach (var sala in candidate.Salas)
                    {
                        if (bloqueo.SalaId == sala.Id && BloqueoSolapa(bloqueo, candidate))
                            return Task.FromResult(AvailabilityResult.Fail("La sala está bloqueada en el intervalo solicitado."));
                    }
                }
            }

            // Nota: reglas recurrentes (p. ej. "no guiadas lunes y miércoles") deberían aplicarse en el producer
            return Task.FromResult(AvailabilityResult.Ok());
        }

        private static DateTime GetCandidateStart(ActividadMuseo candidate)
        {
            return candidate.TimeSlots.Min(ts => ts.Inicio);
        }
        private static DateTime GetCandidateEnd(ActividadMuseo candidate)
        {
            return candidate.TimeSlots.Max(ts => ts.Fin);
        }

        private static bool BloqueoSolapa(BloqueoSala bloqueo, ActividadMuseo candidate)
        {
            var start = GetCandidateStart(candidate);
            var end = GetCandidateEnd(candidate);
            return start < bloqueo.FechaHasta && end > bloqueo.FechaDesde;
        }
    }
}
