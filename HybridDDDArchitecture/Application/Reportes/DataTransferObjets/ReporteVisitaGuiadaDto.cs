namespace Application.Reportes.DataTransferObjets
{
    public class ReporteVisitaGuiadaDto
    {
        public string Id { get; set; }
        public int ReservasTotales { get; private set; }
        public int VisitanteTotales { get; private set; }
        public int VisitasConfirmadas { get; private set; }
        public int VisitasCanceladas { get; private set; }
        public int Reprogramadas { get; private set; }
        public int Pendientes { get; private set; }
        public decimal TasaOcupacion { get; private set; }
    }
}
