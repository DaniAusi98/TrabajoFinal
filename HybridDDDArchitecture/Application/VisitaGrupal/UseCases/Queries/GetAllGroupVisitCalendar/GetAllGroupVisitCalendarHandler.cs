using Application.Common.Repositories;
using Application.VisitaGrupal.DataTransferObjets;
using Application.VisitaGrupal.Repositories;
using Core.Application;

namespace Application.VisitaGrupal.UseCases.Queries.GetAllGroupVisitCalendar
{
internal sealed class GetAllGroupVisitCalendarHandler(
    IRepositorioVisitaGrupalAutoguiada repositorioVisitaGrupalAutoguiada,
    IRepositorioVisitaGuiada repositorioVisitaGuiada,
    IProvinciaRepository repositorioProvincia,
    IDepartamentoRepository repositorioDepartamento,
    ILocalidadRepository repositorioLocalidad
) : IRequestQueryHandler<
            GetAllGroupVisitCalendarQuery,
            QueryResult<VisitaGrupalCalendarDto>>
    {
        private readonly IRepositorioVisitaGuiada _repositorioVisitaGuiada = repositorioVisitaGuiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGuiada));
        private readonly IRepositorioVisitaGrupalAutoguiada _repositorioVisitaGrupalAutoguiada = repositorioVisitaGrupalAutoguiada ?? throw new ArgumentNullException(nameof(repositorioVisitaGrupalAutoguiada));
        private readonly IProvinciaRepository _repositorioProvincia = repositorioProvincia ?? throw new ArgumentNullException(nameof(repositorioProvincia));
        private readonly IDepartamentoRepository _repositorioDepartamento = repositorioDepartamento ?? throw new ArgumentNullException(nameof(repositorioDepartamento));
        private readonly ILocalidadRepository _repositorioLocalidad = repositorioLocalidad ?? throw new ArgumentNullException(nameof(repositorioLocalidad));

        public async Task<QueryResult<VisitaGrupalCalendarDto>> Handle(GetAllGroupVisitCalendarQuery request, CancellationToken cancellationToken)
        {
            var visitasGuiadas = await _repositorioVisitaGuiada.GetAllGroupVisitAsync(request.FechaConsultaDesde, request.FechaConsultaHasta);
            var visitasAutoguiadas = await _repositorioVisitaGrupalAutoguiada.GetAllGroupVisitAuAsync(request.FechaConsultaDesde, request.FechaConsultaHasta);

            // Recolectar ids únicos
            var provinciaIds = visitasGuiadas.Select(v => v.ProvinciaInstitucion)
                .Concat(visitasAutoguiadas.Select(v => v.ProvinciaInstitucion))
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToList();

            var departamentoIds = visitasGuiadas.Select(v => v.DepartamentoInstitucion)
                .Concat(visitasAutoguiadas.Select(v => v.DepartamentoInstitucion))
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToList();

            var localidadIds = visitasGuiadas.Select(v => v.LocalidadInstitucion)
                .Concat(visitasAutoguiadas.Select(v => v.LocalidadInstitucion))
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToList();

            // Consultas en bloque
            var provincias = await _repositorioProvincia.GetByIdsAsync(provinciaIds);
            var departamentos = await _repositorioDepartamento.GetByIdsAsync(departamentoIds);
            var localidades = await _repositorioLocalidad.GetByIdsAsync(localidadIds);

            var mapaProvincias = provincias.ToDictionary(p => p.Id, p => p.Nombre);
            var mapaDepartamentos = departamentos.ToDictionary(d => d.Id, d => d.Nombre);
            var mapaLocalidades = localidades.ToDictionary(l => l.Id, l => l.Nombre);

            var visitasGrupalesDto = new List<VisitaGrupalCalendarDto>();

            foreach (var visitasguiada in visitasGuiadas)
            {
                var slot = visitasguiada.TimeSlots.OrderBy(ts => ts.Inicio).FirstOrDefault();
                visitasGrupalesDto.Add(new VisitaGrupalCalendarDto
                {
                    Id = visitasguiada.Id,
                    Tipo = VisitaGrupalCalendarDto.TipoVisitaGrupal.Guiada,
                    Provincia = mapaProvincias.TryGetValue(visitasguiada.ProvinciaInstitucion, out var pName) ? pName : visitasguiada.ProvinciaInstitucion,
                    Departamento = mapaDepartamentos.TryGetValue(visitasguiada.DepartamentoInstitucion, out var dName) ? dName : visitasguiada.DepartamentoInstitucion,
                    Localidad = mapaLocalidades.TryGetValue(visitasguiada.LocalidadInstitucion, out var lName) ? lName : visitasguiada.LocalidadInstitucion,
                    CantidadPersonas = visitasguiada.CantidadPersonas ?? 0,
                    Institucion = visitasguiada.Institucion,
                    DiversidadFuncional = visitasguiada.DiversidadFuncionalDescripcion,
                    HoraInicio = slot?.Inicio,
                    HoraFin = slot?.Fin
                });
            }

            foreach (var visitaAutoguiada in visitasAutoguiadas)
            {
                var slotAuto = visitaAutoguiada.TimeSlots.OrderBy(ts => ts.Inicio).FirstOrDefault();
                visitasGrupalesDto.Add(new VisitaGrupalCalendarDto
                {
                    Id = visitaAutoguiada.Id,
                    Tipo = VisitaGrupalCalendarDto.TipoVisitaGrupal.Autoguiada,
                    Provincia = mapaProvincias.TryGetValue(visitaAutoguiada.ProvinciaInstitucion, out var pName2) ? pName2 : visitaAutoguiada.ProvinciaInstitucion,
                    Departamento = mapaDepartamentos.TryGetValue(visitaAutoguiada.DepartamentoInstitucion, out var dName2) ? dName2 : visitaAutoguiada.DepartamentoInstitucion,
                    Localidad = mapaLocalidades.TryGetValue(visitaAutoguiada.LocalidadInstitucion, out var lName2) ? lName2 : visitaAutoguiada.LocalidadInstitucion,
                    CantidadPersonas = visitaAutoguiada.CantidadPersonas ?? 0,
                    Institucion = visitaAutoguiada.Institucion,
                    DiversidadFuncional = visitaAutoguiada.DiversidadFuncional,
                    HoraInicio = slotAuto?.Inicio,
                    HoraFin = slotAuto?.Fin
                });
            }

            long totalElementos = visitasGrupalesDto.Count;
            uint tamañoPagina = (uint)visitasGrupalesDto.Count;
            uint indicePagina = 1;

            return new QueryResult<VisitaGrupalCalendarDto>(
                visitasGrupalesDto,
                totalElementos,
                tamañoPagina,
                indicePagina
            );
        }
    }
}
