using Application.Reportes.DataTransferObjets;
using Core.Application;
using System.ComponentModel.DataAnnotations;


namespace Application.Reportes.UseCases.Queries.ReporteVisitaGuiada
{
    public class ReporteVisitaGuiadaQuery : IRequestQuery<ReporteVisitaGuiadaDto>
    {
        [Required]
        public DateOnly MesReporte { get; set; }

        public ReporteVisitaGuiadaQuery()
        {
                
        }
    }
}
