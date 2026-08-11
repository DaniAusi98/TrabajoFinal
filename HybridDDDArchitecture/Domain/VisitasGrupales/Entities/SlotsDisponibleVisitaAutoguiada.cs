using Domain.Common.ValueObjets;
using static Domain.VisitasGrupales.Enums.Enums;

namespace Domain.VisitasGrupales.Entities
{
    public record SlotDisponibleVisitaAutoguiada(TimeSlot horario, int capacidadMax, int cuposDisponibles, EstadoTurno estadoSlot)
    {
        public TimeSlot HorarioSlot { get; } = horario;
        public int CapacidadMaxima { get; } = capacidadMax;
        public int CuposDisponibles { get; } = cuposDisponibles;
        public EstadoTurno EstadoSlot { get; } = estadoSlot;
    }
}