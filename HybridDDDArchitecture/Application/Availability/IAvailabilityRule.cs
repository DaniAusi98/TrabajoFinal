using System.Threading.Tasks;
using Domain.ActividadMuseo.Entities;

namespace Application.Availability
{
    public interface IAvailabilityRule
    {
        Task<AvailabilityResult> CheckAsync(AvailabilityContext ctx, Domain.ActividadMuseo.Entities.ActividadMuseo candidate);
    }
}
