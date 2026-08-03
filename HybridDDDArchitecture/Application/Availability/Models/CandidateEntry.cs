using System;
using System;
using Domain.ActividadMuseo.Entities;

namespace Application.Availability.Models
{
    // Generic in-memory wrapper that connects a candidate activity with its original DTO/source.
    // Original is typed as object to allow different producers (guided, event, educational) to attach
    // their domain-specific DTO (e.g. TurnoDisponible, EventDto, etc.).
    public record CandidateEntry(Guid Id, Domain.ActividadMuseo.Entities.ActividadMuseo Candidate, object? Original, string Source);

    /*
    // Alternative strongly-typed generic variant (commented):
    public record CandidateEntry<TOriginal>(Guid Id, ActividadMuseo Candidate, TOriginal Original, string Source);
    */
}
