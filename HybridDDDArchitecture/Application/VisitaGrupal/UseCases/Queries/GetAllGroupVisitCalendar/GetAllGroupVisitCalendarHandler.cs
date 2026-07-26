using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.GetAllGroupVisitCalendar
{
    internal sealed class GetAllGroupVisitCalendarHandler(IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada,IRepositorioVisitaGuiada repositorioVisitaGuiada) : IRequestQueryHandler<
            GetAllGroupVisitCalendarQuery,
            QueryResult<VisitaGrupalCalendarDto>>
    {
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada=repositorioVisitaGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada = repositorioVisitaGrupalAutoguiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGrupalAutoguiada));

        public async Task<QueryResult<VisitaGrupalCalendarDto>> Handle(GetAllGroupVisitCalendarQuery request, CancellationToken cancellationToken)
        {
            var visitasGuiadas = await _repositorioVisitaGuiada.GetAllGroupVisitAsync(request.FechaConsultaDesde, request.FechaConsultaHasta);
            var visitasAutoguiadas = await _repositorioVisitaGrupalAutoguiada.GetAllGroupVisitAuAsync(request.FechaConsultaDesde, request.FechaConsultaHasta);
            List<VisitaGrupalCalendarDto> visitasGrupalesDto = [];

            foreach (var visitasguiada in visitasGuiadas)
            {
                visitasGrupalesDto.Add(new VisitaGrupalCalendarDto
                {

                    Id = visitasguiada.Id,
                    Tipo = "VisitaGrupalGuiada",
                    IdProvincia = visitasguiada.ProvinciaInstitucion,
                    IdLocalidad = visitasguiada.LocalidadInstitucion,
                    CantidadPersonas = visitasguiada.CantidadPersonas ?? 0,
                    Institucion = visitasguiada.Institucion,
                    DiversidadFuncional = visitasguiada.DiversidadFuncionalDescripcion,
                    HoraInicio = visitasguiada.TimeSlots.FirstOrDefault().Inicio,
                    HoraFin = visitasguiada.TimeSlots.FirstOrDefault().Fin

                });

            }
            foreach (var visitaAutoguiada in visitasAutoguiadas)
            {
                visitasGrupalesDto.Add(new VisitaGrupalCalendarDto
                {

                    Id = visitaAutoguiada.Id,
                    Tipo = "VisitaGrupalAutoguiada",
                    IdProvincia = visitaAutoguiada.ProvinciaInstitucion,
                    IdLocalidad = visitaAutoguiada.CiudadInstitucion,
                    CantidadPersonas = visitaAutoguiada.CantidadPersonas ?? 0,
                    Institucion = visitaAutoguiada.Institucion,
                    DiversidadFuncional = visitaAutoguiada.DiversidadFuncional,
                    HoraInicio = visitaAutoguiada.TimeSlots.FirstOrDefault().Inicio,
                    HoraFin = visitaAutoguiada.TimeSlots.FirstOrDefault().Fin

                });

            }


            long totalElementos = visitasGrupalesDto.Count;
            uint tamañoPagina = (uint)visitasGrupalesDto.Count; // Si no manejas paginación aún, envías el total de la lista
            uint indicePagina = 1; // Página inicial por defecto

            // Retornamos pasando exactamente los 4 parámetros requeridos por tu arquitectura
            return new QueryResult<VisitaGrupalCalendarDto>(
                visitasGrupalesDto,
                totalElementos,
                tamañoPagina,
                indicePagina
            );
        }
    }
}
