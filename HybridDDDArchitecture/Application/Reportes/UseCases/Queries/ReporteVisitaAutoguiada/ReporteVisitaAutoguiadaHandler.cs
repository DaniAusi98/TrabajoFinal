using Application.Availability.Producers;
using Application.Reportes.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.Reportes.DomainServices;

namespace Application.Reportes.UseCases.Queries.ReporteVisitanteSala
{
    internal sealed class ReporteVisitaAutoguiadaHandler(
        IRepositorioVisitaGrupalAutoguiada repositorioVisitaAutoguiada,
        SelfGuidedAvailabilityProducerService selfGuidedAvailabilityProducer
        ) : IRequestQueryHandler<ReporteVisitaAutoguiadaQuery, ReporteVisitaAutoguiadaDto>
    {
        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaAutoguiada = repositorioVisitaAutoguiada ?? throw new ArgumentNullException(nameof(repositorioVisitaAutoguiada));
        private readonly SelfGuidedAvailabilityProducerService _selfGuidedAvailabilityProducer = selfGuidedAvailabilityProducer ?? throw new ArgumentNullException(nameof(selfGuidedAvailabilityProducer));


        public async Task<ReporteVisitaAutoguiadaDto> Handle(ReporteVisitaAutoguiadaQuery request, CancellationToken cancellationToken)
        {
            var visitasAutoguiadas= await _repositorioVisitaAutoguiada.GetSelfGuidedToursByMonth(request.MesReporte);
            var SlotsDisponibles = await _selfGuidedAvailabilityProducer.GetHourlyBlocksAsync(
                request.MesReporte.ToDateTime(new TimeOnly(0, 0)),
                request.MesReporte.AddMonths(1).ToDateTime(new TimeOnly(0, 0))
                );
             int totalCapacidadDisponible = SlotsDisponibles.Sum(s => s.CapacidadMaximaPorGrupo);
            var reporte = ServicioReporteVisitasAutoguiadas.GenerarReporteVisitasAutoguiadas(
               visitasAutoguiadas,
               totalCapacidadDisponible
               );
            return reporte.To<ReporteVisitaAutoguiadaDto>();
        }
    }
}
/*internal sealed class ReporteVisitaGuiadaHandler(
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
}*/
