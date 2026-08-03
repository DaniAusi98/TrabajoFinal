using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.GetGuiaBy
{
    public class GetGuiaByQuery : QueryRequest<Application.VisitaGrupal.DataTransferObjets.GuiaDto>
    {
        public int GuiaId { get; set; }
    }
}
