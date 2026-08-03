using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common.ValueObjets;

namespace Application.Availability.Recurrence
{
    public static class RecurrenceExpander
    {
        public static IEnumerable<TimeSlot> Expand(RecurrenceRule rule, DateTime windowStart, DateTime windowEnd)
        {
            if (rule == null) yield break;

            var start = rule.StartDate.Date < windowStart.Date ? windowStart.Date : rule.StartDate.Date;
            var end = rule.EndDate.HasValue && rule.EndDate.Value.Date < windowEnd.Date ? rule.EndDate.Value.Date : windowEnd.Date;

            if (start > end) yield break;

            // Helper: check exdates
            var ex = rule.ExDates ?? new List<DateTime>();

            // For performance use different strategies per frequency
            switch (rule.Frequency)
            {
                case Frequency.DateRange:
                    for (var d = start; d <= end; d = d.AddDays(1))
                    {
                        if (ex.Any(x => x.Date == d.Date)) continue;
                        if (!rule.TimeFrom.HasValue || !rule.TimeTo.HasValue) continue;
                        var s = d.Date + rule.TimeFrom.Value;
                        var f = d.Date + rule.TimeTo.Value;
                        if (f <= s) continue;
                        if (s > windowEnd || f < windowStart) continue;
                        yield return new TimeSlot(s, f);
                    }
                    break;
                case Frequency.Daily:
                    for (var d = start; d <= end; d = d.AddDays(rule.Interval))
                    {
                        if (ex.Any(x => x.Date == d.Date)) continue;
                        if (!rule.TimeFrom.HasValue || !rule.TimeTo.HasValue) continue;
                        var s = d.Date + rule.TimeFrom.Value;
                        var f = d.Date + rule.TimeTo.Value;
                        if (f <= s) continue;
                        if (s > windowEnd || f < windowStart) continue;
                        yield return new TimeSlot(s, f);
                    }
                    break;
                case Frequency.Weekly:
                    // iterate daily but check ByDays and interval of weeks
                    for (var d = start; d <= end; d = d.AddDays(1))
                    {
                        if (ex.Any(x => x.Date == d.Date)) continue;
                        if (rule.ByDays != null && !rule.ByDays.Contains(d.DayOfWeek)) continue;

                        // check week interval: compute number of weeks since rule.StartDate
                        var weeks = (int)Math.Floor((d.Date - rule.StartDate.Date).TotalDays / 7.0);
                        if (weeks < 0) continue;
                        if (weeks % Math.Max(1, rule.Interval) != 0) continue;

                        if (!rule.TimeFrom.HasValue || !rule.TimeTo.HasValue) continue;
                        var s = d.Date + rule.TimeFrom.Value;
                        var f = d.Date + rule.TimeTo.Value;
                        if (f <= s) continue;
                        if (s > windowEnd || f < windowStart) continue;
                        yield return new TimeSlot(s, f);
                    }
                    break;
                case Frequency.Monthly:
                    for (var d = start; d <= end; d = d.AddDays(1))
                    {
                        if (ex.Any(x => x.Date == d.Date)) continue;
                        if (rule.ByMonths != null && rule.ByMonths.Length > 0 && !rule.ByMonths.Contains(d.Month)) continue;

                        var ok = false;
                        if (rule.ByMonthDays != null && rule.ByMonthDays.Length > 0)
                        {
                            if (rule.ByMonthDays.Contains(d.Day)) ok = true;
                        }

                        if (rule.WeekOfMonth.HasValue)
                        {
                            var week = ((d.Day - 1) / 7) + 1;
                            if (rule.WeekOfMonth.Value == week) ok = true;
                        }

                        if (!ok) continue;

                        if (!rule.TimeFrom.HasValue || !rule.TimeTo.HasValue) continue;
                        var s = d.Date + rule.TimeFrom.Value;
                        var f = d.Date + rule.TimeTo.Value;
                        if (f <= s) continue;
                        if (s > windowEnd || f < windowStart) continue;
                        yield return new TimeSlot(s, f);
                    }
                    break;
            }
        }
    }
}
