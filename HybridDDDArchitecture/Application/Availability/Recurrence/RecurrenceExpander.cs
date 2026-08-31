using Domain.ActividadMuseo.Entities;
using Domain.Common.ValueObjets;

namespace Application.Availability.Recurrence
{
    public static class RecurrenceExpander
    {
        public static IEnumerable<TimeSlot> Expand(
            RecurrenceRule recurrence,
            TimeSlot horario,
            IReadOnlyCollection<ActividadException> exceptions,
            DateOnly windowStart,
            DateOnly windowEnd)
        {
            if (recurrence == null)
                yield break;

            if (windowStart > windowEnd)
                yield break;

            var start = recurrence.StartDate > windowStart
                ? recurrence.StartDate
                : windowStart;

            var end = recurrence.EndDate.HasValue &&
                      recurrence.EndDate.Value < windowEnd
                ? recurrence.EndDate.Value
                : windowEnd;

            switch (recurrence.Frequency)
            {
                case Frequency.Daily:

                    for (var fecha = start;
                         fecha <= end;
                         fecha = fecha.AddDays(recurrence.Interval))
                    {
                        if (EsExcepcion(fecha, exceptions))
                            continue;

                        yield return CrearTimeSlot(fecha, horario);
                    }

                    break;

                case Frequency.Weekly:

                    for (var fecha = start;
                         fecha <= end;
                         fecha = fecha.AddDays(1))
                    {
                        if (recurrence.ByDays == null ||
                            !recurrence.ByDays.Contains(fecha.DayOfWeek))
                            continue;

                        var semanas =
                            (fecha.DayNumber - recurrence.StartDate.DayNumber) / 7;

                        if (semanas % recurrence.Interval != 0)
                            continue;

                        if (EsExcepcion(fecha, exceptions))
                            continue;

                        yield return CrearTimeSlot(fecha, horario);
                    }

                    break;

                case Frequency.Monthly:

                    for (var fecha = start;
                         fecha <= end;
                         fecha = fecha.AddDays(1))
                    {
                        bool cumple = false;

                        // Cada mes el dia X
                        if (recurrence.MonthDay.HasValue)
                        {
                            if (fecha.Day == recurrence.MonthDay.Value)
                                cumple = true;
                        }

                        // Cada mes el N dia de semana
                        if (recurrence.WeekOfMonth.HasValue)
                        {
                            var semana = ((fecha.Day - 1) / 7) + 1;

                            if (semana <= 4 &&
                                fecha.DayOfWeek == horario.Inicio.DayOfWeek &&
                                semana == recurrence.WeekOfMonth.Value)
                            {
                                cumple = true;
                            }
                        }

                        // Cada mes el ultimo dia de la semana
                        if (recurrence.LastWeekOfMonth)
                        {
                            if (fecha.DayOfWeek == horario.Inicio.DayOfWeek &&
                                fecha.AddDays(7).Month != fecha.Month)
                            {
                                cumple = true;
                            }
                        }

                        if (!cumple)
                            continue;

                        if (EsExcepcion(fecha, exceptions))
                            continue;

                        yield return CrearTimeSlot(fecha, horario);
                    }

                    break;

                case Frequency.Yearly:

                    var fechaAnual = recurrence.StartDate;

                    while (fechaAnual <= end)
                    {
                        if (fechaAnual >= start)
                        {
                            if (!(recurrence.StartDate.Month == 2 &&
                                  recurrence.StartDate.Day == 29 &&
                                  !DateTime.IsLeapYear(fechaAnual.Year)))
                            {
                                if (!EsExcepcion(fechaAnual, exceptions))
                                {
                                    yield return CrearTimeSlot(
                                        fechaAnual,
                                        horario);
                                }
                            }
                        }

                        fechaAnual = fechaAnual.AddYears(recurrence.Interval);
                    }

                    break;
            }
        }

        private static bool EsExcepcion(
            DateOnly fecha,
            IReadOnlyCollection<ActividadException> exceptions)
        {
            return exceptions.Any(x => x.Date == fecha);
        }

        private static TimeSlot CrearTimeSlot(
            DateOnly fecha,
            TimeSlot horario)
        {
            var horaInicio =
                TimeOnly.FromDateTime(horario.Inicio);

            var horaFin =
                TimeOnly.FromDateTime(horario.Fin);

            var inicio =
                fecha.ToDateTime(horaInicio);

            var fin =
                fecha.ToDateTime(horaFin);

            return new TimeSlot(inicio, fin);
        }
    }
}


/*
# Recurrence monthly - Frontend logic

## 1. Selected date

The user selects a date from the calendar.

Example:

28 / 01 / 2027 - Thursday

The selected date is the reference for building the recurrence options.

The frontend should obtain:

day of month      -> 28
day of week       -> Thursday
position          -> 4

---

## 2. Option "Every month on day X"

This option is always available.

Example:

28 / 01 / 2027
->Every month on the 28th

Send to backend:

{
    "frequency": "Monthly",
  "monthDay": 28
}

If the day does not exist in a month, simply skip that month.

Example with 31:

January-> 31
February->skip
March-> 31
April->skip

-- -

## 3. Option "Every month on the Nth weekday"

First calculate which week of the month contains the selected date:

const weekOfMonth = Math.floor((day - 1) / 7) + 1;

Only offer this option if:

weekOfMonth <= 4

Example:

18 / 12 / 2026 - Friday

If it is the third Friday:

Every month on the 18th
Every month on the third Friday

Send:

{
    "frequency": "Monthly",
  "weekOfMonth": 3
}

The backend gets the DayOfWeek from the original TimeSlot.

IMPORTANT:

If the selected date is in week 5:

29 / 01 / 2027 - Friday

DO NOT offer:

Every month on the fifth Friday

Our recurrence rule only allows positions 1 to 4.

---

## 4. Option "Every month on the last weekday"

This option is available when the selected date is the last occurrence of that weekday in the month.

Check it with:

const nextWeek = new Date(fecha);
nextWeek.setDate(nextWeek.getDate() + 7);

const isLastWeekOfMonth =
    nextWeek.getMonth() !== fecha.getMonth();

If true, show:

Every month on the last Friday
Every month on the last Saturday
Every month on the last Sunday

according to the DayOfWeek of the selected date.

Send:

{
    "frequency": "Monthly",
  "lastWeekOfMonth": true
}

The backend uses the weekday of the original TimeSlot to determine whether it should search for:

last Monday
last Tuesday
last Wednesday
last Thursday
last Friday
last Saturday
last Sunday

---

# Examples

## 31/01/2027 - Sunday

Available:

Every month on the 31st
Every month on the last Sunday

Not available:

Every month on the fifth Sunday

---

## 28/01/2027 - Thursday

Available:

Every month on the 28th
Every month on the fourth Thursday
Every month on the last Thursday

if the 28th is the last Thursday of that month.

---

## 18/12/2026 - Friday

If it is the third Friday:

Every month on the 18th
Every month on the third Friday

If it is also the last Friday:

Every month on the last Friday

---

# Frontend data

The frontend does not send the weekday because it is already determined by the original TimeSlot.

It only sends the selected recurrence variant:

By day of month:

MonthDay = X

By position:

WeekOfMonth = 1..4

By last weekday:

LastWeekOfMonth = true

Conceptually:

Monthly
  ->MonthDay
  ->WeekOfMonth
  ->LastWeekOfMonth

This logic is mainly presentation logic for the frontend.

The RecurrenceExpander only receives the selected rule and generates the TimeSlots.
```
*/