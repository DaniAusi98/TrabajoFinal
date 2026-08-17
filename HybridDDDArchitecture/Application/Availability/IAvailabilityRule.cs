
namespace Application.Availability
{
    public interface IAvailabilityRule
    {
        Task<AvailabilityResult> CheckAsync(AvailabilityContext ctx, Domain.ActividadMuseo.Entities.ActividadMuseo candidate);
    }
}
