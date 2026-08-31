using Application.Availability.Models;
using Application.Availability.Recurrence;
using Domain.Common.ValueObjets;

namespace Application.Availability.Factories
{
    public static class ActivityAvailabilityFactory
    {
        public static IReadOnlyCollection<ActivityAvailabilityEntry> Create(
            IEnumerable<Domain.ActividadMuseo.Entities.ActividadMuseo> actividades,
            DateOnly windowStart,
            DateOnly windowEnd)
        {
            var result = new List<ActivityAvailabilityEntry>();

            foreach (var actividad in actividades)
            {
                IReadOnlyCollection<TimeSlot> timeSlots;

                if (actividad.Recurrence == null)
                {
                    timeSlots = new List<TimeSlot>
                    {
                        actividad.Horario
                    };
                }
                else
                {
                    timeSlots = RecurrenceExpander
                        .Expand(
                            actividad.Recurrence,
                            actividad.Horario,
                            actividad.Exceptions,
                            windowStart,
                            windowEnd)
                        .ToList();
                }

                result.Add(
                    new ActivityAvailabilityEntry(
                        actividad,
                        timeSlots));
            }

            return result;
        }
    }
}