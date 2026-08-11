using Application.Availability.Producers;
using Application.MuseumResources.Repositories;
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.UseCases.Queries.ConsultarDisponibilidadTurnosVisitaGuiada;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.ConsultarDisponibilidadTurnosVisitaAutoguiada
{
    internal sealed class DisponibilidadTurnosVisitasAutoguiadasHandler(
        GroupAvailabilityProducerService groupAvailabilityProducer,
        AutoMapper.IMapper mapper
       ): IRequestQueryHandler<DisponibilidadTurnosVisitasAutoguiadasQuery,QueryResult<SlotDisponibleDto>>
    {
        private readonly GroupAvailabilityProducerService _groupAvailabilityProducer =
            groupAvailabilityProducer ?? throw new ArgumentNullException(nameof(groupAvailabilityProducer));

        private readonly AutoMapper.IMapper _mapper =
            mapper ?? throw new ArgumentNullException(nameof(mapper));
    
        public async Task<QueryResult<SlotDisponibleDto>> Handle(DisponibilidadTurnosVisitasAutoguiadasQuery request, CancellationToken cancellationToken)
        {
            var turnosDisponibles = await groupAvailabilityProducer.
                GetHourlyBlocksAsync(
                    request.FechaDesde,
                    request.FechaHasta,
                    request.TematicasIds);
            return new QueryResult<SlotDisponibleDto>(
               turnosDisponibles.To<SlotDisponibleDto>(),
               turnosDisponibles.Count,
               request.PageIndex,
               request.PageSize);
        }
    }
}
