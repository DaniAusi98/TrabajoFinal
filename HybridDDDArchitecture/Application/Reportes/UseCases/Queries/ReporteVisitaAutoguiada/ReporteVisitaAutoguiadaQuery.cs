using Application.Reportes.DataTransferObjets;
using Core.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.Reportes.UseCases.Queries.ReporteVisitanteSala
{
    public class ReporteVisitaAutoguiadaQuery : IRequestQuery<ReporteVisitaAutoguiadaDto>
    {
        [Required]
        public DateOnly MesReporte { get; set; }
        public ReporteVisitaAutoguiadaQuery()
        {

        }
    }
}
