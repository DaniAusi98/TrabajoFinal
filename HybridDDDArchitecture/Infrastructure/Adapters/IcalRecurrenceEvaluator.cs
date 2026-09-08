using Domain.Common.ValueObjets; // Aquí vive tu TimeSlot corporativo
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Application.Availability.ApplicationServices;

// Nota el alias para evitar conflictos si en tu proyecto se cruza con otros tipos de datos
using IcalDuration = Ical.Net.DataTypes.Duration;

namespace Infrastructure.Adapters
{
    public class IcalRecurrenceEvaluator : IRecurrenceEvaluator
    {
        public List<TimeSlot> ExpandRule(
            string rrule,
            DateTime eventStart,
            int durationMinutes,
            DateTime windowStart,
            DateTime windowEnd)
        {
            var slots = new List<TimeSlot>();
            var pattern = new RecurrencePattern(rrule);
            var iCalStart = new CalDateTime(eventStart);

            // SOLUCIÓN: Usamos IcalDuration.FromMinutes en vez de TimeSpan
            var virtualEvent = new CalendarEvent
            {
                Start = iCalStart,
                Duration = IcalDuration.FromMinutes(durationMinutes),
                RecurrenceRule = pattern
            };

            var iCalWindowEnd = new CalDateTime(windowEnd);
            IEnumerable<Occurrence> occurrences = virtualEvent
                .GetOccurrences()
                .TakeWhileBefore(iCalWindowEnd);

            foreach (var occurrence in occurrences)
            {
                DateTime slotStart = occurrence.Period.StartTime.Value;

                // Filtrado manual para ignorar las que queden antes de la ventana inicial
                if (slotStart < windowStart)
                {
                    continue;
                }

                DateTime slotEnd = slotStart.AddMinutes(durationMinutes);

                // Instanciamos tu objeto de negocio
                slots.Add(new TimeSlot(slotStart, slotEnd));
            }

            return slots;
        }

        public bool ExceedsLimit(string rrule, DateTime date)
        {
            var pattern = new RecurrencePattern(rrule);

            // En Ical.Net v5, si no hay límite UNTIL, la propiedad es simplemente null
            if (pattern.Until == null)
            {
                return false;
            }

            // pattern.Until.Value extrae el DateTime interno para poder usar .Date de forma segura
            return date.Date > pattern.Until.Value.Date;
        }

    }
}
