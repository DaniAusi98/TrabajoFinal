using Application.Repositories;
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;

using Core.Application;

using Domain.ActividadMuseo.Entities;
using Domain.VisitasGrupales.DomainServices;

namespace Application.VisitaGrupal.UseCases.Queries.ConsultarDisponibilidadTurnosVisitaGuiada
{
    internal class ConsultarDisponibilidadTurnosDiaHandler(
        IRepositorioGuia repositorioGuia,
        IRepositorioVisitaGuiada repositorioVisitaGuiada,
        IRepositorioDiaCierreMuseo repositorioDiaCierreMuseo,
        ICalendarioMuseo calendario,
        AutoMapper.IMapper mapper)
                : IRequestQueryHandler<
            ConsultarDisponibilidadTurnosDiaQuery,
            QueryResult<TurnoDisponibleDto>>
    {
        private readonly IRepositorioGuia _repositorioGuia = repositorioGuia ?? throw new ArgumentNullException(nameof(repositorioGuia));
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        private readonly IRepositorioDiaCierreMuseo _repositorioDiaCierreMuseo = repositorioDiaCierreMuseo?? throw new ArgumentNullException(nameof(repositorioDiaCierreMuseo));
        private readonly ICalendarioMuseo _calendario = calendario?? throw new ArgumentNullException(nameof(calendario));
        private readonly AutoMapper.IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<QueryResult<TurnoDisponibleDto>> Handle(
            ConsultarDisponibilidadTurnosDiaQuery request,
            CancellationToken cancellationToken)
        {
            var guias = await _repositorioGuia
                .ObtenerGuiasConDisponibilidadAsync();

         

            var visitas = await _repositorioVisitaGuiada.GetAllGroupVisitAsync(
                    request.FechaDesde,
                    request.FechaHasta
            );


            var diasCierre = await _repositorioDiaCierreMuseo
                .FindAllAsync();

           

            var turnosDisponibles =
                ServicioDisponibilidadTurnosVisitasGuiadas.DisponiblidadTurnosVisitasGuiadas(
                    request.FechaDesde,
                    request.FechaHasta,
                    guias,
                    visitas,
                    diasCierre,
                    _calendario
                );

            return new QueryResult<TurnoDisponibleDto>(turnosDisponibles.To<TurnoDisponibleDto>(), turnosDisponibles.Count, request.PageIndex, request.PageSize);



        }
    }
}
