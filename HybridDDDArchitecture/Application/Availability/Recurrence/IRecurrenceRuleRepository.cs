using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Availability.Recurrence
{
    // Repository interface to fetch recurrence rules applicable to a window
    public interface IRecurrenceRuleRepository
    {
        Task<List<RecurrenceRule>> GetRulesAsync(DateTime windowStart, DateTime windowEnd, bool appliesToGuided);
    }
}
