using Application.Repositories;
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;
using Domain.DomainServices.VisitasGrupalesMuseo;
using Domain.Entities.DisponibilidadMuseo;


namespace Application.VisitaGrupal.UseCases.Queries.ConsultarDisponibilidadTurnosVisitaGuiada
{
    internal class ConsultarDisponibilidadTurnosDiaHandler
        : IRequestQueryHandler<
            ConsultarDisponibilidadTurnosDiaQuery,
            QueryResult<TurnoDisponibleDto>>
    {
        private readonly IRepositorioGuia _repositorioGuia;
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada;
        private readonly IRepositorioDiaCierreMuseo _repositorioDiaCierreMuseo;
        private readonly ICalendarioMuseo _calendario;
        private readonly AutoMapper.IMapper _mapper;

        public ConsultarDisponibilidadTurnosDiaHandler(
            IRepositorioGuia repositorioGuia,
            IRepositorioVisitaGuiada repositorioVisitaGuiada,
            IRepositorioDiaCierreMuseo repositorioDiaCierreMuseo,
            ICalendarioMuseo calendario,
            AutoMapper.IMapper mapper)
        {
            _repositorioGuia = repositorioGuia
                ?? throw new ArgumentNullException(nameof(repositorioGuia));

            _repositorioVisitaGuiada = repositorioVisitaGuiada
                ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));

            _repositorioDiaCierreMuseo = repositorioDiaCierreMuseo
                ?? throw new ArgumentNullException(nameof(repositorioDiaCierreMuseo));

            _calendario = calendario
                ?? throw new ArgumentNullException(nameof(calendario));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<QueryResult<TurnoDisponibleDto>> Handle(
            ConsultarDisponibilidadTurnosDiaQuery request,
            CancellationToken cancellationToken)
        {
            var guias = await _repositorioGuia
                .ObtenerGuiasConDisponibilidadAsync();

         

            var visitas = await _repositorioVisitaGuiada.ObtenerConActividadAsync(
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
