using System.Linq;
using System.Threading.Tasks;
using Application.Availability;
using Domain.ActividadMuseo.Entities;
using Domain.VisitasGrupales.Entities;
using Domain.RecursoMuseo.Enums;

namespace Application.Availability.Rules
{
    /// <summary>
    /// Bloquea visitas grupales en salas tipo Hall cuando hay eventos o actividades educativas solapadas.
    /// </summary>
    public class NoGroupIfHallEventOrEducationalRule : IAvailabilityRule
    {
        public Task<AvailabilityResult> CheckAsync(AvailabilityContext ctx, ActividadMuseo candidate)
        {
            // Aplica solo a visitas grupales
            if (candidate is not VisitaGrupalGuiada && candidate is not VisitaGrupalAutoguiada)
                return Task.FromResult(AvailabilityResult.Ok());

            // Si la candidata no tiene salas asignadas, conservadoramente no la bloqueamos aquí
            var salas = candidate.Salas ?? new System.Collections.Generic.List<Domain.RecursoMuseo.Entities.Sala>();
            if (!salas.Any() || !salas.Any(s => s.TipoSala == TipoSala.Hall))
                return Task.FromResult(AvailabilityResult.Ok());

            var candidateSlots = candidate.TimeSlots.ToList();

            // Buscar eventos o actividades educativas en las mismas salas y que solapen
            var conflict = ctx.ExistingActivities
                .Where(a => a != null)
                .Any(existing =>
                {
                    if (existing.TipoActividad != Domain.ActividadMuseo.Enums.Enums.TipoActividad.Evento
                        && existing.TipoActividad != Domain.ActividadMuseo.Enums.Enums.TipoActividad.ActividadEducativa)
                        return false;

                    // comprobar sala en común
                    if (!existing.Salas.Any(es => salas.Any(s => s.Id == es.Id)))
                        return false;

                    return candidateSlots.Any(cs => existing.TimeSlots.Any(es => cs.SeSolapaCon(es)));
                });

            if (conflict)
                return Task.FromResult(AvailabilityResult.Fail("No se permiten visitas grupales en el hall cuando hay eventos o actividades educativas programadas."));

            return Task.FromResult(AvailabilityResult.Ok());
        }
    }
}
