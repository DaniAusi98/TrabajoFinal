using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Reportes.DataTransferObjets
{
    public class ReporteVisitaAutoguiadaDto
    {
        public int Id { get; set; }
        public int ReservasTotales { get; private set; }
        public int VisitanteTotales { get; private set; }
        public int VisitasConfirmadas { get; private set; }
        public int VisitasCanceladas { get; private set; }
        public int Reprogramadas { get; private set; }
        public int Pendientes { get; private set; }
        public decimal TasaOcupacion { get; private set; }
    }
}
