using Application.Reportes.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.Reportes.DomainServices;

namespace Application.Reportes.UseCases.Queries.ReporteVisitaGrupal
{
    internal sealed class ReporteVisitasGrupalesHandler(
        IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada, 
        IRepositorioVisitaGuiada repositorioVisitaGuiada
        ) : IRequestQueryHandler<ReporteGeneralVisitaGrupalQuery, ReporteGridDto>
    {
        private readonly IRepositorioVisitaGrupalAutoguiada 
            _repositorioVisitaGrupalAutoguiada= 
            repositorioVisitaGrupalAutoguiada 
            ?? throw new ArgumentNullException(nameof(repositorioVisitaGrupalAutoguiada));

        private readonly IRepositorioVisitaGuiada 
            _repositorioVisitaGuiada= 
            repositorioVisitaGuiada 
            ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));

        public async Task<ReporteGridDto> Handle(ReporteGeneralVisitaGrupalQuery request, CancellationToken cancellationToken)
        {
            var visitasguiadas = await _repositorioVisitaGuiada.GetGuidedToursByMonth(request.MesReporte);
            var visitasAutoguiadas = await _repositorioVisitaGrupalAutoguiada.GetSelfGuidedToursByMonth(request.MesReporte);
            var reporte = ServicioReporteVisitasGrupales.GenerarReporteGeneralVisitasGrupales(
                visitasAutoguiadas,
                visitasguiadas
                );
            return reporte.To<ReporteGridDto>();
        }
    }
}


