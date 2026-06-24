using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.ActividadMuseo.DataTransferObjets;
using Application.ActividadMuseo.Repositories;

using Core.Application;

namespace Application.ActividadMuseo.UseCases.ActividadesMuseo.Queries
{
    internal sealed class GetAllActivitiesHandler(IRepositorioActividadMuseo repositorioActividad):IRequestQueryHandler<GetAllActivitiesQuery, QueryResult<ActividadMuseoDto>>
    {
        private readonly IRepositorioActividadMuseo _repositorioActividadMuseo = repositorioActividad ?? throw new ArgumentNullException(nameof(repositorioActividad));
        public async Task<QueryResult<ActividadMuseoDto>> Handle(GetAllActivitiesQuery request, CancellationToken cancellationToken)
        {
            IList<Domain.Entities.DisponibilidadMuseo.ActividadMuseo> entities = await _repositorioActividadMuseo.FindAllAsync(request.FechaConsultaDesde, request.FechaConsultaHasta);
            return new QueryResult<ActividadMuseoDto>(entities.To<ActividadMuseoDto>(), entities.Count, request.PageIndex, request.PageSize);
        }
    }
}
