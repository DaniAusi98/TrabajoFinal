using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Availability.Recurrence
{
    /// <summary>
    /// Simple in-memory recurrence rule repository for testing/demo purposes.
    /// Add rules to the _rules list in the constructor.
    /// </summary>
    public class InMemoryRecurrenceRuleRepository : IRecurrenceRuleRepository
    {
        private readonly List<RecurrenceRule> _rules = new();

        public InMemoryRecurrenceRuleRepository()
        {
            // Example rule: block every Tuesday and Wednesday between 14:00-18:00
            _rules.Add(new RecurrenceRule
            {
                StartDate = DateTime.Today.AddMonths(-1),
                EndDate = DateTime.Today.AddMonths(6),
                Frequency = Frequency.Weekly,
                ByDays = new[] { DayOfWeek.Tuesday, DayOfWeek.Wednesday },
                TimeFrom = new TimeSpan(14, 0, 0),
                TimeTo = new TimeSpan(18, 0, 0),
                AppliesToGuidedVisits = true,
                Note = "Blocked Tue/Wed afternoons for guided visits (demo)"
            });

            // Example rule: block first week of each month, whole day
            _rules.Add(new RecurrenceRule
            {
                StartDate = DateTime.Today.AddMonths(-1),
                EndDate = DateTime.Today.AddMonths(12),
                Frequency = Frequency.Monthly,
                ByMonthDays = new[] { 1, 2, 3, 4, 5, 6, 7 },
                TimeFrom = new TimeSpan(0, 0, 0),
                TimeTo = new TimeSpan(23, 59, 59),
                AppliesToGuidedVisits = true,
                Note = "First week of month blocked (demo)"
            });
        }

        public Task<List<RecurrenceRule>> GetRulesAsync(DateTime windowStart, DateTime windowEnd, bool appliesToGuided)
        {
            var result = _rules
                .Where(r => r.AppliesToGuidedVisits == appliesToGuided)
                .Where(r => (r.EndDate ?? DateTime.MaxValue) >= windowStart && r.StartDate <= windowEnd)
                .ToList();

            return Task.FromResult(result);
        }
    }
}
