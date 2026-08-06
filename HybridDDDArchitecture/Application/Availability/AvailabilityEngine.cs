

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.ActividadMuseo.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Availability
{
    public class AvailabilityEngine
    {
        private readonly IRuleFactory _ruleFactory;
        private readonly ILogger<AvailabilityEngine> _logger;

        public AvailabilityEngine(IRuleFactory ruleFactory, ILogger<AvailabilityEngine> logger)
        {
            _ruleFactory = ruleFactory;
            _logger = logger;
        }

        public async Task<AvailabilityResult> CheckAsync(AvailabilityContext ctx, Domain.ActividadMuseo.Entities.ActividadMuseo candidate)
        {
            var rules = _ruleFactory.GetRulesFor(candidate);
            var rulesList = rules.ToList();

            _logger.LogInformation("Checking availability for {TipoActividad} (Id: {CandidateId}) with {RuleCount} rules",
                candidate.TipoActividad, candidate.Id, rulesList.Count);

            foreach (var rule in rulesList)
            {
                var ruleName = rule.GetType().Name;
                _logger.LogDebug("Executing rule: {RuleName}", ruleName);

                var res = await rule.CheckAsync(ctx, candidate);
                if (!res.IsOk)
                {
                    _logger.LogWarning("Rule {RuleName} failed for {TipoActividad} (Id: {CandidateId}): {Message}",
                        ruleName, candidate.TipoActividad, candidate.Id, res.Message);
                    return res;
                }

                _logger.LogDebug("Rule {RuleName} passed", ruleName);
            }

            _logger.LogInformation("All rules passed for {TipoActividad} (Id: {CandidateId})",
                candidate.TipoActividad, candidate.Id);

            return AvailabilityResult.Ok();
        }

        // Validate a collection of candidates using the engine. Returns a map candidate -> result.
        // For each candidate a per-candidate context is created by copying typed prefetched data and
        // filtering ExistingActivities to those that overlap with the candidate timeslots.
        public async Task<IDictionary<Domain.ActividadMuseo.Entities.ActividadMuseo, AvailabilityResult>> CheckManyAsync(AvailabilityContext baseCtx, IEnumerable<Domain.ActividadMuseo.Entities.ActividadMuseo> candidates)
        {
            var results = new Dictionary<Domain.ActividadMuseo.Entities.ActividadMuseo, AvailabilityResult>();

            if (candidates == null)
                return results;

            // Materialize candidates to avoid multiple enumeration
            var list = candidates.ToList();

            foreach (var candidate in list)
            {
                // Build a per-candidate context reusing prefetched data from baseCtx
                var candidateStart = candidate.TimeSlots.Min(ts => ts.Inicio);
                var candidateEnd = candidate.TimeSlots.Max(ts => ts.Fin);

                var perCtx = new AvailabilityContext
                {
                    Start = candidateStart,
                    End = candidateEnd,
                    Metadata = baseCtx.Metadata,
                    // Filter ExistingActivities to those that overlap the candidate time slot
                    ExistingActivities = baseCtx.ExistingActivities?
                        .Where(a => a.TimeSlots.Any(ts => ts.SeSolapaCon(new Domain.Common.ValueObjets.TimeSlot(candidateStart, candidateEnd))))
                        .ToList() ?? new List<Domain.ActividadMuseo.Entities.ActividadMuseo>()
                };

                var res = await CheckAsync(perCtx, candidate);
                results[candidate] = res;
            }

            return results;
        }

        // Overload that accepts CandidateEntry list and returns results keyed by the candidate Id.
        public async Task<IDictionary<Guid, AvailabilityResult>> CheckManyAsync(AvailabilityContext baseCtx, IEnumerable<Application.Availability.Models.CandidateEntry> entries)
        {
            var results = new Dictionary<Guid, AvailabilityResult>();
            if (entries == null)
                return results;

            // Materialize entries to avoid multiple enumeration
            var list = entries.ToList();

            foreach (var entry in list)
            {
                var candidate = entry.Candidate;

                var candidateStart = candidate.TimeSlots.Min(ts => ts.Inicio);
                var candidateEnd = candidate.TimeSlots.Max(ts => ts.Fin);

                var perCtx = new AvailabilityContext
                {
                    Start = candidateStart,
                    End = candidateEnd,
                    Metadata = baseCtx.Metadata,
                    ExistingActivities = baseCtx.ExistingActivities?
                        .Where(a => a.TimeSlots.Any(ts => ts.SeSolapaCon(new Domain.Common.ValueObjets.TimeSlot(candidateStart, candidateEnd))))
                        .ToList() ?? new List<Domain.ActividadMuseo.Entities.ActividadMuseo>()
                };

                var res = await CheckAsync(perCtx, candidate);
                results[entry.Id] = res;
            }

            return results;
        }
    }
}
