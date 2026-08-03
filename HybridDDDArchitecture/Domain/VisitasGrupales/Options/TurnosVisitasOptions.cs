using System;

namespace Domain.VisitasGrupales.Options
{
    public class TurnosVisitasOptions
    {
        // Número mínimo de guías para considerar capacidad máxima por turno
        public int MinGuiasParaCapacidadCompleta { get; set; } = 2;

        // Capacidad de personas por cada guía (cuando sólo hay una guía en el turno)
        public int CapacidadPorGuia { get; set; } = 25;

        // Capacidad máxima por turno (cuando hay >= MinGuiasParaCapacidadCompleta guías)
        public int CapacidadMaximaPorTurno { get; set; } = 50;
    }
}
