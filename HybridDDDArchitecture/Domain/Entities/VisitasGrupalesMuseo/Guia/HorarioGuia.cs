using Domain.Exceptions;
using Core.Domain.Entities;
using Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities.VisitasGrupalesMuseo.Guia
{
    public class HorarioGuia : DomainEntity
    {
        // Navigation back to Guia
        public int GuiaId { get; private set; }
        public Guia Guia { get; private set; } = default!;

        // Días de la semana en los que aplica este horario
        public List<DayOfWeek> DiasLaborales { get; private set; } = new();
        public TimeOnly HoraInicio { get; private set; }
        public TimeOnly HoraFin { get; private set; }

        public HorarioGuia()
        {
            // EF
        }

        public HorarioGuia(
            int guiaId,
            IEnumerable<DayOfWeek> dias,
            TimeOnly inicio,
            TimeOnly fin)
        {
            if (dias == null || !dias.Any())
                throw new DomainException("Debe indicar al menos un día.");

            if (dias.Any(d => d is DayOfWeek.Saturday or DayOfWeek.Sunday))
            {
                throw new DomainException(
                    "Los guías no pueden trabajar fines de semana.");
            }

            var diasList = dias.ToList();
            if (diasList.Count != diasList.Distinct().Count())
            {
                throw new DomainException(
                    "Hay días laborales repetidos.");
            }

            if (fin <= inicio)
            {
                throw new DomainException(
                    "La hora de fin debe ser posterior a la hora de inicio.");
            }

            GuiaId = guiaId;
            DiasLaborales = diasList;
            HoraInicio = inicio;
            HoraFin = fin;
        }
    }
}
