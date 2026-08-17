using System.Linq;
using System.Threading.Tasks;
using Application.Availability;
using Domain.ActividadMuseo.Entities;
using Domain.VisitasGrupales.Entities;
using Domain.VisitasGrupales.Entities.GrupalGuiada;
using Microsoft.Extensions.Logging;

namespace Application.Availability.Rules
{
    /// <summary>
    /// Regla que impide la convivencia entre visitas guiadas y visitas grupales autoguiadas
    /// en el mismo intervalo horario / sala.
    /// </summary>
    public class NoConcurrentGuidedWithAutoguidedRule : IAvailabilityRule
    {
        private readonly ILogger<NoConcurrentGuidedWithAutoguidedRule> _logger;

        public NoConcurrentGuidedWithAutoguidedRule(ILogger<NoConcurrentGuidedWithAutoguidedRule> logger)
        {
            _logger = logger;
        }

        public Task<AvailabilityResult> CheckAsync(AvailabilityContext ctx, Domain.ActividadMuseo.Entities.ActividadMuseo candidate)
        {
            // Aplica sólo a actividades grupales
            if (candidate is not VisitaGrupalGuiada && candidate is not VisitaGrupalAutoguiada)
                return Task.FromResult(AvailabilityResult.Ok());

            var candidateType = candidate is VisitaGrupalGuiada ? "Guiada" : "Autoguiada";
            _logger.LogDebug("Checking concurrent visits for {CandidateType} candidate {CandidateId}",
                candidateType, candidate.Id);

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
            {
                _logger.LogDebug("Concurrent visit conflict detected for {CandidateType} candidate {CandidateId}",
                    candidateType, candidate.Id);
                return Task.FromResult(AvailabilityResult.Fail("No se permiten visitas guiadas y autoguiadas en el mismo horario."));
            }

            _logger.LogDebug("No concurrent visit conflicts for candidate {CandidateId}", candidate.Id);
            return Task.FromResult(AvailabilityResult.Ok());
        }
    }
}
