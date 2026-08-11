using Application.VisitaGrupal.DataTransferObjets;
using Core.Application;
using Domain.VisitasGrupales.Entities;


namespace Application.VisitaGrupal.UseCases.Queries.ConsultarDisponibilidadTurnosVisitaAutoguiada
{
    public class DisponibilidadTurnosVisitasAutoguiadasQuery(DateTime fechaDesde, DateTime fechaHasta,List<int> tematicasIds) : QueryRequest<QueryResult<SlotDisponibleDto>>
    {
        public DateTime FechaDesde { get; } = fechaDesde;
        public DateTime FechaHasta { get; } = fechaHasta;
        public List<int> TematicasIds { get; } = tematicasIds;


    }
}
