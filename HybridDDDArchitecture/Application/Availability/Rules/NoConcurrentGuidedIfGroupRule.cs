using System.Linq;
using System.Threading.Tasks;
using Domain.ActividadMuseo.Entities;
using Domain.VisitasGrupales.Entities;

namespace Application.Availability.Rules
{
    // Rule that enforces mutual exclusion between group visits and guided visits
    public class NoConcurrentGuidedIfGroupRule : Application.Availability.IAvailabilityRule
    {
        public Task<Application.Availability.AvailabilityResult> CheckAsync(Application.Availability.AvailabilityContext ctx, Domain.ActividadMuseo.Entities.ActividadMuseo candidate)
        {
            // If candidate is a group visit (autoguiada) and there is a guided visit overlapping -> fail
            if (candidate is VisitaGrupalAutoguiada)
            {
                if (ctx.ExistingActivities.Any(a => a is VisitaGrupalGuiada))
                    return Task.FromResult(Application.Availability.AvailabilityResult.Fail("No se permiten visitas grupales cuando hay una visita guiada programada"));
            }

            // If candidate is a guided visit and there is any group visit overlapping -> fail
            if (candidate is VisitaGrupalGuiada)
            {
                if (ctx.ExistingActivities.Any(a => a is VisitaGrupalAutoguiada))
                    return Task.FromResult(Application.Availability.AvailabilityResult.Fail("No se permite programar una visita guiada si hay visitas grupales autoguiadas solapadas"));
            }

            return Task.FromResult(Application.Availability.AvailabilityResult.Ok());
        }
    }
}
