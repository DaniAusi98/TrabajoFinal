using System.Collections.Generic;
using Domain.ActividadMuseo.Entities;

namespace Application.Availability
{
    public class AvailabilityContext
    {
        public System.DateTime Start { get; init; }
        public System.DateTime End { get; init; }

        // Activities that overlap in time/space with the candidate (prepared by application layer)
        public IReadOnlyCollection<Domain.ActividadMuseo.Entities.ActividadMuseo> ExistingActivities { get; init; } = new List<Domain.ActividadMuseo.Entities.ActividadMuseo>();

        // Typed optional properties commonly used by rules (prefetched by orchestrator)

        public IReadOnlyDictionary<string, object> Metadata { get; init; } = new Dictionary<string, object>();
    }
}
