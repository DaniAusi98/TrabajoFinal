using System.Linq;
using System.Threading.Tasks;
using Application.Availability;
using Domain.ActividadMuseo.Entities;
using Domain.VisitasGrupales.Entities;

namespace Application.Availability.Rules
{
    /// <summary>
    /// Bloquea visitas grupales si existe una muestra cuya etapa de montaje o desmontaje solapa con la candidata.
    /// Nota: la entidad Muestra debe exponer los time slots de montaje/desmontaje o guardarlos en sus TimeSlots;
    /// si no, adaptar la comprobación para leer los campos específicos de la entidad.
    /// </summary>
    public class NoGroupIfExhibitInMountingOrDisassemblyRule : IAvailabilityRule
    {
        public Task<AvailabilityResult> CheckAsync(AvailabilityContext ctx, ActividadMuseo candidate)
        {
            if (candidate is not VisitaGrupalGuiada && candidate is not VisitaGrupalAutoguiada)
                return Task.FromResult(AvailabilityResult.Ok());

            var candidateSlots = candidate.TimeSlots.ToList();

            // Preferir exhibits prefetechados en ctx.Metadata[AvailabilityMetadataKeys.Exhibits].
            // Si no vienen en metadata, no realizamos ninguna comprobación aquí (dejamos Ok) —
            // el producer debe prefetchear y filtrar candidatos cuando corresponda.
            if (ctx.Metadata == null || !ctx.Metadata.TryGetValue(AvailabilityMetadataKeys.Exhibits, out var exhibitsObj) || exhibitsObj is not System.Collections.Generic.IEnumerable<ActividadMuseo> exhibitsList)
            {
                // No hay información de muestras prefetechada: no aplicamos esta regla
                return Task.FromResult(AvailabilityResult.Ok());
            }

            // Comparar los TimeSlots de la candidata con los TimeSlots de cada muestra prefetechada.
            // Se asume que las muestras en metadata contienen los time slots de montaje/desmontaje o los intervalos relevantes.
            foreach (var muestra in exhibitsList)
            {
                if (muestra?.TimeSlots == null) continue;
                if (candidateSlots.Any(cs => muestra.TimeSlots.Any(ms => cs.SeSolapaCon(ms))))
                    return Task.FromResult(AvailabilityResult.Fail("No se permiten visitas mientras la muestra se encuentra en montaje o desmontaje."));
            }

            return Task.FromResult(AvailabilityResult.Ok());
        }
    }
}
