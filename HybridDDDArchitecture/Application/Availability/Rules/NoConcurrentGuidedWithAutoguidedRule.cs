using System.Linq;
using System.Threading.Tasks;
using Application.Availability;
using Domain.ActividadMuseo.Entities;
using Domain.VisitasGrupales.Entities;

namespace Application.Availability.Rules
{
    /// <summary>
    /// Regla que impide la convivencia entre visitas guiadas y visitas grupales autoguiadas
    /// en el mismo intervalo horario / sala.
    /// </summary>
    public class NoConcurrentGuidedWithAutoguidedRule : IAvailabilityRule
    {
        public Task<AvailabilityResult> CheckAsync(AvailabilityContext ctx, ActividadMuseo candidate)
        {
            // Aplica sólo a actividades grupales
            if (candidate is not VisitaGrupalGuiada && candidate is not VisitaGrupalAutoguiada)
                return Task.FromResult(AvailabilityResult.Ok());

            var candidateSlots = candidate.TimeSlots.ToList();

            // Buscar en las actividades existentes solapamientos con el tipo contrario
            var conflict = ctx.ExistingActivities
                .Where(a => a != null)
                .Any(existing =>
                {
                    // Si candidate es guiada, buscamos autoguiadas; y viceversa
                    if (candidate is VisitaGrupalGuiada && existing is VisitaGrupalAutoguiada) return candidateSlots.Any(cs => existing.TimeSlots.Any(es => cs.SeSolapaCon(es)));
                    if (candidate is VisitaGrupalAutoguiada && existing is VisitaGrupalGuiada) return candidateSlots.Any(cs => existing.TimeSlots.Any(es => cs.SeSolapaCon(es)));
                    return false;
                });

            if (conflict)
                return Task.FromResult(AvailabilityResult.Fail("No se permiten visitas guiadas y autoguiadas en el mismo horario."));

            return Task.FromResult(AvailabilityResult.Ok());
        }
    }
}
