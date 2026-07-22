using Domain.Common.ValueObjets;

using static Domain.VisitasGrupales.Enums.Enums;

namespace Domain.VisitasGrupales.Entities
{
    public record TurnoDisponible(TimeSlot horario, int capacidadMax, int capacidadDispo, EstadoTurno estadoTurno)
    {
        public TimeSlot HorarioTurno { get;} = horario;
        public int CapacidadMaxima { get;  } = capacidadMax;
        public int CapacidadDisponible { get; } = capacidadDispo;
        public EstadoTurno EstadoTurno { get; } = estadoTurno;

    }
}
