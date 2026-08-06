namespace Application.VisitaGrupal.DataTransferObjets
{
    public class ConfiguracionVisitasGrupalesGuiadasDto
    {
        public int Id { get; set; }
        public int MinGuiasParaCapacidadCompleta { get; set; }
        public int CapacidadPorGuia { get; set; }
        public int CapacidadMaximaPorTurno { get; set; }
        public List<DayOfWeek> DiasDisponibles { get; set; } = new();
        public List<TurnoDto> Turnos { get; set; } = new();
    }

    public class TurnoDto
    {
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }
}
