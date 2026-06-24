using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;

using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.GetTematicaVisitaGrupal
{
    internal class GetAllTematicasHandler(IRepositorioTematicas repositorioTematicas) :IRequestQueryHandler<GetAllTematicasQuery, QueryResult<TematicaVisitaGrupalDto>>
    {
        private readonly IRepositorioTematicas _repositorioTematicas = repositorioTematicas ?? throw new ArgumentNullException(nameof(repositorioTematicas));
        public async Task<QueryResult<TematicaVisitaGrupalDto>> Handle(GetAllTematicasQuery request, CancellationToken cancellationToken)
        {
            IList<Domain.Entities.VisitasGrupalesMuseo.TematicaVisita> entities = await _repositorioTematicas.ObtenerDisponiblesAsync();
            return new QueryResult<TematicaVisitaGrupalDto>(entities.To<TematicaVisitaGrupalDto>(), entities.Count, request.PageIndex, request.PageSize);
        }
    }
}
