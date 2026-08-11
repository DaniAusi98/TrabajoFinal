using Application.ApplicationMuseo.DataTransferObjects;

namespace Application.VisitaGrupal.DataTransferObjets
{
    public class SlotDisponibleDto
    {
        public TimeSlotDto HorarioSlot { get; set; } = default!;
        public int CapacidadMaxima { get; set; }
        public int CuposDisponibles { get; set; }
        public string EstadoSlot { get; set; } = default!;
    }
}