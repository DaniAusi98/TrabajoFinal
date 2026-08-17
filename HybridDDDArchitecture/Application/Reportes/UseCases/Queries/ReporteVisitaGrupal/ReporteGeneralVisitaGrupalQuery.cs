using Application.Reportes.DataTransferObjets;
using Core.Application;
using System.ComponentModel.DataAnnotations;


namespace Application.Reportes.UseCases.Queries.ReporteVisitaGrupal
{
    public class ReporteGeneralVisitaGrupalQuery:IRequestQuery<ReporteGridDto>
    {
        [Required]
        public DateOnly MesReporte { get; set; }

        public ReporteGeneralVisitaGrupalQuery()
        {

        }
            
    }
}
