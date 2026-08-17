using Application.Availability.Producers;
using Application.Reportes.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.Reportes.DomainServices;

namespace Application.Reportes.UseCases.Queries.ReporteVisitaGuiada
{
    internal sealed class ReporteVisitaGuiadaHandler(
        IRepositorioVisitaGuiada repositorioVisitaGuiada,
        GuidedAvailabilityProducerService guidedAvailabilityProducer


        ) : IRequestQueryHandler<ReporteVisitaGuiadaQuery, ReporteVisitaGuiadaDto>
    {
        private readonly IRepositorioVisitaGuiada 
            _repositorioVisitaGuiada= 
            repositorioVisitaGuiada 
            ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        private readonly GuidedAvailabilityProducerService 
            _guidedAvailabilityProducer= 
            guidedAvailabilityProducer 
            ?? throw new ArgumentNullException(nameof(guidedAvailabilityProducer));
        public async Task<ReporteVisitaGuiadaDto> Handle(ReporteVisitaGuiadaQuery request, CancellationToken cancellationToken)
        {
            var visitasguiadas =await _repositorioVisitaGuiada.GetGuidedToursByMonth(request.MesReporte);
            var TurnosDisponibles = await _guidedAvailabilityProducer.GetAvailableTurnsAsync(
                request.MesReporte.ToDateTime(new TimeOnly(0, 0)),
                request.MesReporte.AddMonths(1).ToDateTime(new TimeOnly(0, 0))
                );
            int totalCapacidadDisponible = TurnosDisponibles.Sum(t => t.CapacidadMaxima);
            var reporte = ServicioReporteVisitasGuiadas.ReporteVisitasGuiadas(
                visitasguiadas,
                totalCapacidadDisponible
                );
            return reporte.To<ReporteVisitaGuiadaDto>();
        }
    }
}

/*using Application.Reportes.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.Reportes.DomainServices;

namespace Application.Reportes.UseCases.Queries.ReporteVisitaGrupal
{
    internal sealed class ReporteVisitasGrupalesHandler(
        IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada, 
        IRepositorioVisitaGuiada repositorioVisitaGuiada
        ) : IRequestQueryHandler<ReporteGeneralVisitaGrupalQuery, ReporteVisitasGrupalesDto>
    {
        private readonly IRepositorioVisitaGrupalAutoguiada 
            _repositorioVisitaGrupalAutoguiada= 
            repositorioVisitaGrupalAutoguiada 
            ?? throw new ArgumentNullException(nameof(repositorioVisitaGrupalAutoguiada));

        private readonly IRepositorioVisitaGuiada 
            _repositorioVisitaGuiada= 
            repositorioVisitaGuiada 
            ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));

        public async Task<ReporteVisitasGrupalesDto> Handle(ReporteGeneralVisitaGrupalQuery request, CancellationToken cancellationToken)
        {
            var visitasguiadas = await _repositorioVisitaGuiada.GetGuidedToursByMonth(request.MesReporte);
            var visitasAutoguiadas = await _repositorioVisitaGrupalAutoguiada.GetGuidedToursByMonth(request.MesReporte);
            var reporte = ServicioReporteVisitasGrupales.GenerarReporteGeneralVisitasGrupales(
                visitasAutoguiadas,
                visitasguiadas
                );
            return reporte.To<ReporteVisitasGrupalesDto>();
        }
    }
}


*/