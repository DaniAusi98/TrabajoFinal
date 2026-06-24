using static Domain.Enums.VisitasGrupalesEnums.Enums;

namespace Domain.ValueObjets.VisitaGrupalMuseo
{
    public record TurnoDisponible(TimeSlot horario, int capacidadMax, int capacidadDispo, EstadoTurno estadoTurno)
    {
        public TimeSlot HorarioTurno { get;} = horario;
        public int CapacidadMaxima { get;  } = capacidadMax;
        public int CapacidadDisponible { get; } = capacidadDispo;
        public EstadoTurno EstadoTurno { get; } = estadoTurno;

    }
}
