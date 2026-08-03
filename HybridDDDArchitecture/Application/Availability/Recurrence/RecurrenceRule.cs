using System;
using System.Collections.Generic;

namespace Application.Availability.Recurrence
{
    public enum Frequency
    {
        Daily,
        Weekly,
        Monthly,
        DateRange
    }

    public class RecurrenceRule
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }

        public TimeSpan? TimeFrom { get; init; }
        public TimeSpan? TimeTo { get; init; }

        public Frequency Frequency { get; init; } = Frequency.Weekly;
        public int Interval { get; init; } = 1; // every n units

        // Weekly
        public DayOfWeek[]? ByDays { get; init; }

        // Monthly
        public int[]? ByMonthDays { get; init; }
        public int[]? ByMonths { get; init; }
        public int? WeekOfMonth { get; init; }

        // Exceptions
        public List<DateTime>? ExDates { get; init; }

        // Scope
        public bool AppliesToGuidedVisits { get; init; }
        public bool AppliesToGroupVisits { get; init; }

        public string? Note { get; init; }
    }
}
