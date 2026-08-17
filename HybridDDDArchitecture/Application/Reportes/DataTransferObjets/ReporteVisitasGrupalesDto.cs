using System.Text.Json.Serialization;
using static Domain.ActividadMuseo.Enums.Enums;
using static Domain.VisitasGrupales.Enums.Enums;

namespace Application.Reportes.DataTransferObjets
{
    public class ReporteVisitasGrupalesDto
    {
        public int Id { get; set; }
        public int ReservasTotales { get; private set; }
        public int VisitanteTotales { get; private set; }
        public int VisitasConfirmadas { get; private set; }
        public int VisitasCanceladas { get; private set; }
        public int Reprogramadas { get; private set; }
        public int Pendientes { get; private set; }
    }
 

}
