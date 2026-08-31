using Domain.Common.Exceptions;

namespace Domain.ActividadMuseo.Entities
{
    public enum Frequency
    {
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    public class RecurrenceRule
    {
        public DateOnly StartDate { get; private set; }

        public DateOnly? EndDate { get; private set; }

        public Frequency Frequency { get; private set; }

        public int Interval { get; private set; }

        // Weekly
        public DayOfWeek[]? ByDays { get; private set; }

        // Monthly
        public int? MonthDay { get; private set; }

        public int? WeekOfMonth { get; private set; }

        public bool LastWeekOfMonth { get; private set; }

        private RecurrenceRule() { }

        public RecurrenceRule(
            DateOnly startDate,
            Frequency frequency,
            int interval = 1,
            DateOnly? endDate = null,
            DayOfWeek[]? byDays = null,
            int? monthDay = null,
            int? weekOfMonth = null,
            bool lastWeekOfMonth = false)
        {
            if (interval <= 0)
                throw new DomainException(
                    "El intervalo de recurrencia debe ser mayor a cero.");

            if (endDate.HasValue && endDate.Value < startDate)
                throw new DomainException(
                    "La fecha de finalizacion no puede ser anterior a la fecha de inicio.");

            StartDate = startDate;
            Frequency = frequency;
            Interval = interval;
            EndDate = endDate;

            ByDays = byDays;
            MonthDay = monthDay;
            WeekOfMonth = weekOfMonth;
            LastWeekOfMonth = lastWeekOfMonth;

            Validar();
        }

        private void Validar()
        {
            if (Frequency == Frequency.Weekly)
            {
                if (ByDays == null || ByDays.Length == 0)
                    throw new DomainException(
                        "Una recurrencia semanal debe tener al menos un dia.");
            }

            if (Frequency == Frequency.Monthly)
            {
                var opciones = 0;

                if (MonthDay.HasValue)
                    opciones++;

                if (WeekOfMonth.HasValue)
                    opciones++;

                if (LastWeekOfMonth)
                    opciones++;

                if (opciones != 1)
                    throw new DomainException(
                        "Una recurrencia mensual debe tener una sola opcion.");

                if (MonthDay.HasValue &&
                    (MonthDay.Value < 1 || MonthDay.Value > 31))
                    throw new DomainException(
                        "El dia del mes debe estar entre 1 y 31.");

                if (WeekOfMonth.HasValue &&
                    (WeekOfMonth.Value < 1 || WeekOfMonth.Value > 4))
                    throw new DomainException(
                        "La semana del mes debe estar entre 1 y 4.");
            }

            if (Frequency != Frequency.Weekly && ByDays != null)
                throw new DomainException(
                    "Los dias de la semana solo pueden utilizarse en recurrencias semanales.");

            if (Frequency != Frequency.Monthly)
            {
                if (MonthDay.HasValue ||
                    WeekOfMonth.HasValue ||
                    LastWeekOfMonth)
                    throw new DomainException(
                        "Las opciones mensuales solo pueden utilizarse en recurrencias mensuales.");
            }
        }

        public void CambiarFechaFin(DateOnly? endDate)
        {
            if (endDate.HasValue && endDate.Value < StartDate)
                throw new DomainException(
                    "La fecha de finalizacion no puede ser anterior a la fecha de inicio.");

            EndDate = endDate;
        }

        public void CambiarIntervalo(int interval)
        {
            if (interval <= 0)
                throw new DomainException(
                    "El intervalo debe ser mayor a cero.");

            Interval = interval;
        }
    }
}