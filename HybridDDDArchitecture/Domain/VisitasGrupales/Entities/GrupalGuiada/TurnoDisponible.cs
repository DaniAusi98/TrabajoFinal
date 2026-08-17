using Domain.Common.ValueObjets;
using static Domain.VisitasGrupales.Enums.Enums;

namespace Domain.VisitasGrupales.Entities.GrupalGuiada
{
    public record TurnoDisponible(TimeSlot horario, int capacidadMax, EstadoTurno estadoTurno)
    {
        public TimeSlot HorarioTurno { get;} = horario;
        public int CapacidadMaxima { get;  } = capacidadMax;
        public EstadoTurno EstadoTurno { get; } = estadoTurno;

    }
}
